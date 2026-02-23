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
    partial class Model2TrafficLight : TrafficLight
    {
        // Свойства
        private static ModelForm ModelForm { get; set; } = null; // Форма модели
        // Конструкторы
        public Model2TrafficLight(TrafficLight tl) : base(tl.ToString()) { }
        /// <summary>
        /// Связывание с моделью
        /// </summary>
        public static void BindingWithModel(ModelForm modelForm)
        {
            ModelForm = modelForm; // Задаем модель
            // Конвертируем объекты TrafficLight в объекты Model2TrafficLight
            for (int i = 0; i < TLsList.Count; i++)
            {
                // В списке элемент класса TrafficLight? - приведение к Model2TrafficLight
                if (TLsList[i] is TrafficLight)
                    TLsList[i] = new Model2TrafficLight(TLsList[i]);
            }
            Initialization();
        }
    }
    /// <summary>
    /// Часть, код в которой можно только добавлять к уже существующим методам
    /// </summary>
    partial class Model2TrafficLight : TrafficLight
    {
        // Свойства
        public static string ModelName { get; private set; } = "Модель 2 - АСРС"; // Название для модели
        /// <summary>
        /// Инициализация свойств (перегрузка)
        /// </summary>
        public new static void Initialization()
        {
            TrafficLight.Initialization(); // Инициализация базовых свойств
            // TODO: здесь будет инициализация новых свойств или изменение старых (статические методы)
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
    partial class Model2TrafficLight
    {
        private int EstimatedTime { get; set; } = 0; // Оценочное время для автоколонны
        private bool Synchronized { get; set; } = false; // Флаг синхронизации
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
        /// Вычисление оценочного времени автоколонны для светофора
        /// </summary>
        private void EstimatedTimeCalculation()
        {
            // Инициализация и заполнение списка светофоров, для которых вычисляется оценочное время
            List<Model2TrafficLight> calculatingTLs = new List<Model2TrafficLight>
            {
                this
            };
            // Добавляем параллельные светофоры к списку для вычисления
            foreach (Model2TrafficLight m2tl in ParallelTLsList)
                calculatingTLs.Add(m2tl);
            // Вычисление оценочного времени
            foreach (Model2TrafficLight m2tl in calculatingTLs)
            {
                m2tl.EstimatedTime = 0;
                foreach (Auto a in Auto.AutosList)
                {
                    if (m2tl.IsInSight(a))
                    {
                        string type = a.GetType().ToString().Replace("PSMoTR.Classes.", ""); // Получаем имя класса объекта
                        AutosEstatementTimeDictionary.TryGetValue(type, out int time); // Получаем из списка значений оценочное время для объекта
                        m2tl.EstimatedTime += time;
                    }
                }
            }
            calculatingTLs.Clear();
        }
        /// <summary>
        /// Вычисление длительности зеленого сигнала в следующей итерации работы светофора
        /// </summary>
        private void CalculateGD()
        {
            // Выбор приоритетного светофора для синхронизации среди текущего и параллельных
            Model2TrafficLight priorityTL = this;
            foreach (Model2TrafficLight parallelTL in priorityTL.ParallelTLsList)
            {
                if (parallelTL.EstimatedTime > priorityTL.EstimatedTime)
                    priorityTL = parallelTL;
            }
            // Изменение длительности зеленого сигнала у текущего и параллельных на значение приоритетного светофора
            GD = priorityTL.EstimatedTime;
            Synchronized = true;
            foreach (Model2TrafficLight parallelTL in ParallelTLsList)
            {
                parallelTL.GD = priorityTL.EstimatedTime;
                parallelTL.Synchronized = true;
            }
        }
    }
}
