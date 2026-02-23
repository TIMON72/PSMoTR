using PSMoTR.Classes;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace PSMoTR.Forms
{
    public partial class RoutesEditForm : Form
    {
        // Поля
        ModelForm modelForm; // Форма 1-ой модели
        private int choosenRow = -1; // Индекс выделенной строки
        // Конструктор
        public RoutesEditForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoutesEditForm_Load(object sender, EventArgs e)
        {
            modelForm = Owner as ModelForm; // Задаем родителя
            bindingSource.DataSource = Route.RoutesList; // Создаем связь binding со списком
            bindingSource.AllowNew = true; // Разрешение на добавление нового элемента
            DGV_Routes.DataSource = bindingSource; // Указываем источником таблицы для отображения нашу связь binding
            // Даем названия заголовкам колонок
            DGV_Routes.RowHeadersWidth = 50; // Ширина системной колонки
            DGV_Routes.Columns["Start"].DisplayIndex = 0;
            DGV_Routes.Columns["Start"].Width = 180;
            //DGV_Routes.Columns["Start"].HeaderText = "Нач. точка";
            DGV_Routes.Columns["Type"].DisplayIndex = 1;
            DGV_Routes.Columns["Type"].Width = 60;
            //DGV_Routes.Columns["Type"].HeaderText = "Тип маршрута";
            DGV_Routes.Columns["End"].DisplayIndex = 2;
            DGV_Routes.Columns["End"].Width = 180;
            //DGV_Routes.Columns["End"].HeaderText = "Кон. точка";
            DGV_Routes.Columns["Rotation"].Visible = false;
            DGV_Routes.Columns["Color"].Visible = false;
            DGV_Routes.Columns["Number"].Visible = false;
        }
        /// <summary>
        /// Событие перед добавлением последней строки в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_AddingNew(object sender, AddingNewEventArgs e)
        {
            Route route = new Route(new Classes.Point(0, 0, 0), "straight", new Classes.Point(10, 10, 0));
            e.NewObject = route;
        }
        /// <summary>
        /// Событие при изменении данных списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_ListChanged(object sender, ListChangedEventArgs e)
        {
            modelForm.PB_Model.Invalidate();
            DGV_Routes.Invalidate();
        }
        /// <summary>
        /// Событие прорисовки строк, но до прорисовки ячеек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            object head = DGV_Routes.Rows[e.RowIndex].HeaderCell.Value;
            if (head == null || !head.Equals(e.RowIndex.ToString()))
                DGV_Routes.Rows[e.RowIndex].HeaderCell.Value = e.RowIndex.ToString();
        }
        /// <summary>
        /// Событие выделения строки/ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            choosenRow = e.RowIndex;
            // Выделяем редактируемый маршрут
            for (int i = 0; i < Route.RoutesList.Count; i++)
                Route.RoutesList[i].Color = Color.Orange;
            Route.RoutesList[choosenRow].Color = Color.Red;
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие нажатия на клавишу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_KeyDown(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Delete и выделена какая-либо строка
            if (e.KeyCode == Keys.Delete && choosenRow >= 0)
            {
                Route route_rem = Route.RoutesList[choosenRow];
                bindingSource.Remove(route_rem);
                Route.List_SafelyRemove(route_rem);
            }
        }
        /// <summary>
        /// Событие после добавления последней строки в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            int pos = e.Row.Index - 1;
            bool isOK = Route.List_SafelyAdd(Route.RoutesList[pos]);
            if (!isOK)
            {
                bindingSource.RemoveAt(pos);
                MessageBox.Show("Аналогичный маршрут уже существует", "Ошибка");
            }
        }
        /// <summary>
        /// Событие при окончании редактирования содержимого ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string columnName = DGV_Routes.Columns[e.ColumnIndex].Name; // Имя выбранной колонки
            if (columnName == "Start" || columnName == "End")
            {
                Route editingRoute = Route.RoutesList[e.RowIndex]; // Объект изменяемого маршрута
                Classes.Point editingPoint = null;
                // Если выделена определенная колонка, то задаем изменяемой точкой ее значение
                if (columnName == "Start")
                    editingPoint = editingRoute.Start;
                else if (columnName == "End")
                    editingPoint = editingRoute.End;
                // Определяем новую, совпадающую и старую точку
                Classes.Point newPoint = new Classes.Point(e.Value.ToString()); // Новое значение изменяемой точки
                Classes.Point similarPoint = Classes.Point.PointsList.Find(p => p != newPoint && p.Equals(newPoint));
                Classes.Point oldPoint = editingPoint.Clone(); // Значение изменяемой точки до изменений
                // Если у точки изменили только скорость
                if (similarPoint != null && newPoint.X == oldPoint.X && newPoint.Y == oldPoint.Y)
                {
                    editingPoint.Speed = newPoint.Speed;
                    similarPoint.Speed = editingPoint.Speed;
                }
                // Иначе
                else
                {
                    // Если в коллекции нет такой точки, то меняем координаты
                    if (similarPoint == null)
                    {
                        editingPoint.X = newPoint.X;
                        editingPoint.Y = newPoint.Y;
                        editingPoint.Speed = newPoint.Speed;
                    }
                    // Иначе привязываем к уже существующей аналогичной точке
                    else
                    {
                        Route.EditingSimilarPoints(editingPoint, similarPoint);
                        editingPoint = similarPoint;
                        Classes.Point.PointsList.Remove(oldPoint);
                    }
                }
                e.Value = editingPoint;
            }
            e.ParsingApplied = true;
        }
        /// <summary>
        /// Нажатие на кнопку "Сохранить в файл"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SaveFile_Click(object sender, EventArgs e)
        {
            Classes.Point.RewriteFile(Project.Current.Directory.FullName + "\\points.txt");
            Route.RewriteFile(Project.Current.Directory.FullName + "\\routes.txt");
            MessageBox.Show("Сохранение выполнено успешно");
            Close();
        }
    }
}