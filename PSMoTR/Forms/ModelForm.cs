using PSMoTR.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PSMoTR.Forms
{
    public partial class ModelForm : Form
    {
        // Поля
        MenuForm menuForm; // Форма меню
        int zoom = 6; // Масштаб (6 px : 1 метр)
        int timerCounter = 0; // Счетчик таймера
        int routeTypeCounter = 0; // Счетчик типа маршрута
        string currentConstructObjType = ""; // Текущий тип конструируемого объекта
        // Свойства
        public int ModelNumber { get; set; } = -1;
        public System.Drawing.Point StartLocation { get; set; } = new System.Drawing.Point(0, 0); // Начальная позиция формы
        public Classes.Point MousePoint { get; set; } = new Classes.Point();
        public Route Route_New { get; set; } = new Route(null, "straight", null, Color.Orange);
        public TrafficLight TL_New { get; set; } = new TrafficLight("X=0; Y=0; Speed=0; Type=usual; GD=0; RD=0; SSC=Gray; OL=0; LS=False");
        // Конструктор
        public ModelForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Загрузка окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelForm_Load(object sender, EventArgs e)
        {
            menuForm = Owner as MenuForm; // Задаем родителя для этой формы
        }
        /// <summary>
        /// Инициализация светофоров для модели по номеру
        /// </summary>
        /// <param name="modelNumber"></param>
        public void TrafficLightsInitialization()
        {
            // Светофоры считаны из файла?
            if (TrafficLight.TLsList.Count == 0)
            {
                TrafficLight.OpenFile(Project.Current.Directory.FullName + "\\traffic_lights.txt"); // Загружаем светофоры из файла
                TrafficLight.Initialization(); // Инициализируем все свойства загруженных светофоров
            }
            // Связывание алгоритмов светофоров с моделями
            switch (ModelNumber)
            {
                case 1:
                    Model1TrafficLight.BindingWithModel(this);
                    break;
                case 2:
                    Model2TrafficLight.BindingWithModel(this);
                    break;
                case 3:
                    Model3TrafficLight.BindingWithModel(this);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Адаптация размера формы под экран
        /// </summary>
        public void FormSizeAdaptation(int requiredWidth = 0)
        {
            Size resolution = Screen.PrimaryScreen.Bounds.Size; // Разрешение экрана
            Size = new Size(1708, 855);
            int diff_width;
            int diff_height;
            zoom = 6;
            bool exit = false;
            do
            {
                diff_width = Size.Width - PB_Model.Width; // 16
                diff_height = Size.Height - PB_Model.Height; // 39
                Size = new Size(PB_Model.Width / 6 * zoom + diff_width,
                    PB_Model.Height / 6 * zoom + diff_height);
                if (resolution.Width < Size.Width + requiredWidth)
                    zoom--;
                else
                    exit = true;
            } while (!exit || zoom == 2);
            Location = StartLocation;
        }
        /// <summary>
        /// Прорисовка и анимация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PB_Model_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Orange, 1); // Цвет линии и ширина
            Font font = new Font("", (float)1.1 * zoom); // Шрифт
            float pointSize = 1; // Размер точек
            // Прорисовка маршрутов
            if (menuForm.CB_ShowRoutes.Checked)
            {
                // Отображаем точки из списка
                for (int i = 0; i < Route.RoutesList.Count; i++)
                {
                    // Отображаем начальную и конечную точки маршрута
                    Route route = Route.RoutesList[i];
                    if (route.Start != null)
                        e.Graphics.FillEllipse(Brushes.Blue, (route.Start.X - pointSize / 2) * zoom,
                            (route.Start.Y - pointSize / 2) * zoom, pointSize * zoom, pointSize * zoom);
                    if (route.End is null)
                        continue;
                    e.Graphics.FillEllipse(Brushes.Blue, (route.End.X - pointSize / 2) * zoom,
                        (route.End.Y - pointSize / 2) * zoom, pointSize * zoom, pointSize * zoom);
                    // Отображение маршрута
                    pen.Color = route.Color;
                    // Путь - прямая?
                    if (route.Type == "straight")
                    {
                        e.Graphics.DrawLine(pen, new PointF(route.Start.X * zoom, route.Start.Y * zoom),
                            new PointF(route.End.X * zoom, route.End.Y * zoom));
                        //// Прорисовываем тело маршрута
                        //PointF[] polygon = new PointF[route.Body.Length];
                        //for (int j = 0; j < polygon.Length; j++)
                        //{
                        //    polygon[j].X = zoom * route.Body[j].X;
                        //    polygon[j].Y = zoom * route.Body[j].Y;
                        //}
                        //pen = new Pen(Color.Violet, 1);
                        //e.Graphics.DrawPolygon(pen, polygon);
                    }
                    // Путь - дуга X или Y?
                    else if (route.Type == "curveX" || route.Type == "curveY")
                    {
                        e.Graphics.DrawArc(pen,
                            route.Body[0].X * zoom, route.Body[0].Y * zoom, route.Body[1].X * zoom, route.Body[1].Y * zoom,
                            route.Body[2].X, route.Body[2].Y); // Рисуем дугу
                    }
                    // Если необходимо отображать скорость точек
                    if (menuForm.CB_ShowPointsSpeed.Checked)
                    {
                        e.Graphics.DrawString(route.Start.Speed.ToString(),
                            font, Brushes.Red, new PointF(route.Start.X * zoom, (route.Start.Y + (float)0.5) * zoom));
                        e.Graphics.DrawString(route.End.Speed.ToString(),
                            font, Brushes.Red, new PointF(route.End.X * zoom, (route.End.Y + (float)0.5) * zoom));
                    }
                    // Если необходимо отобразить путь авто
                    if (menuForm.CB_ShowAutoWay.Checked)
                    {
                        if (menuForm.TB_AutoNumber.Text != "")
                        {
                            try
                            {
                                Auto auto = Auto.AutosList[int.Parse(menuForm.TB_AutoNumber.Text)];
                                if (auto is null)
                                {
                                    menuForm.TB_AutoNumber.Text = "";
                                    menuForm.CB_ShowAutoWay.Checked = false;
                                    return;
                                }
                                for (int j = 0; j < auto.Way.RoutesList.Count; j++)
                                {
                                    auto.Way.RoutesList[j].Color = Color.LightBlue;
                                }
                                auto.CurrentRoute.Color = Color.DarkBlue;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                menuForm.TB_AutoNumber.Text = "";
                                menuForm.CB_ShowAutoWay.Checked = false;
                                MessageBox.Show("Введен несуществующий номер авто");
                            }
                            catch (FormatException)
                            {
                                menuForm.TB_AutoNumber.Text = "";
                                menuForm.CB_ShowAutoWay.Checked = false;
                                MessageBox.Show("Введены некорректные данные в поле \"Номер авто\"");
                            }
                        }
                    }
                    else
                        route.Color = Color.Orange;
                }
            }
            // Прорисовка светофоров
            for (int i = 0; i < TrafficLight.TLsList.Count; i++)
            {
                TrafficLight tl = TrafficLight.TLsList[i];
                // Прорисовываем цвет сигнала светофора
                pen = new Pen(tl.CurrentSC, 3);
                pointSize = (float)2.7;
                e.Graphics.FillEllipse(pen.Brush, (tl.Position.X - pointSize / 2) * zoom,
                    (tl.Position.Y - pointSize / 2) * zoom, pointSize * zoom, pointSize * zoom);
                // Прорисовываем контур светофора
                pointSize = 3;
                e.Graphics.DrawImage(tl.Icon, (tl.Position.X - pointSize / 2) * zoom,
                    (tl.Position.Y - pointSize / 2) * zoom, pointSize * zoom, pointSize * zoom);
                // Прорисовываем таймер светофора
                if (!(tl.BaseRoute is null))
                {
                    float x = tl.Position.X;
                    float y = tl.Position.Y;
                    if (tl.BaseRoute.Course[0] == "right")
                        x += 2;
                    else if (tl.BaseRoute.Course[0] == "left")
                        x -= 2;
                    else if (tl.BaseRoute.Course[0] == "up")
                        y -= 2;
                    else if (tl.BaseRoute.Course[0] == "down")
                        y += 2;
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    font = new Font("Times", (float)1.5 * zoom);
                    e.Graphics.DrawString(tl.TimerCounter.ToString(), font, Brushes.Black, x * zoom, y * zoom, sf);
                }
                // Прорисовываем поле зрения сфетовора, если включено его отображение
                if (menuForm.CB_ShowOverlook.Checked == true)
                {
                    PointF[] polygon = new PointF[tl.Overlook.Length];
                    for (int j = 0; j < polygon.Length; j++)
                    {
                        polygon[j].X = zoom * tl.Overlook[j].X;
                        polygon[j].Y = zoom * tl.Overlook[j].Y;
                    }
                    pen = new Pen(Color.Brown, 1);
                    e.Graphics.DrawPolygon(pen, polygon); // Рисуем полигон поля видимости авто
                }
            }
            // Прорисока машин
            pen = new Pen(Color.Black, 1);
            for (int i = 0; i < Auto.AutosList.Count; i++)
            {
                if (Auto.AutosList[i] is null)
                    continue;
                // Создаем контур (прямоугольник) авто
                float coord_X_leftTop = (Auto.AutosList[i].X - Auto.AutosList[i].Length / 2) * zoom; // Координата X левого верхнего угла авто
                float coord_Y_leftTop = (Auto.AutosList[i].Y - Auto.AutosList[i].Width / 2) * zoom; // Координата Y левого верхнего угла авто
                RectangleF rect = new RectangleF(coord_X_leftTop, coord_Y_leftTop,
                    Auto.AutosList[i].Length * zoom, Auto.AutosList[i].Width * zoom); // Контур авто

                // Отображение авто
                float centerX = rect.Left + rect.Width / 2; // Координата X центра авто
                float centerY = rect.Top + rect.Height / 2; // Координата Y центра авто
                e.Graphics.FillEllipse(Brushes.Red, centerX, centerY, 3, 3); // Точка центра авто
                e.Graphics.TranslateTransform(centerX, centerY); // Применить ось координат на центр авто
                e.Graphics.RotateTransform(Auto.AutosList[i].Rotation); // Поворот оси на заданный угол
                e.Graphics.FillRectangle(Brushes.DarkGray, -rect.Width / 2, -rect.Height / 2, rect.Width, rect.Height); // Перерисовывание контура 
                                                                                                           // авто так, что центр оси в центре авто
                e.Graphics.ResetTransform(); // Сброс трансформаций и возврат к изначальной системе координат
                // Текст на авто
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(i.ToString(), font, Brushes.Black, rect, sf);
                // Если необходимо отображать поле видимости авто
                if (menuForm.CB_ShowOverlook.Checked)
                {
                    // Масштабируем полигон и поле видимости авто
                    PointF[] polygon = new PointF[Auto.AutosList[i].Overlook.Length];
                    for (int j = 0; j < polygon.Length; j++)
                    {
                        polygon[j].X = zoom * Auto.AutosList[i].Overlook[j].X;
                        polygon[j].Y = zoom * Auto.AutosList[i].Overlook[j].Y;
                    }
                    pen = new Pen(Color.Blue, 1);
                    e.Graphics.DrawPolygon(pen, polygon); // Рисуем полигон поля видимости авто
                    // Масштабируем полигон и тело авто
                    polygon = new PointF[Auto.AutosList[i].Body.Length];
                    for (int j = 0; j < polygon.Length; j++)
                    {
                        polygon[j].X = zoom * Auto.AutosList[i].Body[j].X;
                        polygon[j].Y = zoom * Auto.AutosList[i].Body[j].Y;
                    }
                    pen = new Pen(Color.Red, 1);
                    e.Graphics.DrawPolygon(pen, polygon); // Рисуем полигон поля видимости авто
                    for (int j = 0; j < polygon.Length; j++)
                    {
                        e.Graphics.FillEllipse(Brushes.Black, polygon[j].X, polygon[j].Y, 3, 3); // Точка корпуса авто
                    }
                }
            }
            // Освобождаем ресурсы
            pen.Dispose();
            font.Dispose();
            e.Dispose();
        }
        /// <summary>
        /// Событие при движении мыши по окну отображения модели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PB_Model_MouseMove(object sender, MouseEventArgs e)
        {
            // Не включен режим "Construct routes"?
            if (!menuForm.IsRoutesConstructMode)
                return;
            // Поиск рядом точки, у которой расстояние между указателем мыши и ней 1 метр
            Classes.Point nearestPoint = Classes.Point.PointsList.Find(p => p != MousePoint &&
                Classes.Point.Distance(p, new Classes.Point(e.X / zoom, e.Y / zoom)) < 1);
            // Поиск рядом светофора, у которого расстояние между указателем мыши и ним 1 метр
            TrafficLight nearestTL = TrafficLight.TLsList.Find(tl => tl.Position != MousePoint &&
                Classes.Point.Distance(tl.Position, new Classes.Point(e.X / zoom, e.Y / zoom)) < 1); ;
            // Поиск рядом маршрутов, в поле зрения которых указатель
            List<Route> nearestRoutes = Route.RoutesList.FindAll(r =>
                r != Route_New && r.Type == "straight" && r.IsInSight(new Classes.Point(e.X / zoom, e.Y / zoom)));
            // "Прилипаем" указатель к близжайшей точке, если она есть
            if (nearestPoint != null)
            {
                MousePoint.X = nearestPoint.X;
                MousePoint.Y = nearestPoint.Y;
                MousePoint.Speed = nearestPoint.Speed;
            }
            // "Прилипаем" указатель к близжайшему светофору, если он есть
            else if (nearestTL != null)
            {
                MousePoint.X = nearestTL.Position.X;
                MousePoint.Y = nearestTL.Position.Y;
                MousePoint.Speed = nearestTL.Position.Speed;
            }
            // Рядом есть маршруты и он один?
            else if (nearestRoutes.Count == 1)
            {
                Route nearestRoute = nearestRoutes[0];
                // Маршрут - вертикальный?
                if (nearestRoute.Start.X == nearestRoute.End.X)
                {
                    MousePoint.X = nearestRoute.Start.X;
                    MousePoint.Y = (float)Math.Round((float)e.Y / zoom, 1);
                }
                else
                {
                    if (Math.Abs(nearestRoute.End.X - nearestRoute.Start.X) > 5)
                    {
                        MousePoint.X = (float)Math.Round((float)e.X / zoom, 1);
                        MousePoint.Y = Route.StraightEquationX(nearestRoute.Start, nearestRoute.End, MousePoint.X);
                    }
                    else
                    {
                        MousePoint.Y = (float)Math.Round((float)e.Y / zoom, 1);
                        MousePoint.X = Route.StraightEquationY(nearestRoute.Start, nearestRoute.End, MousePoint.Y);
                    }
                }
                MousePoint.Speed = 0;
            }
            // Иначе точка на указателе мыши
            else
            {
                // Обновление координат указателя мыши
                MousePoint.X = (float)Math.Round((float)e.X / zoom, 1);
                MousePoint.Y = (float)Math.Round((float)e.Y / zoom, 1);
                MousePoint.Speed = 0;
                // Текущий объект конструктора - маршрут?
                if (currentConstructObjType == "route")
                {
                    // Текущая точка указателя - конечная точка дуги? : пропорциональное изменение координат
                    if (Route_New.End == MousePoint && (Route_New.Type == "curveX" || Route_New.Type == "curveY"))
                    {
                        float deltaX = Route_New.End.X - Route_New.Start.X;
                        float deltaY = Route_New.End.Y - Route_New.Start.Y;
                        if (deltaX < 0)
                            deltaX = -deltaX;
                        if (deltaY > 0)
                            MousePoint.Y = Route_New.Start.Y + deltaX;
                        else
                            MousePoint.Y = Route_New.Start.Y - deltaX;
                    }
                }
            }
            PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие при клике кнопки мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PB_Model_MouseClick(object sender, MouseEventArgs e)
        {
            // Не включен режим "Construct routes"?
            if (!menuForm.IsRoutesConstructMode)
                return;
            // Действие при клике ЛКМ
            if (e.Button == MouseButtons.Left)
            {
                // Текущий объект конструктора - маршрут?
                if (currentConstructObjType == "route")
                {
                    // Конечная точка не задана у нового маршрута?
                    if (Route_New.End is null)
                    {
                        // Задаем начальной точкой текущее положение указателя и назначаем конечной точке указатель
                        Route_New.Start = MousePoint.Clone();
                        Route_New.End = MousePoint;
                    }
                    else
                    {
                        // Удаляем из списка последнюю временную точку - указатель мыши, т.к. ссылочная связь потеряена, а также эта точка мешает
                        // корректно отработать методу Point.FindSimilarPoint() при добавлении маршрута в список с соблюдением целостности
                        Classes.Point.PointsList.RemoveAt(Classes.Point.PointsList.Count - 1);
                        // Удаляем из списка временный маршрут
                        Route.RoutesList.Remove(Route_New);
                        // Добавляем новый маршрут к списку
                        Route.List_SafelyAdd(Route_New.Clone());
                        // Заполнение списков временными объектами: указатель и маршрут
                        Classes.Point.PointsList.Add(MousePoint);
                        Route_New.Start = MousePoint;
                        Route_New.End = null;
                        Route.RoutesList.Add(Route_New);
                    }
                }
                // Текущий объект конструктора - светофор?
                else if (currentConstructObjType == "tl")
                {
                    // Удаляем из списка последнюю временную точку - указатель мыши, т.к. ссылочная связь потеряена
                    Classes.Point.PointsList.RemoveAt(Classes.Point.PointsList.Count - 1);
                    // Удаляем из списка временный светофор
                    TrafficLight.TLsList.Remove(TL_New);
                    // Добавляем в список новый светофор
                    TrafficLight.List_SafelyAdd(TL_New.Clone());
                    // Заполнение списков временными объектами: указатель и светофор
                    Classes.Point.PointsList.Add(MousePoint);
                    TL_New.Position = MousePoint;
                    TrafficLight.TLsList.Add(TL_New);
                }
            }
            // Действие при клике ПКМ
            else if (e.Button == MouseButtons.Right)
            {
                // Текущий объект конструктора - маршрут?
                if (currentConstructObjType == "route")
                {
                    // Если начальная точка уже задана и указатель на конечной, то перемещаем указатель на начальную
                    if (Route_New.Start != null && Route_New.End != null)
                    {
                        Route_New.Start = MousePoint;
                        Route_New.End = null;
                        Route_New.Type = "straight";
                    }
                }
            }
            // Действие при клике на СКМ
            else if (e.Button == MouseButtons.Middle)
            {
                // Текущий объект конструктора - светофор?
                if (currentConstructObjType == "tl")
                {
                    // Начальный сигнал - зеленый или серый?
                    if (TL_New.StartSC == Color.Green || TL_New.StartSC == Color.Gray)
                    {
                        TL_New.StartSC = Color.Red;
                        TL_New.CurrentSC = Color.Red;
                    }
                    // Начальный сигнал - красный?
                    else if (TL_New.StartSC == Color.Red)
                    {
                        TL_New.StartSC = Color.Green;
                        TL_New.CurrentSC = Color.Green;
                    }
                }
            }
        }
        /// <summary>
        /// Событие при вращении колесика мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PB_Model_MouseWheel(object sender, MouseEventArgs e)
        {
            // Текущий объект конструктора - маршрут?
            if (currentConstructObjType == "route")
            {
                // Колесико мыши вверх?
                if (e.Delta > 0)
                    routeTypeCounter++;
                // Колесико мыши вниз?
                else if (e.Delta < 0)
                    routeTypeCounter--;
                // Счетчик типа достиг верхнего предела?
                if (routeTypeCounter > Enum.GetNames(typeof(Route.Types)).Length - 1)
                    routeTypeCounter = 0;
                // Счетчик типа достиг нижнего предела?
                else if (routeTypeCounter < 0)
                    routeTypeCounter = Enum.GetNames(typeof(Route.Types)).Length - 1;
                // Задаем тип маршрута
                Route_New.Type = Enum.GetName(typeof(Route.Types), routeTypeCounter);
            }
        }
        /// <summary>
        /// Событие нажатия клавиши при выделенной форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Не включен режим "Construct routes"?
            if (!menuForm.IsRoutesConstructMode)
                return;
            // Клавиша нажата маршрутов "R"?
            if (e.KeyCode == Keys.R)
            {
                currentConstructObjType = "route";
                if (!Route.RoutesList.Contains(Route_New))
                {
                    Route_New.Start = MousePoint;
                    Route.RoutesList.Add(Route_New);
                }
                TrafficLight.TLsList.Remove(TL_New);
            }
            // Клавиша нажата светофоров "T"?
            else if (e.KeyCode == Keys.T)
            {
                currentConstructObjType = "tl";
                if (!TrafficLight.TLsList.Contains(TL_New))
                {
                    TL_New = new TrafficLight(MousePoint);
                    TrafficLight.TLsList.Add(TL_New);
                }
                Route.RoutesList.Remove(Route_New);
            }
        }
        /// <summary>
        /// Событие тика таймера движения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerMove_Tick(object sender, EventArgs e)
        {
            bool findObstacles = false;
            for (int i = 0; i < Auto.AutosList.Count; i++)
            {
                // Авто не существует?
                if (Auto.AutosList[i] is null)
                    continue;
                // !!! Оптимизация !!! Как часто вызывать проверку на наличие помех
                if (timerCounter % 40 == 0)
                    findObstacles = true;
                Auto.AutosList[i].Move(Auto.AutosList, TrafficLight.TLsList, findObstacles);
                findObstacles = false;
            }
            timerCounter += TimerMove.Interval;
            Invalidate(); // Перерисовка
        }
    }
}