using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;

namespace PSMoTR.Classes
{
    class Auto
    {
        // Свойства
        public float Speed { get; set; } = 16; // Скорость
        public float Length { get; private set; } = (float)4.3; // Длина
        public float Width { get; private set; } = (float)1.9; // Ширина
        public float X { get; set; } = -1; // Координата центра X
        public float Y { get; set; } = -1; // Координата центра Y
        public float Rotation { get; set; } = 0.0F; // Угол поворота
        public Way Way { get; set; } = null; // Путь
        public Route CurrentRoute { get; set; } = null; // Текущий маршрут
        public string Course
        {
            get
            {
                // Авто находится на маршруте-дуге?
                if (CurrentRoute.Type == "curveX" || CurrentRoute.Type == "curveY")
                    return CurrentRoute.Course[0];
                // Движение вверх
                if (Rotation >= -135 && Rotation < -45)
                    return "up";
                // Движение право
                else if (Rotation >= -45 && Rotation < 45 )
                    return "right";
                // Движение вниз
                else if (Rotation >= 45 && Rotation < 135)
                    return "down";
                // Движение влево
                else if (Rotation >= 135 || Rotation < -135)
                    return "left";
                else
                    return "";
            }
        } // Направление движения относительно модели
        public float CriticalDistance { get; set; } = (float)1.8; // Минимальное расстояние
        private float BrakingDistance { get; set; } = float.MaxValue; // Тормозной путь
        public PointF[] Body { get; set; } = new PointF[6]; // Поле корпуса
        public PointF[] Overlook { get; set; } = new PointF[4]; // Поле видимости
        private Auto AheadAuto { get; set; } = null; // Впереди идущее авто
        private TrafficLight AheadTL { get; set; } = null; // Светофор спереди
        private Auto OncomingAuto { get; set; } = null; // Встречное авто
        public int Number { get; set; } = -1; // Номер авто
        // Константы
        private const float screenRatio = 200; // Плотность точек
        private const float speedRatio = 6 / screenRatio; // 6 px = 1 м
        // Списки
        public static List<Auto> AutosList { get; set; } = new List<Auto>();
        public static List<Auto> GeneratedAutosList { get; set; } = new List<Auto>();
        public static List<int> CountWaveAutosList { get; set; } = new List<int>();
        /// <summary>
        /// Конструкторы
        /// </summary>
        public Auto() { }
        /// <summary>
        /// Обновление свойств класса
        /// </summary>
        private void UpdateProperties()
        {
            // Обновление BrakingDistance
            if (Speed > 0)
                BrakingDistance = Speed * (float)2.2;
            else
                BrakingDistance = (float)1.8;
            // Обновление Body
            Body = new PointF[] {
                    new PointF(X - Length / 2, Y - Width / 2), // Верхняя левая
                    new PointF(X, Y - Width / 2), // Верхняя средняя
                    new PointF(X + Length / 2, Y - Width / 2), // Верхнаяя правая
                    new PointF(X + Length / 2, Y + Width / 2), // Нижняя правая
                    new PointF(X, Y + Width / 2), // Нижняя средняя
                    new PointF(X - Length / 2, Y + Width / 2) // Нижняя левая
                };
            // Трансформируем (поворачивая каждую точку) и возвращаем тело авто
            PointF center = new PointF(X, Y); // Центр трансформирования
            float rotationRad = Rotation * (float)Math.PI / 180; // Конвертация угла поворота в радианы
            for (int i = 0; i < Body.Length; i++)
            {
                float x;
                float y;
                x = (float)((Body[i].X - center.X) * Math.Cos(rotationRad) - (Body[i].Y - center.Y) * Math.Sin(rotationRad)
                    + center.X); // Изменение координаты X
                y = (float)((Body[i].X - center.X) * Math.Sin(rotationRad) + (Body[i].Y - center.Y) * Math.Cos(rotationRad)
                    + center.Y); // Изменение координаты Y
                Body[i] = new PointF(x, y);
            }
            // Обновление Overlook
            // Курс авто направлен вправо вверх и это дуга?
            if (CurrentRoute.Course[0] == "right" && CurrentRoute.Course[1] == "up" && CurrentRoute.Type == "curveX")
            {
                Overlook = new PointF[] { new PointF(Body[2].X, Body[2].Y),
                        new PointF(CurrentRoute.End.X + CurrentRoute.Start.Speed * 2, Body[2].Y),
                        new PointF(CurrentRoute.End.X + CurrentRoute.Start.Speed * 2, Body[2].Y - Width * (float)2),
                        new PointF(Body[2].X, Body[2].Y - Width * (float)2) };
            }
            // Курс авто направлен влево вниз и это дуга?
            else if (CurrentRoute.Course[0] == "left" && CurrentRoute.Course[1] == "down" && CurrentRoute.Type == "curveX")
            {
                Overlook = new PointF[] { new PointF(Body[2].X, Body[2].Y),
                        new PointF(CurrentRoute.End.X - CurrentRoute.Start.Speed * 2, Body[2].Y),
                        new PointF(CurrentRoute.End.X - CurrentRoute.Start.Speed * 2, Body[2].Y + Width * (float)2),
                        new PointF(Body[2].X, Body[2].Y + Width * (float)2) };
            }
            // Курс авто направлен вверх налево и это дуга?
            else if (CurrentRoute.Course[0] == "up" && CurrentRoute.Course[1] == "left" && CurrentRoute.Type == "curveY")
            {
                Overlook = new PointF[] { new PointF(Body[2].X, Body[2].Y),
                        new PointF(Body[2].X, CurrentRoute.End.Y - CurrentRoute.Start.Speed * 2),
                        new PointF(Body[2].X - Width * (float)2, CurrentRoute.End.Y - CurrentRoute.Start.Speed * 2),
                        new PointF(Body[2].X - Width * (float)2, Body[2].Y) };
            }
            // Курс авто направлен вниз направо и это дуга?
            else if (CurrentRoute.Course[0] == "down" && CurrentRoute.Course[1] == "right" && CurrentRoute.Type == "curveY")
            {
                Overlook = new PointF[] { new PointF(Body[2].X, Body[2].Y),
                        new PointF(Body[2].X, CurrentRoute.End.Y + CurrentRoute.Start.Speed * 2),
                        new PointF(Body[2].X + Width * (float)2, CurrentRoute.End.Y + CurrentRoute.Start.Speed * 2),
                        new PointF(Body[2].X + Width * (float)2, Body[2].Y) };
            }
            else
            {
                Overlook = new PointF[] { new PointF(X + CriticalDistance, Y - Width / (float)1.5),
                        new PointF(X + CriticalDistance + BrakingDistance, Y - Width / (float)1.5),
                        new PointF(X + CriticalDistance + BrakingDistance, Y + Width / (float)1.5),
                        new PointF(X + CriticalDistance, Y + Width / (float)1.5) };
                // Трансформируем (поворачивая каждую точку) и возвращаем поле видимости авто
                center = new PointF(X, Y); // Центр трансформирования
                rotationRad = Rotation * (float)Math.PI / 180; // Конвертация угла поворота в радианы
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
        /// <summary>
        /// Генерация пути авто
        /// </summary>
        /// <param name="oldCountAuto"></param>
        /// <param name="newCountAuto"></param>
        private static int DefineWayNumber(int oldCountAuto, int newCountAuto, List<Auto> autosList)
        {
            Random rnd = new Random();
            bool stop = false;
            int wayNumber;
            do
            {
                wayNumber = rnd.Next(0, Way.WaysList.Count); // Случайный выбор пути
                for (int i = oldCountAuto; i < newCountAuto; i++)
                {
                    if (autosList[i].Way == null)
                        if (i == newCountAuto - 1)
                        {
                            stop = true;
                            break;
                        }
                        else
                            continue;
                    else if (autosList[i].Way.StartRoute == Way.WaysList[wayNumber].StartRoute)
                    {
                        stop = false;
                        break;
                    }
                    else
                        stop = true;
                }
            } while (!stop);
            return wayNumber;
        }
        /// <summary>
        /// Генерация авто
        /// </summary>
        public static void GenerateAutos()
        {
            Random rnd = new Random();
            int oldCountAuto = GeneratedAutosList.Count; // Начало списка, куда добавляем авто текуще волны 
            int count = rnd.Next(1, Way.StartRoutesCount); // Определяем сколько авто создастся в текущей волне
            CountWaveAutosList.Add(count);
            int newCountAuto = oldCountAuto + count; // Определяем конец списка с учетом новых авто
            for (int i = oldCountAuto; i < newCountAuto; i++)
            {
                // Генерация авто
                GeneratedAutosList.Add(new Auto());
                int wayNumber = DefineWayNumber(oldCountAuto, i + 1, GeneratedAutosList); // Генерируем номер пути для авто
                GeneratedAutosList[i].Way = Way.WaysList[wayNumber];
                GeneratedAutosList[i].CurrentRoute = GeneratedAutosList[i].Way.StartRoute;
                GeneratedAutosList[i].X = GeneratedAutosList[i].CurrentRoute.Start.X;
                GeneratedAutosList[i].Y = GeneratedAutosList[i].CurrentRoute.Start.Y;
                GeneratedAutosList[i].Number = i;
            }
        }
        /// <summary>
        /// Использование сгенерированных авто моделью
        /// </summary>
        /// <param name="modelAutosList"></param>
        /// <param name="wavesCounter"></param>
        public static void UseGeneratedAutos(List<Auto> modelAutosList, int wavesCounter)
        {
            if (CountWaveAutosList.Count <= wavesCounter)
                return;
            int start = modelAutosList.Count;
            int end = start + CountWaveAutosList[wavesCounter];
            for (int i = start; i < end; i++)
            {
                // Генерация авто для 1 модели
                modelAutosList.Add(new Auto());
                modelAutosList[i].Way = GeneratedAutosList[i].Way;
                modelAutosList[i].CurrentRoute = GeneratedAutosList[i].CurrentRoute;
                modelAutosList[i].X = GeneratedAutosList[i].X;
                modelAutosList[i].Y = GeneratedAutosList[i].Y;
                modelAutosList[i].Number = GeneratedAutosList[i].Number;
            }
        }
        /// <summary>
        /// Смена маршрута
        /// </summary>
        private void GetNextRoute(List<Auto> autosList)
        {
            int index = Way.RoutesList.IndexOf(CurrentRoute); // Определяем маршрут в списке маршрутов у авто
            // Следующий маршрут не последний?
            if (index + 1 < Way.RoutesList.Count)
                CurrentRoute = Way.RoutesList[index + 1]; // Назначаем следующий в пути маршрут текущим маршрутом
            else
                autosList[autosList.IndexOf(this)] = null;
        }
        /// <summary>
        /// Вычислениде дистанции
        /// </summary>
        /// <param name="targetAuto"></param>
        /// <returns></returns>
        private float Distance(Auto targetAuto)
        {
            float distance = float.MaxValue;
            // Находим наикротчайшее расстояние между точками тела двух авто
            for (int i = 0; i < Body.Length; i++)
                for (int j = 0; j < targetAuto.Body.Length; j++)
                {
                    float d = (float)Math.Pow(Math.Pow(targetAuto.Body[j].X - Body[i].X, 2) + Math.Pow(targetAuto.Body[j].Y - Body[i].Y, 2), 0.5);
                    if (d < distance)
                        distance = d;
                }
            return distance;
        }
        /// <summary>
        /// Определение является ли помехой справа
        /// </summary>
        /// <param name="targetAuto"></param>
        /// <returns></returns>
        private bool IsRight(Auto targetAuto)
        {
            if (Course == "right" && targetAuto.Course == "up")
                return true;
            else if (Course == "up" && targetAuto.Course == "left")
                return true;
            else if (Course == "down" && targetAuto.Course == "right")
                return true;
            else if (Course == "left" && targetAuto.Course == "down")
                return true;
            return false;
        }
        /// <summary>
        /// Торможение
        /// </summary>
        /// <param name="from"></param>
        /// <param name="aheadAuto"></param>
        private void Braking(Auto targetAuto)
        {
            float distance = Distance(targetAuto);
            // Дистанция меньше тормозного пути?
            if (distance <= BrakingDistance)
            {
                // Скорость превышает допустимую?
                if (Speed >= targetAuto.Speed && Speed >= 0.5)
                    Speed -= (float)0.5;
                // Авто "слиплись"?
                if (distance < CriticalDistance / 2)
                {
                    targetAuto.Speed += (float)0.5;
                    Speed -= (float)0.5;
                }
                    
                // Расстояние между авто достигло расстояния, равному дистанции?
                else if (distance <= CriticalDistance)
                    Speed = 0;
            }
            else
            {
                // Скорость ниже допустимой?
                if (Speed < CurrentRoute.End.Speed)
                    Speed += (float)0.5;
            }
        }
        private void Braking(TrafficLight targetTL)
        {
            float distance = (float)Math.Pow(Math.Pow(targetTL.Position.X - X, 2) + Math.Pow(targetTL.Position.Y - Y, 2), 0.5);
            // Дистанция меньше тормозного пути?
            if (distance <= BrakingDistance * 2)
            {
                // Расстояние между авто и точкой достигло расстояния, равному дистанции?
                if (distance <= CriticalDistance * 2)
                    Speed = targetTL.Position.Speed;
                // Скорость превышает допустимую?
                if (Speed >= targetTL.Position.Speed && Speed >= 0.5)
                    Speed -= (float)0.5;
                // Скорость ниже допустимой?
                else if (Speed < targetTL.Position.Speed && Speed >= 0.5)
                    Speed += (float)0.5;
            }
            else
            {
                // Скорость ниже допустимой?
                if (Speed < targetTL.Position.Speed)
                    Speed += (float)0.5;
            }
        }
        private void Braking(Point targetPoint)
        {
            float distance = (float)Math.Pow(Math.Pow(targetPoint.X - X, 2) + Math.Pow(targetPoint.Y - Y, 2), 0.5);
            // Дистанция меньше тормозного пути?
            if (distance <= BrakingDistance * 2)
            {
                // Расстояние между авто и точкой достигло расстояния, равному дистанции?
                if (distance <= CriticalDistance * 2)
                    Speed = targetPoint.Speed;
                // Скорость превышает допустимую?
                if (Speed >= targetPoint.Speed && Speed >= 0.5)
                    Speed -= (float)0.5;
                // Скорость ниже допустимой?
                else if (Speed < targetPoint.Speed && Speed >= 0.5)
                    Speed += (float)0.5;
            }
            else
            {
                // Скорость ниже допустимой?
                if (Speed < targetPoint.Speed)
                    Speed += (float)0.5;
            }
        }
        private void Braking(bool stop)
        {
            if (stop)
                Speed = 0;
            else
            {
                // Скорость ниже допустимой?
                if (Speed < CurrentRoute.End.Speed)
                    Speed += (float)0.5;
            }  
        }
        /// <summary>
        /// Попадание авто в поле видимости
        /// </summary>
        /// <returns></returns>
        private bool IsInSight(Auto targetAuto)
        {
            using (GraphicsPath gp_overlook = new GraphicsPath())
            {
                gp_overlook.AddPolygon(Overlook);
                foreach (PointF p in targetAuto.Body)
                    if (gp_overlook.IsVisible(p.X, p.Y))
                        return true;
                return false;
            }
        }
        private bool IsInSight(TrafficLight targetTL)
        {
            using (GraphicsPath gp_overlook = new GraphicsPath())
            {
                gp_overlook.AddPolygon(Overlook);
                // Светофор попадает в поле зрения и один из маршрутов авто совпадает с теми, что контролирует светофор?
                if (gp_overlook.IsVisible(targetTL.Position.X, targetTL.Position.Y) && Way.RoutesList.Any(r => targetTL.NextRoutes.Contains(r)))
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Определение является ли авто встречным
        /// </summary>
        /// <param name="targetAuto"></param>
        /// <returns></returns>
        private bool IsOncoming(Auto targetAuto)
        {
            // Движение текущего авто вверх (относительно модели), а целевое авто движется в другом направлении?
            if (Course == "up" && targetAuto.Course != "up")
                return true;
            // Движение текущего авто вправо (относительно модели), а целевое авто движется в другом направлении?
            else if (Course == "right" && targetAuto.Course != "right")
                return true;
            // Движение текущего авто вниз (относительно модели), а целевое авто движется в другом направлении?
            else if (Course == "down" && targetAuto.Course != "down")
                return true;
            // Движение текущего авто влево (относительно модели), а целевое авто движется в другом направлении?
            else if (Course == "left" && targetAuto.Course != "left")
                return true;
            return false;
        }
        /// <summary>
        /// Поиск помех для авто
        /// </summary>
        /// <param name="autosList"></param>
        private void FindObstacles(List<Auto> autosList, List<TrafficLight> TLsList)
        {
            // Инициализация расстояний
            float aheadAuto_distance = float.MaxValue;
            float aheadTL_distance = float.MaxValue;
            float oncomingAuto_distance = float.MaxValue;
            // Поиск авто в поле зрения
            List<Auto> overlookAutos = autosList.FindAll(a => a != this && a != null && IsInSight(a)); // Все авто в поле зрения
            // Есть авто в поле зрения?
            if (overlookAutos.Count != 0)
            {
                foreach (Auto a in overlookAutos)
                {
                    float a_distance = Distance(a);
                    // Это встречное авто и в данный момент пересекается его путь?
                    if (IsOncoming(a) && a.Way.IsCrossingRoute(CurrentRoute))
                    {
                        // У авто минимальное расстояние с текущим авто?
                        if (a_distance <= oncomingAuto_distance)
                        {
                            OncomingAuto = a;
                            oncomingAuto_distance = a_distance;
                        }
                    }
                    // Иначе это впререди идущее авто
                    else
                    {
                        // Вперед идущее авто находится не на пути и не мешает сейчас?
                        if (!Way.RoutesList.Contains(a.CurrentRoute) &&
                            !(CurrentRoute.End.Equals(a.CurrentRoute.Start) || CurrentRoute.End.Equals(a.CurrentRoute.End)))
                            continue;
                        // У авто минимальное расстояние с текущим авто?
                        if (a_distance <= aheadAuto_distance)
                        {
                            AheadAuto = a;
                            aheadAuto_distance = a_distance;
                        }
                    }
                }
            }
            // Поиск светофоров в поле зрения
            List<TrafficLight> overlookTLs = TLsList.FindAll(tl => tl != null && IsInSight(tl) &&
                (tl.CurrentSC == Color.Red || tl.CurrentSC == Color.Orange)); // Все красные/оранжевые светофоры в поле зрения
            // Есть светофоры в поле зрения?
            if (overlookTLs.Count != 0)
            {
                foreach (TrafficLight oTL in overlookTLs)
                {
                    // Расстояние до светофора от текущего авто
                    float oTL_distance = Point.Distance(new Point(X, Y), new Point(oTL.Position.X, oTL.Position.Y));
                    // У светофора минимальное расстояние с текущим авто?
                    if (oTL_distance <= aheadTL_distance)
                    {
                        AheadTL = oTL;
                        aheadTL_distance = oTL_distance;
                    }
                }
            }
            // Выясняем, кто среди авто и светофоров ближе
            // Впереди идущее авто ближе?
            if (aheadAuto_distance < oncomingAuto_distance && aheadAuto_distance < aheadTL_distance)
            {
                OncomingAuto = null;
                AheadTL = null;
            }
            // Светофор ближе?
            else if (aheadTL_distance < aheadAuto_distance && aheadTL_distance < oncomingAuto_distance)
            {
                AheadAuto = null;
                OncomingAuto = null;
            }
            // Встречное авто ближе?
            else if (oncomingAuto_distance < aheadAuto_distance && oncomingAuto_distance < aheadTL_distance)
            {
                AheadAuto = null;
                AheadTL = null;
            }
            else
            {
                AheadAuto = null;
                AheadTL = null;
                OncomingAuto = null;
            }
            // Очистка списков
            overlookAutos.Clear();
            overlookTLs.Clear();
        }
        /// <summary>
        /// Контроль движения
        /// </summary>
        private void MovingControl(Auto targetAuto)
        {
            // Помеха - впереди идущее авто?
            if (targetAuto == AheadAuto)
                Braking(targetAuto);
            // Помеха - встречное авто?
            else if (targetAuto == OncomingAuto)
            {
                // Помеха напротив?
                if (Course == "up" && targetAuto.Course == "down" ||
                        Course == "right" && targetAuto.Course == "left" ||
                        Course == "down" && targetAuto.Course == "up" ||
                        Course == "left" && targetAuto.Course == "right")
                {
                    // Помеха едет медленно?
                    if (targetAuto.Speed > 1)
                        Braking(true);
                    else
                        Braking(false);
                }
                else
                {
                    // Помеха не видит текущее авто?
                    if (targetAuto.OncomingAuto != this)
                        Braking(targetAuto);
                    // Авто и помеха видят друг друга и помеха справа?
                    else if (targetAuto.OncomingAuto == this && IsRight(targetAuto))
                        Braking(targetAuto);
                    else
                        Braking(false);
                }
            }
        }
        private void MovingControl(TrafficLight targetTL)
        {
            // Действия
            if (targetTL.CurrentSC == Color.Red)
                Braking(targetTL);
            else if (targetTL.CurrentSC == Color.Orange)
                Braking(targetTL);
            else
                Braking(CurrentRoute.End); // Торможение перед переходом на другой маршрут
        }
        private void MovingControl(Point targetPoint)
        {
            Braking(targetPoint);
        }
        /// <summary>
        /// Проверка на существование текущих помех
        /// </summary>
        /// <returns></returns>
        private bool CheckOnCurrentObstacles(List<Auto> autosList)
        {
            // Есть помеха - впереди идущее авто? И оно все еще на модели?
            if (AheadAuto != null && autosList.Contains(AheadAuto))
            {
                // Помеха в области видимости?
                if (IsInSight(AheadAuto))
                {
                    MovingControl(AheadAuto);
                    return true;
                }
                else return false;
            }
            // Есть помеха - встречное авто? И оно все еще на модели?
            else if (OncomingAuto != null && autosList.Contains(OncomingAuto))
            {
                // Помеха в области видимости?
                if (IsInSight(OncomingAuto))
                {
                    if (!IsOncoming(OncomingAuto))
                    {
                        AheadAuto = OncomingAuto;
                        OncomingAuto = null;
                        MovingControl(AheadAuto);
                    }
                    else
                        MovingControl(OncomingAuto);
                    return true;
                }
                else return false;
            }
            // Есть помеха - светофор впереди?
            else if (AheadTL != null)
            {
                // Помеха в области видимости?
                if (IsInSight(AheadTL))
                {
                    MovingControl(AheadTL);
                    return true;
                }
                else return false;
            }
            else
            {
                MovingControl(CurrentRoute.End);
                return false;
            }
        }
        /// <summary>
        /// Проверка на завершение текущего маршрута
        /// </summary>
        private void CheckOnCurrentRouteEnd(List<Auto> autosList)
        {
            // Текущий маршрут направлен вправо?
            if (CurrentRoute.End.X > CurrentRoute.Start.X)
            {
                // Достигнут конец маршрута?
                if (X >= CurrentRoute.End.X)
                    GetNextRoute(autosList);
            }
            // Текущий маршрут направлен влево?
            else if (CurrentRoute.End.X < CurrentRoute.Start.X)
            {
                // Достигнут конец маршрута?
                if (X <= CurrentRoute.End.X)
                    GetNextRoute(autosList);
            }
            else
            {
                // Текущий маршрут направлен вверх?
                if (CurrentRoute.End.Y > CurrentRoute.Start.Y)
                {
                    // Достигнут конец маршрута?
                    if (Y >= CurrentRoute.End.Y)
                        GetNextRoute(autosList);
                }
                // Текущий маршрут направлен вниз?
                else if (CurrentRoute.End.Y < CurrentRoute.Start.Y)
                {
                    // Достигнут конец маршрута?
                    if (Y <= CurrentRoute.End.Y)
                        GetNextRoute(autosList);
                }
            }
        }
        /// <summary>
        /// Движение
        /// </summary>
        /// <param name="type"></param>
        /// <param name="course"></param>
        /// <param name="zoom"></param>
        public void Move(List<Auto> autosList, List<TrafficLight> trafficLightsList, bool findObstacles)
        {
            UpdateProperties(); // Обновляем свойства авто
            CheckOnCurrentRouteEnd(autosList); // Проверка на завершение текущего маршрута
            // Необходимо проверить на помехи и текущих помех нет?
            if (findObstacles && !CheckOnCurrentObstacles(autosList))
                FindObstacles(autosList, trafficLightsList);
            // Тип движения прямо?
            if (CurrentRoute.Type == "straight")
                MoveStraight();
            // Тип движения по дуге?
            else if (CurrentRoute.Type == "curveX" || CurrentRoute.Type == "curveY")
                MoveCurve();
        }
        /// <summary>
        /// Движение прямо
        /// </summary>
        /// <param name="course"></param>
        /// <param name="zoom"></param>
        private void MoveStraight()
        {
            Rotation = (float)(Math.Atan2(CurrentRoute.End.Y - Y, CurrentRoute.End.X - X) * 180 / Math.PI);
            float cos = (float)Math.Cos(Rotation * Math.PI / 180);
            float sin = (float)Math.Sin(Rotation * Math.PI / 180);
            X += Speed * speedRatio * cos;
            Y += Speed * speedRatio * sin;
        }
        /// <summary>
        /// Движение по дуге
        /// </summary>
        private void MoveCurve()
        {
            float center_X = 0; // Центр окружности X
            float center_Y = 0; // Центр окружности Y
            float r; // Радиус дуги
            float radSpeed; // Скорость изменения угла поворота
            float globalRotation = 0.0F; // Угол поворота относительно модели
            // Вычисляем угол поворота относительно градусной окружности
            if (CurrentRoute.Course[0] == "up" && CurrentRoute.Course[1] == "right")
                globalRotation = -Rotation + 90F; // от -90 до 0 --> от 180 до 90
            else if (CurrentRoute.Course[0] == "right" && CurrentRoute.Course[1] == "up")
                globalRotation = -90F - Rotation; // от 0 до -90 --> от -90 до 0
            else if (CurrentRoute.Course[0] == "up" && CurrentRoute.Course[1] == "left")
                globalRotation = -Rotation - 90F; // от -90 до -180 --> от 0 до 90
            else if (CurrentRoute.Course[0] == "left" && CurrentRoute.Course[1] == "up")
                globalRotation = -Rotation - 270F; // от -180 до -90 --> от -90 до -180
            else if (CurrentRoute.Course[0] == "down" && CurrentRoute.Course[1] == "right")
                globalRotation = -Rotation - 90F; // от 90 до 0 --> от -180 до -90
            else if (CurrentRoute.Course[0] == "right" && CurrentRoute.Course[1] == "down")
                globalRotation = 90F - Rotation; // от 0 до 90 --> от 90 до 0
            else if (CurrentRoute.Course[0] == "down" && CurrentRoute.Course[1] == "left")
                globalRotation = -Rotation + 90F; // от 90 до 180 --> от 0 до -90
            else if (CurrentRoute.Course[0] == "left" && CurrentRoute.Course[1] == "down")
                globalRotation = -Rotation + 270F; // от 180 до 90 --> от 90 до 180
            // Вычисление cos и sin относительно градусной окружности
            float cos = (float)Math.Cos(globalRotation * Math.PI / 180);
            float sin = (float)Math.Sin(globalRotation * Math.PI / 180);
            // Движение по дуге curveX?
            if (CurrentRoute.Type == "curveX")
            {
                center_X = CurrentRoute.Start.X;
                center_Y = CurrentRoute.End.Y;
            }
            // Движение по дуге curveY?
            else if (CurrentRoute.Type == "curveY")
            {
                center_X = CurrentRoute.End.X;
                center_Y = CurrentRoute.Start.Y;
            }
            // Вычисляем радиус окружности поворота и радиальную скорость поворота
            r = Math.Abs(CurrentRoute.Start.Y - CurrentRoute.End.Y);
            radSpeed = Speed * speedRatio / r;
            // Определение новых координат
            // Дистанция до конца маршрута меньше 0.01?
            if (Math.Abs(X - CurrentRoute.End.X) < 0.01)
            {
                X = CurrentRoute.End.X;
                Y = CurrentRoute.End.Y;
            }
            else
            {
                // Координата точки на окружности
                X = center_X + r * cos;
                Y = center_Y - r * sin;
            }
            // Изменение градуса поворота авто
            float d_angle = radSpeed * 180 / (float)Math.PI;
            if (CurrentRoute.Course[0] == "up" && CurrentRoute.Course[1] == "right")
                Rotation += d_angle;
            else if (CurrentRoute.Course[0] == "right" && CurrentRoute.Course[1] == "up")
                Rotation -= d_angle;
            else if (CurrentRoute.Course[0] == "up" && CurrentRoute.Course[1] == "left")
                Rotation -= d_angle;
            else if (CurrentRoute.Course[0] == "left" && CurrentRoute.Course[1] == "up")
                Rotation += d_angle;
            else if (CurrentRoute.Course[0] == "down" && CurrentRoute.Course[1] == "right")
                Rotation -= d_angle;
            else if (CurrentRoute.Course[0] == "right" && CurrentRoute.Course[1] == "down")
                Rotation += d_angle;
            else if (CurrentRoute.Course[0] == "down" && CurrentRoute.Course[1] == "left")
                Rotation += d_angle;
            else if (CurrentRoute.Course[0] == "left" && CurrentRoute.Course[1] == "down")
                Rotation -= d_angle;
        }
    }
}