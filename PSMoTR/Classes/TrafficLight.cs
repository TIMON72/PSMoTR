using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PSMoTR.Forms;

namespace PSMoTR.Classes
{
    public class TrafficLight
    {
        // Поля
        private int greenDuration = 0; // Длительность зеленого сигнала
        private int redDuration = 0; // Длительность красного сигнала
        // Свойства
        public Point Position { get; set; } // Позиция светофора
        public Bitmap Icon { get; set; } = Properties.Resources.image_circle; // Иконка
        public string Type { get; set; } // Тип
        public int GD
        {
            get
            {
                return greenDuration;
            }
            set
            {
                greenDuration = value;
                SignalDurationChanged?.Invoke(this, new TrafficLightEventArgs(greenDuration, Color.Green));
                if (Timer is null && PreviousSC == Color.Red)
                    TimerCounter = GD + OD;
            }
        } // GreenDuration - Длительность зеленого сигнала
        public int OD { get; set; } = 5; // OrangeDuration - Длительность оранжевого сигнала
        public int RD
        {
            get
            {
                return redDuration;
            }
            set
            {
                redDuration = value;
                SignalDurationChanged?.Invoke(this, new TrafficLightEventArgs(redDuration, Color.Red));
                if (Timer is null && PreviousSC == Color.Green)
                    TimerCounter = RD + OD;
            }
        } // RedDuration - Длительность красного сигнала
        public int SkipD { get; set; } = 0; // Уменьшение длительности сигнала
        public Color StartSC { get; set; } // StartSignalColor - Начальный цвет сигнала
        public Color CurrentSC { get; set; } // CurrentSignalColor - Текущий сигнал
        public Color PreviousSC { get; set; } // PreviousSignalColor - Предыдущий сигнал
        public Timer Timer { get; set; } = null; // Таймер
        public int TimerCounter { get; set; } = 0; // Счетчик таймера
        public Route BaseRoute { get; set; } // Маршрут, на котором расположен светофор
        public PointF[] Overlook { get; set; } = new PointF[4]; // Поле зрения
        public float OL { get; set; } = 70; // OverlookLength - Длина поля зрения
        public bool LimitedSection { get; set; } = false; // Ограниченный участок дороги
        public int Number
        {
            get
            {
                return TLsList.IndexOf(this);
            }
        } // Номер в списке
        // Списки
        public static List<TrafficLight> TLsList { get; set; } = new List<TrafficLight>();
        public List<Route> NextRoutes { get; set; } = new List<Route>(); // Маршруты, которые контролирует светофор
        public List<TrafficLight> ParallelTLsList { get; set; } = new List<TrafficLight>(); // Список встречных светофоров (сигналы совпадают)
        public List<TrafficLight> PerpendicularTLsList { get; set; } = new List<TrafficLight>(); // Список перпендикулярных светофоров (сигналы противоположны)
        public static Dictionary<string, int> AutosEstatementTimeDictionary { get; set; } = new Dictionary<string, int>
        {
            { "Auto", 3 },
            { "Car", 5 },
            { "Bus", 10 }
        }; // Словарь ожидаемого времени для авто
        // События
        public event TrafficLightEventHandler SignalDurationChanged; // Изменение длительности сигнала
        // Конструкторы
        public TrafficLight() { }
        public TrafficLight(Point point)
        {
            Position = point;
            Type = "usual";
            GD = 0;
            RD = 0;
            StartSC = Color.Gray;
            CurrentSC = StartSC;
            OL = 0;
            LimitedSection = false;
        }
        public TrafficLight(string str)
        {
            try
            {
                str = str.Replace("X=", "");
                str = str.Replace("Y=", "");
                str = str.Replace("Speed=", "");
                str = str.Replace("Type=", "");
                str = str.Replace("GD=", "");
                str = str.Replace("RD=", "");
                str = str.Replace("SSC=", "");
                str = str.Replace("OL=", "");
                str = str.Replace("LS=", "");
                str = str.Replace(" ", "");
                string[] buff = str.Split(';');
                Position = new Point(float.Parse(buff[0]), float.Parse(buff[1]), float.Parse(buff[2]));
                Type = buff[3];
                GD = Convert.ToInt32(buff[4]);
                RD = Convert.ToInt32(buff[5]);
                StartSC = Color.FromName(buff[6]);
                CurrentSC = StartSC;
                SignalDurationChanged += SignalTiming; // Подписываем событию метод синхронизации сигналов
                OL = Convert.ToSingle(buff[7]);
                LimitedSection = Convert.ToBoolean(buff[8]);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка преобразования данных в объект\n" + str, "Ошибка формата");
            }
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public TrafficLight Clone()
        {
            TrafficLight clone = new TrafficLight
            {
                Position = Position.Clone(),
                Type = Type,
                GD = GD,
                RD = RD,
                StartSC = StartSC,
                CurrentSC = CurrentSC,
                OL = OL
            };
            clone.LimitedSection = clone.LimitedSection;
            return clone;
        }
        /// <summary>
        /// Добавление нового светофора в список с соблюдением целостности
        /// </summary>
        /// <param name="tl_new"></param>
        /// <returns></returns>
        public static bool List_SafelyAdd(TrafficLight tl_new)
        {
            // Сопоставление нового светофора с существующими светофорами
            TrafficLight similarTL = TLsList.Find(tl => tl != tl_new &&
                Point.Distance(tl.Position, tl_new.Position) < 2);
            // Уже рядом есть светофор?
            if (similarTL != null)
                return false;
            // Инициализация свойств добавляемого светофора
            tl_new.PropertiesInitialization();
            // Базовый маршрут не определился? : не добавляем светофор
            if (tl_new.BaseRoute is null)
                return false;
            // Добавление в список
            TLsList.Add(tl_new);
            return true;
        }
        /// <summary>
        /// Инициализация свойств всех объектов класса
        /// </summary>
        public virtual void PropertiesInitialization()
        {
            // Загружаем иконку из ресурсов
            if (Type == "usual")
                Icon = Properties.Resources.image_circle;
            else if (Type == "right")
                Icon = Properties.Resources.image_arrow_right;
            else if (Type == "left")
                Icon = Properties.Resources.image_arrow_left;
            else if (Type == "up")
                Icon = Properties.Resources.image_arrow_up;
            else if (Type == "down")
                Icon = Properties.Resources.image_arrow_down;
            else
                Icon = Properties.Resources.image_circle;
            // Замена серого сигнала на красный (при редактировании)
            if (StartSC == Color.Gray)
                StartSC = Color.Red;
            // Назначение текущего сигнала цветом начальным 
            CurrentSC = StartSC;
            // Задаем значение свойства CurrentSC
            if (CurrentSC == Color.Red)
            {
                TimerCounter = OD + RD - SkipD;
                PreviousSC = Color.Green;
            }
            else if (CurrentSC == Color.Green)
            {
                TimerCounter = OD + GD - SkipD;
                PreviousSC = Color.Red;
            }
            // Задаем значение свойства BaseRoute
            // Поиск маршрута, на котором расположен светофор (поиск прямой, которой принадлежит точка)
            BaseRoute = Route.RoutesList.Find(r => r.IsContainPoint(Position) &&
                !(Position.Equals(r.Start) || Position.Equals(r.End)));
            // Задаем значение свойства NextRoutes
            // Поиск маршрутов, которые идут после того, на котором расположен светофор
            if (Type == "usual")
                NextRoutes = Route.RoutesList.FindAll(r => BaseRoute != null &&
                    r.Start.X == BaseRoute.End.X && r.Start.Y == BaseRoute.End.Y);
            else if (Type == "up")
                NextRoutes = Route.RoutesList.FindAll(r => BaseRoute != null &&
                    r.Start.X == BaseRoute.End.X && r.Start.Y == BaseRoute.End.Y && r.Course[1] == "up");
            else if (Type == "down")
                NextRoutes = Route.RoutesList.FindAll(r => BaseRoute != null &&
                    r.Start.X == BaseRoute.End.X && r.Start.Y == BaseRoute.End.Y && r.Course[1] == "down");
            else if (Type == "right")
                NextRoutes = Route.RoutesList.FindAll(r => BaseRoute != null &&
                    r.Start.X == BaseRoute.End.X && r.Start.Y == BaseRoute.End.Y && r.Course[1] == "right");
            else if (Type == "left")
                NextRoutes = Route.RoutesList.FindAll(r => BaseRoute != null &&
                    r.Start.X == BaseRoute.End.X && r.Start.Y == BaseRoute.End.Y && r.Course[1] == "left");
            // Определяем свойство Overlook
            if (BaseRoute != null)
            {
                Overlook = new PointF[]
                {
                    new PointF(Position.X, Position.Y - (float)1.5),
                    new PointF(Position.X - OL, Position.Y - (float)1.5),
                    new PointF(Position.X - OL, Position.Y + (float)1.5),
                    new PointF(Position.X, Position.Y + (float)1.5)
                };
                PointF center = new PointF(Position.X, Position.Y); // Центр трансформирования
                float rotationRad = BaseRoute.Rotation * (float)Math.PI / 180; // Конвертация угла поворота в радианы
                for (int i = 0; i < Overlook.Length; i++)
                {
                    float x;
                    float y;
                    x = (float)((Overlook[i].X - center.X) * Math.Cos(rotationRad) - (Overlook[i].Y - center.Y) * Math.Sin(rotationRad)
                        + center.X); // Изменение координаты X
                    y = (float)((Overlook[i].X - center.X) * Math.Sin(rotationRad) + (Overlook[i].Y - center.Y) * Math.Cos(rotationRad)
                        + center.Y); // Изменение координаты Y
                    Overlook[i] = new PointF(x, y);
                }
            }
        }
        public static void Initialization()
        {
            for (int i = 0; i < TLsList.Count; i++)
            {
                TrafficLight tl = TLsList[i];
                tl.PropertiesInitialization();
            }
            FindDependentTLs();
        }
        /// <summary>
        /// Старт таймера
        /// </summary>
        public void TimerStart()
        {
            // Запускаем таймер
            Timer = new Timer
            {
                Interval = 1000,
            };
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Enabled = true;
            Timer.Start();
        }
        /// <summary>
        /// Пауза таймера
        /// </summary>
        public void TimerPause()
        {
            if (Timer.Enabled)
                Timer.Stop();
            else
                Timer.Start();

        }
        /// <summary>
        /// Остановка таймера
        /// </summary>
        public void TimerStop()
        {
            Timer.Stop();
            Timer.Enabled = false;
        }
        /// <summary>
        /// Действия с таймером
        /// </summary>
        /// <param name="action"></param>
        public static void TimersAction(string action, ModelForm modelForm)
        {
            foreach (TrafficLight tl in TLsList)
            {
                if (action == "start")
                    tl.TimerStart();
                else if (action == "pause/continue")
                    tl.TimerPause();
                else if (action == "stop")
                    tl.TimerStop();
            }
            if (action == "stop")
            {
                TLsList.Clear();
                modelForm.TrafficLightsInitialization();
            }
        }
        /// <summary>
        /// Событие тика таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Timer_Tick(object sender, EventArgs e)
        {
            TimerCounter--;
            if (TimerCounter == 5)
            {
                CurrentSC = Color.Orange;
                Position.Speed = 0;
            }
            else if (TimerCounter == 0)
            {
                // Меняем текущий сигнал
                if (PreviousSC == Color.Red && RD != 0)
                {
                    CurrentSC = Color.Red;
                    Position.Speed = 0;
                    TimerCounter = OD + RD;
                }
                else if (PreviousSC == Color.Green && GD != 0)
                {
                    CurrentSC = Color.Green;
                    Position.Speed = BaseRoute.End.Speed;
                    TimerCounter = OD + GD;
                }
                else
                {
                    TimerCounter = OD;
                }
                // Меняем предыдущий сигнал
                if (PreviousSC == Color.Green)
                    PreviousSC = Color.Red;
                else if (PreviousSC == Color.Red)
                    PreviousSC = Color.Green;
            }
        }
        /// <summary>
        /// Перевод в string (перегрузка)
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            "X=" + Position.X + "; Y=" + Position.Y + "; Speed=" + Position.Speed +
            "; Type=" + Type + "; GD=" + GD + "; RD=" + RD + "; SSC=" + StartSC.Name.ToString() +
            "; OL=" + OL + "; LS=" + LimitedSection;
        /// <summary>
        /// Сортировка
        /// </summary>
        public static void Sort() => TLsList = TLsList.OrderBy(tl => tl.Position.X).ThenBy(tl => tl.Position.Y).ToList();
        /// <summary>
        /// Открытие файла
        /// </summary>
        public static bool OpenFile(string path)
        {
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                TLsList.Clear();
                if (file.Length == 0) // Если файл пустой, то загружаем в него содержимое из шаблона в ресурсах
                {
                    string[] reader = Properties.Resources.traffic_lights.Replace("\r", "").Split('\n');
                    for (int i = 0; i < reader.Length; i++)
                        TLsList.Add(new TrafficLight(reader[i]));
                }
                else // Иначе считываем файл
                {
                    StreamReader reader = new StreamReader(file);
                    while (!reader.EndOfStream)
                    {
                        string str = reader.ReadLine();
                        if (str.Length == 0)
                            continue;
                        TLsList.Add(new TrafficLight(str));
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Перезапись файла
        /// </summary>
        public static void RewriteFile(string path)
        {
            Sort();
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (TrafficLight tl in TLsList)
                    writer.WriteLine(tl.ToString());
            }
        }
        /// <summary>
        /// Поиск влияющих маршрутов
        /// </summary>
        /// <returns></returns>
        public List<Route> FindInfluentialRoutes()
        {
            List<Route> influentialRoutes = new List<Route>();
            // Поиск пересекающих маршрутов у маршрутов, следующих за светофором
            for (int i = 0; i < NextRoutes.Count; i++)
            {
                if (influentialRoutes.Count > 0)
                {
                    influentialRoutes.AddRange(Route.FindCrossingRoutes(NextRoutes[i]));
                    // Убираем повторяющиеся влияющие маршруты
                    for (int j = 0; j < influentialRoutes.Count; j++)
                        for (int k = 0; k < influentialRoutes.Count; k++)
                            if (influentialRoutes[k].Number == influentialRoutes[j].Number && k != j)
                            {
                                influentialRoutes.RemoveAt(k);
                                k--;
                            }
                }
                else
                    influentialRoutes.AddRange(Route.FindCrossingRoutes(NextRoutes[i]));
                // Поиск и удаление влияющих маршрутов, исходящих из начала или конца текущего или являющихся текущим
                List<Route> results = influentialRoutes.FindAll(ir => ir == BaseRoute || ir.Start.Equals(NextRoutes[i].Start) || ir.Start.Equals(NextRoutes[i].End));
                if (results != null)
                    foreach (Route r in results)
                        influentialRoutes.Remove(r);
                // Убираем влияющие маршруты, имеющие продолжение в виде другого влияющего маршрута
                bool end = false;
                while (!end)
                {
                    end = true;
                    foreach (Route ir in influentialRoutes)
                    {
                        Route result = influentialRoutes.FirstOrDefault(ir_into => ir_into.Start.Equals(ir.End));
                        // Пересекающий маршрут имеет продолжение в виде другого пересекающего маршрута?
                        if (result != null)
                        {
                            influentialRoutes.Remove(ir);
                            end = false;
                            break;
                        }

                    }
                }
            }
            return influentialRoutes;
        }
        /// <summary>
        /// Поиск зависимых светофоров и разделение их на встречные и влияющие
        /// </summary>
        public static void FindDependentTLs()
        {
            // Поиск перпендикулярных и параллельных (пересекающих) светофоров
            foreach (TrafficLight tl in TLsList)
            {
                if (tl.BaseRoute is null)
                    continue;
                if (tl.ParallelTLsList.Count != 0)
                    tl.ParallelTLsList.Clear();
                if (tl.PerpendicularTLsList.Count != 0)
                    tl.PerpendicularTLsList.Clear();
                // Поиск влияющих и встречных светофоров для рассматриваемого светофора через влияющие маршруты
                List<Route> influentialRoutes = tl.FindInfluentialRoutes();
                foreach (TrafficLight dependentTL in TLsList)
                {
                    if (dependentTL.BaseRoute is null)
                        continue;
                    // В влияющих маршрутах есть такие, начало которых совпадает с концом базового маршрута потенциально зависимого светофора?
                    if (influentialRoutes.Any(ir => ir.Start.Equals(dependentTL.BaseRoute.End)))
                    {
                        if (tl.BaseRoute.Course[0] == "right" && dependentTL.BaseRoute.Course[0] == "left" && !tl.ParallelTLsList.Contains(dependentTL))
                            tl.ParallelTLsList.Add(dependentTL);
                        else if (tl.BaseRoute.Course[0] == "left" && dependentTL.BaseRoute.Course[0] == "right" && !tl.ParallelTLsList.Contains(dependentTL))
                            tl.ParallelTLsList.Add(dependentTL);
                        else if (tl.BaseRoute.Course[0] == "up" && dependentTL.BaseRoute.Course[0] == "down" && !tl.ParallelTLsList.Contains(dependentTL))
                            tl.ParallelTLsList.Add(dependentTL);
                        else if (tl.BaseRoute.Course[0] == "down" && dependentTL.BaseRoute.Course[0] == "up" && !tl.ParallelTLsList.Contains(dependentTL))
                            tl.ParallelTLsList.Add(dependentTL);
                        else if (!tl.PerpendicularTLsList.Contains(dependentTL) && !tl.ParallelTLsList.Contains(dependentTL))
                            tl.PerpendicularTLsList.Add(dependentTL);
                    }
                }
            }
            // Поиск оставшихся параллельных непересекающих светофоров
            foreach (TrafficLight tl in TLsList)
            {
                // Поиск параллельных светофоров через влияющие
                foreach (TrafficLight perpendicularTL in tl.PerpendicularTLsList)
                {
                    // Поиск параллельных светофоров для текущего среди перпендикулярных у рассматриваемого перпендикулярного, которые не входят в список параллельных
                    List<TrafficLight> parallelTLs = perpendicularTL.PerpendicularTLsList.FindAll(affectingTL_into => affectingTL_into != tl &&
                                                                                                    !tl.ParallelTLsList.Contains(affectingTL_into));
                    if (parallelTLs.Count != 0)
                        tl.ParallelTLsList.AddRange(parallelTLs);
                }
            }
        }
        /// <summary>
        /// Событие синхронизации таймеров зависимых светофоров у текущего объекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SignalTiming(object sender, TrafficLightEventArgs e)
        {
            if (sender is TrafficLight targetTL)
            {
                if (e.SignalColor == Color.Green)
                {
                    if (targetTL.PerpendicularTLsList.Count > 0)
                        foreach (TrafficLight perpendicularTL in targetTL.PerpendicularTLsList)
                        {
                            // Длительность красного сигнала у перпендикулярного меньше новой длительности зеленого у целевого?
                            // (увеличили длительность зеленого сигнала у текущего)
                            // - увеличим длительность красного сигнала у перпендикулярного
                            if (perpendicularTL.RD < e.SignalDuration)
                            {
                                perpendicularTL.SignalDurationChanged = null;
                                perpendicularTL.RD = e.SignalDuration;
                                perpendicularTL.SignalDurationChanged += SignalTiming;
                            }
                            // Длительность красного сигнала у перпендикулярного больше новой длительности зеленого у целевого?
                            // (уменьшили длительность зеленого сигнала у целевого)
                            else if (perpendicularTL.RD > e.SignalDuration)
                            {
                                int max = e.SignalDuration;
                                // Поиск максимальной длительности сигнала среди параллельных светофоров целевого
                                if (targetTL.ParallelTLsList.Count > 0)
                                    max = targetTL.ParallelTLsList.Max(parallelTL => parallelTL.GD);
                                // Максимальная длительность зеленого параллельных светофоров не превосходит
                                // значения длительности красного перпендикулярного?
                                // - уменьшим длительность этого красного сигнала до полученного максимального значения
                                if (max < perpendicularTL.RD)
                                {
                                    perpendicularTL.SignalDurationChanged = null;
                                    perpendicularTL.RD = max;
                                    perpendicularTL.SignalDurationChanged += SignalTiming;
                                }
                            }
                        }
                }
                else if (e.SignalColor == Color.Red)
                {
                    foreach (TrafficLight perpendicularTL in targetTL.PerpendicularTLsList)
                    {
                        // Длительность зеленого сигнала у перпендикулярного больше новой длительности красного у целевого?
                        // (уменьшили длительность красного сигнала у целевого)
                        // - уменьшим длительность зеленого сигнала у перпендикулярного
                        if (perpendicularTL.GD > e.SignalDuration)
                        {
                            perpendicularTL.SignalDurationChanged = null;
                            perpendicularTL.GD = e.SignalDuration;
                            perpendicularTL.SignalDurationChanged += SignalTiming;
                        }
                        // Длительность зеленого сигнала у перпендикулярного меньше новой длительности красного у целевого?
                        // (увеличили длительность красного сигнала у целевого)
                        else if (perpendicularTL.GD < e.SignalDuration)
                        {
                            int min = e.SignalDuration;
                            // Поиск минимальной длительности красного сигнала среди параллельных светофоров целевого
                            if (targetTL.ParallelTLsList.Count > 0)    
                                min = targetTL.ParallelTLsList.Min(parallelTL => parallelTL.RD);
                            // Минимальная длительность красного параллельных больше длительности зеленого перпендикулярного?
                            // - уменьшим длительность зеленого сигнала у перпендикулярного до этого минимального значения
                            if (min > perpendicularTL.GD)
                            {
                                perpendicularTL.SignalDurationChanged = null;
                                perpendicularTL.GD = min;
                                perpendicularTL.SignalDurationChanged += SignalTiming;
                            }
                        }
                    }
                }
            }
        }
    }

    public delegate void TrafficLightEventHandler(object sender, TrafficLightEventArgs e);
    public class TrafficLightEventArgs : EventArgs
    {
        // Свойства
        public int SignalDuration { get; set; }
        public Color SignalColor { get; set; }
        // Конструкторы
        public TrafficLightEventArgs(int signalDuration, Color signalColor)
        {
            SignalDuration = signalDuration;
            SignalColor = signalColor;
        }
    }
}
