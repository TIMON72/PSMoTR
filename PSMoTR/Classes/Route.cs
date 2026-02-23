using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PSMoTR.Classes
{
    public class Route
    {
        // Перечисления
        public enum Types
        {
            straight,
            curveX,
            curveY
        }
        // Свойства
        public Point Start { get; set; } // Начальная точка
        public string Type { get; set; } // Тип
        public Point End { get; set; } // Конечная точка
        public float Rotation
        {
            get
            {
                float rotation;
                rotation = (float)(Math.Atan2(End.Y - Start.Y, End.X - Start.X) * 180 / Math.PI);
                return rotation;
            }
        } // Поворот маршрута в градусах
        public string[] Course
        {
            get
            {
                string[] course = new string[2];
                // Тип маршрута - прямая?
                if (Type == "straight")
                {
                    // Движение вверх
                    if (Rotation >= -135 && Rotation < -45)
                    {
                        course[0] = "up";
                        course[1] = "up";
                    }
                    // Движение право
                    else if (Rotation >= -45 && Rotation < 45)
                    {
                        course[0] = "right";
                        course[1] = "right";
                    }
                    // Движение вниз
                    else if (Rotation >= 45 && Rotation < 135)
                    {
                        course[0] = "down";
                        course[1] = "down";
                    }
                    // Движение влево
                    else if (Rotation >= 135 || Rotation < -135)
                    {
                        course[0] = "left";
                        course[1] = "left";
                    }
                }
                // Тип маршрута - дуга X?
                else if (Type == "curveX")
                {
                    if (Start.X < End.X && Start.Y > End.Y)
                    {
                        course[0] = "right";
                        course[1] = "up";
                    }
                    else if (Start.X < End.X && Start.Y < End.Y)
                    {
                        course[0] = "right";
                        course[1] = "down";
                    }
                    else if (Start.X > End.X && Start.Y > End.Y)
                    {
                        course[0] = "left";
                        course[1] = "up";
                    }
                    else if (Start.X > End.X && Start.Y < End.Y)
                    {
                        course[0] = "left";
                        course[1] = "down";
                    }
                }
                // Тип маршрута - дуга Y?
                else if (Type == "curveY")
                {
                    if (Start.X < End.X && Start.Y > End.Y)
                    {
                        course[0] = "up";
                        course[1] = "right";
                    }
                    else if (Start.X > End.X && Start.Y > End.Y)
                    {
                        course[0] = "up";
                        course[1] = "left";
                    }
                    else if (Start.X < End.X && Start.Y < End.Y)
                    {
                        course[0] = "down";
                        course[1] = "right";
                    }
                    else if (Start.X > End.X && Start.Y < End.Y)
                    {
                        course[0] = "down";
                        course[1] = "left";
                    }
                }
                return course;
            }
        } // Направление маршрута
        public PointF[] Body
        {
            get
            {
                PointF[] body = null;
                // Маршурт - прямая?
                if (Type == "straight")
                {
                    float length = GetLength();
                    body = new PointF[]
                    {
                        new PointF(Start.X, Start.Y - 1),
                        new PointF(Start.X + length, Start.Y - 1),
                        new PointF(Start.X + length, Start.Y + 1),
                        new PointF(Start.X, Start.Y + 1)
                    };
                    PointF center = new PointF(Start.X, Start.Y); // Центр трансформирования
                    float rotationRad = Rotation * (float)Math.PI / 180; // Конвертация угла поворота в радианы
                    for (int i = 0; i < body.Length; i++)
                    {
                        float x;
                        float y;
                        x = (float)((body[i].X - center.X) * Math.Cos(rotationRad) - (body[i].Y - center.Y) * Math.Sin(rotationRad)
                            + center.X); // Изменение координаты X
                        y = (float)((body[i].X - center.X) * Math.Sin(rotationRad) + (body[i].Y - center.Y) * Math.Cos(rotationRad)
                            + center.Y); // Изменение координаты Y
                        body[i] = new PointF(x, y);
                    }
                }
                // Маршрут - дуга?
                else if (Type == "curveX" || Type == "curveY")
                {
                    float x = Start.X; // Начальная точка X вершины прямоугольника (выбрана начальная точка маршрута)
                    float y = Start.Y; // Начальная точка Y вершины прямоугольника (выбрана начальная точка маршрута)
                    float xs = End.X; // Смещение прямоугольника по X (выбрана конечная точка маршрута)
                    float ys = End.Y; // Смещение прямоугольника по Y (выбрана конечная точка маршрута)
                    float dx = Math.Abs((xs - x) * 2); // Точка, равная двойной длине прямоугольника по X
                    if (dx < 1)
                        dx = 1;
                    float dy = Math.Abs((ys - y) * 2); // Точка, равная двойной длине прямоугольника по Y
                    if (dy < 1)
                        dy = 1;
                    float startAngle = 0; // Угол от начальной точки
                    float sweepAngle = 0; // Угол от конечной точки
                    // Маршрут - дуга X?
                    if (Type == "curveX")
                    {

                        if (Course[0] == "right" && Course[1] == "down")
                        {
                            startAngle = 270F;
                            sweepAngle = 90F;
                            x = xs - dx;
                        }
                        else if (Course[0] == "right" && Course[1] == "up")
                        {
                            startAngle = 0F;
                            sweepAngle = 90F;
                            x = xs - dx;
                            y -= dy;
                        }
                        else if (Course[0] == "left" && Course[1] == "down")
                        {
                            startAngle = 180F;
                            sweepAngle = 90F;
                            x = xs;
                        }
                        else if (Course[0] == "left" && Course[1] == "up")
                        {
                            startAngle = 90F;
                            sweepAngle = 90F;
                            x = xs;
                            y -= dy;
                        }
                    }
                    // Маршрут - дуга Y?
                    else if (Type == "curveY")
                    {
                        if (Course[0] == "down" && Course[1] == "right")
                        {
                            startAngle = 90F;
                            sweepAngle = 90F;
                            y = ys - dy;
                        }
                        else if (Course[0] == "up" && Course[1] == "right")
                        {
                            startAngle = 180F;
                            sweepAngle = 90F;
                            y = ys;
                        }
                        else if (Course[0] == "down" && Course[1] == "left")
                        {
                            startAngle = 0F;
                            sweepAngle = 90F;
                            x -= dx;
                            y = ys - dy;
                        }
                        else if (Course[0] == "up" && Course[1] == "left")
                        {
                            startAngle = 270F;
                            sweepAngle = 90F;
                            x -= dx;
                            y = ys;
                        }
                    }
                    body = new PointF[]
                    {
                        new PointF(x, y),
                        new PointF(dx, dy),
                        new PointF(startAngle, sweepAngle)
                    };
                }
                return body;
            }
        } // Тело (прямоугольник), в которое вписана линия, соединяющая точки
        public Color Color { get; set; } = Color.Orange; // Цвет отображения маршрута
        public int Number
        {
            get
            {
                return RoutesList.IndexOf(this);
            }
        } // Номер маршрута
        // Функции
        public static Func<Point, Point, float, float> StraightEquationX = (p1, p2, x) =>
            (float)Math.Round((x - p1.X) * (p2.Y - p1.Y) / (p2.X - p1.X) + p1.Y, 1); // Функция прямой по двум точкам
                                                                                     // каноническое уравнение прямой
                                                                                     // на выходе получаем значение Y
        public static Func<Point, Point, float, float> StraightEquationY = (p1, p2, y) =>
            (float)Math.Round((y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X, 1); // Функция прямой по двум точкам
                                                                                     // каноническое уравнение прямой
                                                                                     // на выходе получаем значение X
        // Списки
        public static List<Route> RoutesList { get; private set; } = new List<Route>(); // Список маршрутов
        // Конструкторы
        public Route(Point start, string type, Point end)
        {
            Start = start;
            Type = type;
            End = end;
        }
        public Route(Point start, string type, Point end, Color color)
        {
            Start = start;
            Type = type;
            End = end;
            Color = color;
        }
        public Route(string routeStr)
        {
            if (string.IsNullOrWhiteSpace(routeStr))
            {
                throw new ArgumentException("message", nameof(routeStr));
            }
            string[] str = routeStr.Split(' ');
            Start = new Point(x: Convert.ToSingle(str[0], null), y: Convert.ToSingle(str[1], null), speed: Convert.ToSingle(str[2], null));
            End = new Point(x: Convert.ToSingle(str[4], null), y: Convert.ToSingle(str[5], null), speed: Convert.ToSingle(str[6], null));
            Type = str[3];
        }
        /// <summary>
        /// Сравнение двух объектов на равенство (перегрузка)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Route r = obj as Route;
            if (r as Route == null)
                return false;
            return r.Start == Start && r.End == End && r.Type == Type;
        }
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public Route Clone()
        {
            Route clone = new Route(null, "", null)
            {
                Start = Start.Clone(),
                Type = Type,
                End = End.Clone(),
                Color = Color.Yellow
            };
            return clone;
        }
        /// <summary>
        /// Добавление нового маршрута в список с соблюдением целостности
        /// </summary>
        /// <param name="route_new"></param>
        /// <returns></returns>
        public static bool List_SafelyAdd(Route route_new)
        {
            Route similarRoute = null; // аналогичный маршрут
            Route relatedRoute = null; // связанный точкой(-ами) маршрут
            Point similarPoint = null; // аналогичная точка
            // Поиск аналогичного маршрута
            similarRoute = RoutesList.Find(r => r != route_new && 
                ((route_new.Start.Equals(r.Start) && route_new.End.Equals(r.End)) ||
                (route_new.Start.Equals(r.End) && route_new.End.Equals(r.Start))));
            if (similarRoute != null)
                return false;
            // Поиск связанного маршрута, где начальная точка является для добавляемого начальной
            relatedRoute = RoutesList.Find(r => r != route_new && route_new.Start.Equals(r.Start));
            if (relatedRoute != null)
                route_new.Start = relatedRoute.Start;
            // Поиск связанного маршрута, где конечная точка является для добавляемого начальной
            relatedRoute = RoutesList.Find(r => r != route_new && route_new.Start.Equals(r.End));
            if (relatedRoute != null)
                route_new.Start = relatedRoute.End;
            // Поиск связанного маршрута, где начальная точка является для добавляемого конечной
            relatedRoute = RoutesList.Find(r => r != route_new && route_new.End.Equals(r.Start));
            if (relatedRoute != null)
                route_new.End = relatedRoute.Start;
            // Поиск связанного маршрута, где конечная точка является для добавляемого конечной
            relatedRoute = RoutesList.Find(r => r != route_new && route_new.End.Equals(r.End));
            if (relatedRoute != null)
                route_new.End = relatedRoute.End;
            // Поиск в списке точки аналогичной начальной добавляемого маршрута в списке
            similarPoint = Point.PointsList.Find(p => p.Equals(route_new.Start));
            if (similarPoint is null)
                Point.PointsList.Add(route_new.Start);
            // Поиск в списке точки аналогичной конечной добавляемого маршрута в списке
            similarPoint = Point.PointsList.Find(p => p.Equals(route_new.End));
            if (similarPoint is null)
                Point.PointsList.Add(route_new.End);
            RoutesList.Add(route_new);
            _ = route_new.Course;
            return true;
        }
        /// <summary>
        /// Удаление маршрута из списка с соблюдением целостности
        /// </summary>
        /// <param name="route_rem"></param>
        /// <returns></returns>
        public static bool List_SafelyRemove(Route route_rem)
        {
            Route relatedRoute = null;
            // Проверка на отсутствие маршрутов, связанных с начальной точкой удаляемого
            relatedRoute = RoutesList.Find(r => r != route_rem && (route_rem.Start.Equals(r.Start) || route_rem.Start.Equals(r.End)));
            if (relatedRoute is null)
                Point.PointsList.Remove(route_rem.Start);
            // Проверка на отсутствие маршрутов, связанных с конечной точкой удаляемого
            relatedRoute = RoutesList.Find(r => r != route_rem && (route_rem.End.Equals(r.Start) || route_rem.End.Equals(r.End)));
            if (relatedRoute is null)
                Point.PointsList.Remove(route_rem.End);
            // Удаление маршрута из списка
            RoutesList.Remove(route_rem);
            return true;
        }
        /// <summary>
        /// Присвоить значение полю RoutesList
        /// </summary>
        /// <param name="routesList"></param>
        public static void SetRoutesList(List<Route> routesList)
        {
            RoutesList.AddRange(routesList);
        }
        /// <summary>
        /// Сортировка
        /// </summary>
        public static void Sort()
        {
            RoutesList = RoutesList.OrderBy(r => r.Start.X).ThenBy(r => r.Start.Y).ToList(); // Сортировка
        }
        /// <summary>
        /// Открытие файла
        /// </summary>
        public static bool OpenFile(string path)
        {
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                RoutesList.Clear();
                string[] str;
                Route route;
                Point startPoint;
                Point endPoint;
                try
                {
                    if (file.Length == 0) // Если файл пустой, то загружаем в него содержимое из шаблона в ресурсах
                    {
                        string[] reader2 = Properties.Resources.routes.Replace("\r", "").Split('\n'); // переим!!!!!!!!!
                        for (int i = 0; i < reader2.Length; i++)
                        {
                            str = reader2[i].Split(' ');
                            startPoint = new Point(Convert.ToSingle(str[0]), Convert.ToSingle(str[1]), Convert.ToSingle(str[2]));
                            endPoint = new Point(Convert.ToSingle(str[4]), Convert.ToSingle(str[5]), Convert.ToSingle(str[6]));
                            route = new Route(startPoint, str[3], endPoint);
                            List_SafelyAdd(route);
                        }
                    }
                    else // Иначе считываем файл
                    {
                        StreamReader reader = new StreamReader(file);
                        while (!reader.EndOfStream)
                        {
                            str = reader.ReadLine().Split(' ');
                            startPoint = new Point(Convert.ToSingle(str[0]), Convert.ToSingle(str[1]), Convert.ToSingle(str[2]));
                            endPoint = new Point(Convert.ToSingle(str[4]), Convert.ToSingle(str[5]), Convert.ToSingle(str[6]));
                            route = new Route(startPoint, str[3], endPoint);
                            List_SafelyAdd(route);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Перезапись файла
        /// </summary>
        public static void RewriteFile(string path)
        {
            Sort(); // Выполняем сортировку
            // Открытие диалогового окна для выбора перезаписываемого файла
            using (StreamWriter writer = new StreamWriter(path, false)) // false - перезапись
            {
                try
                {
                    foreach (Route r in RoutesList)
                        writer.WriteLine(r.Start.X.ToString() + " " + r.Start.Y.ToString() + " " + r.Start.Speed + " " + r.Type
                            + " " + r.End.X.ToString() + " " + r.End.Y.ToString() + " " + r.End.Speed);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        /// <summary>
        /// Получить длину маршрута
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public float GetLength()
        {
            return Point.Distance(Start, End);
        }
        /// <summary>
        /// Изменение значения исходной точки на целевое у всех маршрутов с соблюдением связности
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        public static void EditingSimilarPoints(Point original, Point target)
        {
            foreach (Route route in RoutesList)
            {
                if (route.Start == original)
                {
                    route.Start = target;
                    route.Start.Speed = target.Speed;
                }
                if (route.End == original)
                {
                    route.End = target;
                    route.End.Speed = target.Speed;
                }
            }
        }
        /// <summary>
        /// Проверка на принадлежность точки маршруту
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsContainPoint(Point point)
        {
            // Принадлежность точки прямой, заданной двумя точками
            float result = (point.X - Start.X) * (End.Y - Start.Y) -
                (point.Y - Start.Y) * (End.X - Start.X);
            // Проверка результата и вхождения в диапазон
            if ((Math.Abs(result) < 2) && ((Start.X <= point.X && point.X <= End.X) || (End.X <= point.X && point.X <= Start.X)) &&
            ((Start.Y <= point.Y && point.Y <= End.Y) || (End.Y <= point.Y && point.Y <= Start.Y)))
                return true;
            return false;
        }
        /// <summary>
        /// Поиск маршрута по точкам
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Route FindByPoints(Point start, Point end)
        {
            Route result = RoutesList.Find(r => r.Start.Equals(start) && r.End.Equals(end));
            return result;
        }
        /// <summary>
        /// Поиск пересекающих маршрутов для заданного
        /// </summary>
        /// <param name="targetRoute"></param>
        /// <returns></returns>
        public static List<Route> FindCrossingRoutes(Route targetRoute)
        {
            List<Route> resultsList = new List<Route>();
            Route pendingRoute;
            for (int i = 0; i < RoutesList.Count; i++)
            {
                pendingRoute = RoutesList[i];
                if (pendingRoute.Equals(targetRoute))
                    continue;
                Point p1 = targetRoute.Start;
                Point p2 = targetRoute.End;
                Point p3 = pendingRoute.Start;
                Point p4 = pendingRoute.End;
                // Отрезок 1 справа налево или снизу вверх? - меняем направление
                if (p2.X < p1.X || (p2.X == p1.X && p2.Y < p1.Y))
                {
                    Point tmp = p1;
                    p1 = p2;
                    p2 = tmp;
                }
                // Отрезок 2 справа налево или снизу вверх? - меняем направление
                if (p4.X < p3.X || (p4.X == p3.X && p4.Y < p3.Y))
                {
                    Point tmp = p3;
                    p3 = p4;
                    p4 = tmp;
                }
                // Отрезки имеют общую потенциальную область пересечения?
                if (!((p2.X < p3.X || (p2.X == p3.X && p2.Y < p3.Y)) || (p4.X < p1.X || (p4.X == p1.X && p4.Y < p1.Y))))
                {
                    // Частный случай
                    // Какой-либо отрезок вертикальный?
                    if (p1.X == p2.X || p3.X == p4.X)
                    {
                        // Оба отрезка - вертикальные и они лежат на одной прямой?
                        if (p1.X == p2.X && p3.X == p4.X && p1.X == p3.X)
                        {
                            // Отрезок 1 имеет общий отрезок со 2? - значит пересекаются
                            if ((p1.Y >= p3.Y && p1.Y <= p4.Y) || (p2.Y >= p3.Y && p2.Y <= p4.Y))
                                resultsList.Add(pendingRoute);
                        }
                        // Первый отрезок - вертикальный?
                        else if (p1.X == p2.X && p3.X != p4.X)
                        {
                            double X1 = Math.Round(p1.X, 2);
                            double A2 = Math.Round((p3.Y - p4.Y) / (p3.X - p4.X), 2);
                            double B2 = Math.Round(p3.Y - A2 * p3.X, 2);
                            double Y1 = Math.Round(A2 * X1 + B2, 2);
                            // Точка пересечения (X1,Y1) входит в интервал 2-го отрезка? - значит пересекаются
                            if (p3.X <= X1 && p4.X >= X1 && Math.Min(p1.Y, p2.Y) <= Y1 && Math.Max(p1.Y, p2.Y) >= Y1)
                                resultsList.Add(pendingRoute);
                        }
                        // Второй отрезок - вертикальный?
                        else if (p3.X == p4.X && p1.X != p2.X)
                        {
                            double X2 = Math.Round(p3.X, 2);
                            double A1 = Math.Round((p1.Y - p2.Y) / (p1.X - p2.X), 2);
                            double B1 = Math.Round(p1.Y - A1 * p1.X, 2);
                            double Y2 = Math.Round(A1 * X2 + B1, 2);
                            // Точка пересечения (X2,Y2) входит в интервал 1-го отрезка? - значит пересекаются
                            if (p1.X <= X2 && p2.X >= X2 && Math.Min(p3.Y, p4.Y) <= Y2 && Math.Max(p3.Y, p4.Y) >= Y2)
                                resultsList.Add(pendingRoute);
                        }
                    }
                    // Оба отрезка - горизонтальные?
                    else if (p1.Y == p2.Y && p3.Y == p4.Y)
                    {
                        // Отрезки лежат на одной прямой?
                        if (p1.Y == p3.Y)
                            // Отрезок 1 имеет общий отрезок со 2? - значит пересекаются
                            if ((p1.X >= p3.X && p1.X <= p4.X) || (p2.X >= p3.X && p2.X <= p4.X))
                                resultsList.Add(pendingRoute);
                    }
                    // Общий случай
                    else
                    {
                        double A1 = Math.Round((p1.Y - p2.Y) / (p1.X - p2.X), 2);
                        double A2 = Math.Round((p3.Y - p4.Y) / (p3.X - p4.X), 2);
                        double B1 = Math.Round(p1.Y - A1 * p1.X, 2);
                        double B2 = Math.Round(p3.Y - A2 * p3.X, 2);
                        // Прямые 1 и 2 - параллельные?
                        if (A1 == A2)
                        {
                            // Прямые находятся на одной прямой?
                            if (B1 == B2)
                                resultsList.Add(pendingRoute);
                            else
                                continue;
                        }
                        else
                        {
                            //Xa - абсцисса точки пересечения двух прямых
                            double Xa = Math.Round((B2 - B1) / (A1 - A2), 2);
                            // Точка Xa находится вне пересечения проекций отрезков на ось X?
                            if ((Xa + 1 < Math.Max(p1.X, p3.X)) || (Xa - 1 > Math.Min(p2.X, p4.X)))
                                continue;
                            else
                                resultsList.Add(pendingRoute);
                        }
                    }

                }
            }
            return resultsList;
        }
        /// <summary>
        /// Попадание точки в тело маршрута
        /// </summary>
        /// <returns></returns>
        public bool IsInSight(Point targetPoint)
        {
            using (GraphicsPath gp_overlook = new GraphicsPath())
            {
                gp_overlook.AddPolygon(Body);
                if (gp_overlook.IsVisible(targetPoint.X, targetPoint.Y))
                    return true;
                return false;
            }
        }
    }
}