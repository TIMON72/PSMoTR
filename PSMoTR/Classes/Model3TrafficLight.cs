using PSMoTR.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PSMoTR.Classes
{
    /// <summary>
    /// Часть, код в которой не следует изменять
    /// </summary>
    partial class Model3TrafficLight : TrafficLight
    {
        // Свойства
        static ModelForm ModelForm { get; set; } = null; // Форма модели
        // Конструкторы
        public Model3TrafficLight(TrafficLight tl) : base(tl.ToString()) { }
        /// <summary>
        /// Связывание с моделью
        /// </summary>
        public static void BindingWithModel(ModelForm modelForm)
        {
            ModelForm = modelForm; // Задаем модель
            // Конвертируем объекты TrafficLight в объекты Model3TrafficLight
            for (int i = 0; i < TLsList.Count; i++)
            {
                // В списке элемент класса TrafficLight? - приведение к Model3TrafficLight
                if (TLsList[i] is TrafficLight)
                    TLsList[i] = new Model3TrafficLight(TLsList[i]);
            }
            Initialization();
        }
    }
    /// <summary>
    /// Часть, код в которой можно только добавлять к уже существующим методам
    /// </summary>
    partial class Model3TrafficLight : TrafficLight
    {
        // Свойства
        public static string ModelName { get; private set; } = "Модель 3 - ВАСРС"; // Название для модели
        /// <summary>
        /// Инициализация свойств (перегрузка)
        /// </summary>
        public new static void Initialization()
        {
            TrafficLight.Initialization(); // Инициализация базовых свойств
            // TODO: здесь будет инициализация новых свойств или изменение старых (статические методы)
            FindInterconnectedTL(TLsList);
        }
        public override void PropertiesInitialization()
        {
            base.PropertiesInitialization(); // Инициализация базовых свойств
            // TODO: здесь будет инициализация новых свойств или изменение старых
            CurrentSC = Color.Orange;
            TimerCounter = OD;
        }
        /// <summary>
        /// Событие тика таймера (перегрузка)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Timer_Tick(object sender, EventArgs e)
        {
            // TODO: здесь добавляются новые действия, связанные с таймером светофора
            // Сигнал стал оранжевым?
            if (TimerCounter == 5 && !Synchronized)
            {
                // Текущий сигнал красный?
                if (PreviousSC == Color.Green)
                {
                    EstimatedTimeCalculation();
                    CalculateGD();
                }
            }
            // Время сигнала вышло? - сбрасываем флаг синхронизации
            else if (TimerCounter == 1 && Synchronized)
                Synchronized = false;
            // Вызов базового события
            base.Timer_Tick(sender, e);
        }
    }
    /// <summary>
    /// Часть класса, в которой реализуются новый алгоритм
    /// </summary>
    partial class Model3TrafficLight
    {
        // Свойства
        private int EstimatedTime { get; set; } = 0; // Оценочное время для автоколонны
        private bool Synchronized { get; set; } = false; // Флаг синхронизации
        private Model3TrafficLight InterconnectedTL { get; set; } = null; // Взаимодействующий светофор, с которым происходит обмен информацией
        // Списки
        private List<Auto> AutosRowList { get; set; } = new List<Auto>(); // Список автоколонны перед светофором
        /// <summary>
        /// Рекурсивный поиск связанных светофоров и вычисление длины секции дороги
        /// </summary>
        /// <param name="currentRoute"></param>
        private static void FindInterconnectedTL(List<TrafficLight> TLsList)
        {
            foreach (Model3TrafficLight m3tl in TLsList)
                m3tl.FindInterconnectedTL(m3tl.BaseRoute);
        }
        private void FindInterconnectedTL(Route currentRoute)
        {
            List<Route> nextRoutes = Route.RoutesList.FindAll(r => r.Start.Equals(currentRoute.End));
            // Следующий маршрут есть?
            if (nextRoutes.Count == 0)
            {
                return;
            }
            else
            {
                foreach (Route nextRoute in nextRoutes)
                {
                    int foundIndex = TLsList.FindIndex(tl => tl.BaseRoute.Equals(nextRoute)); // Поиск светофора на маршруте
                    // Светофор найден?
                    if (foundIndex != -1)
                    {
                        Model3TrafficLight foundTL = (Model3TrafficLight)TLsList[foundIndex];
                        // Светофор расположен на ограниченной секции дороги?
                        if (foundTL.LimitedSection)
                        {
                            // Нынешний взаимодействующий светофор неопределен или поле зрения его меньше, чем у найденного?
                            if (InterconnectedTL is null || foundTL.OL > InterconnectedTL.OL)
                                InterconnectedTL = foundTL;
                        }
                        return;
                    }
                    else
                        FindInterconnectedTL(nextRoute);
                }
            }
        }
        /// <summary>
        /// Проверка нахождения авто в поле зрения
        /// </summary>
        /// <param name="targetAuto"></param>
        /// <returns></returns>
        private bool IsInSight(Auto targetAuto)
        {
            using (GraphicsPath gp_overlook = new GraphicsPath())
            {
                gp_overlook.AddPolygon(Overlook);
                if (targetAuto != null && gp_overlook.IsVisible(targetAuto.X, targetAuto.Y))
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Вычисление оценочного времени автоколонны для светофора и ее длины
        /// </summary>
        private void EstimatedTimeCalculation()
        {
            // Инициализация и заполнение списка светофоров, для которых вычисляется оценочное время
            List<Model3TrafficLight> calculatingTLs = new List<Model3TrafficLight>
            {
                this
            };
            // Есть ли взаимодействующий светофор у текущего? - добавляем светофоры к списку для вычисления
            if (InterconnectedTL != null)
                calculatingTLs.Add(InterconnectedTL);
            // Добавляем параллельные светофоры к списку для вычисления
            foreach (Model3TrafficLight m3tl in ParallelTLsList)
                calculatingTLs.Add(m3tl);
            // Вычисление оценочного времени и длины автоколонны
            foreach (Model3TrafficLight m3tl in calculatingTLs)
            {
                m3tl.EstimatedTime = 0;
                m3tl.AutosRowList.Clear();
                // Поиск тех авто, что в поле зрения
                foreach (Auto a in Auto.AutosList)
                {
                    if (m3tl.IsInSight(a))
                    {
                        // Добавляем текущее авто в автоколонну
                        m3tl.AutosRowList.Add(a);
                        // Вычисляем оценочное время
                        string type = a.GetType().ToString().Replace("PSMoTR.Classes.", ""); // Получаем имя класса объекта
                        AutosEstatementTimeDictionary.TryGetValue(type, out int time); // Получаем из списка значений оценочное время для объекта
                        m3tl.EstimatedTime += time;
                    }
                }
            }
            calculatingTLs.Clear();
        }
        /// <summary>
        /// Получить длину потока авто
        /// </summary>
        /// <returns></returns>
        private float GetAutosRowLength()
        {
            float length = 0;
            foreach (Auto a in AutosRowList)
                length += a.Length + a.CriticalDistance;
            return length;
        }
        /// <summary>
        /// Проверка взаимодействующего светофора на перегруженность авто
        /// </summary>
        /// <returns></returns>
        private bool IsITLCongested()
        {
            int number = Number;
            // Взаимодействующий светофор есть у текущего и автоколонна не пуста?
            if (InterconnectedTL != null && AutosRowList.Count != 0)
            {
                float freeSpaceLength = InterconnectedTL.OL - InterconnectedTL.GetAutosRowLength();
                // Длина автоколонны текущего меньше свободного пространства взаимодействующего?
                if (GetAutosRowLength() <= freeSpaceLength)
                    return false;
                else
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Поправка оценочного времени
        /// </summary>
        private void EstimatedTimeCorrection()
        {
            // Пока оценочное время не отрегулируется под ситуацию перед взаимодействующим светофором - убавляем
            while (IsITLCongested())
            {
                // Ищем самое дальнее авто от светофора
                Auto distantAuto = null;
                float maxDistance = 0;
                foreach (Auto a in AutosRowList)
                {
                    float currentDistance = Point.Distance(new Point(a.X, a.Y), new Point(Position.X, Position.Y));
                    if (currentDistance > maxDistance)
                    {
                        maxDistance = currentDistance;
                        distantAuto = a;
                    }
                }
                // Убавляем оценочное время и убираем самое дальнее авто от светофора 
                string type = distantAuto.GetType().ToString().Replace("PSMoTR.Classes.", ""); // Получаем имя класса объекта
                AutosEstatementTimeDictionary.TryGetValue(type, out int time); // Получаем из списка значений оценочное время для объекта
                EstimatedTime -= time;
                AutosRowList.Remove(distantAuto);
            }
        }
        /// <summary>
        /// Вычисление длительности зеленого сигнала в следующей итерации работы светофора
        /// </summary>
        private void CalculateGD()
        {
            // Выбор приоритетного светофора для синхронизации среди текущего и параллельных
            Model3TrafficLight priorityTL = this;
            foreach (Model3TrafficLight parallelTL in priorityTL.ParallelTLsList)
            {
                if (parallelTL.EstimatedTime > priorityTL.EstimatedTime)
                    priorityTL = parallelTL;
            }
            // Поправка оценочного времени приоритетного светофора
            priorityTL.EstimatedTimeCorrection();
            // Изменение длительности зеленого сигнала у текущего и параллельных на значение приоритетного светофора
            GD = priorityTL.EstimatedTime;
            Synchronized = true;
            foreach (Model3TrafficLight parallelTL in ParallelTLsList)
            {
                parallelTL.GD = priorityTL.EstimatedTime;
                parallelTL.Synchronized = true;
            }
        }
    }
}
