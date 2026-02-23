using PSMoTR.Classes;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace PSMoTR.Forms
{
    public partial class TrafficLightsEditForm : Form
    {
        // Поля
        ModelForm modelForm; // Форма модели для редактирования
        private int choosenRow = -1; // Индекс выделенной строки
        // Конструктор
        public TrafficLightsEditForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrafficLightsEditForm_Load(object sender, EventArgs e)
        {
            modelForm = Owner as ModelForm; // Задаем родителя
            BS.DataSource = TrafficLight.TLsList; // Создаем связь binding со списком
            BS.AllowNew = true; // Разрешение на добавление нового элемента
            DGV_TrafficLights.DataSource = BS; // Указываем источником таблицы для отображения нашу связь binding
            // Настраиваем колонки
            DGV_TrafficLights.RowHeadersWidth = 50; // Ширина системной колонки
            DGV_TrafficLights.Columns["Position"].DisplayIndex = 0;
            DGV_TrafficLights.Columns["Position"].Width = 150;
            DGV_TrafficLights.Columns["Type"].DisplayIndex = 1;
            DGV_TrafficLights.Columns["Type"].Width = 50;
            //DGV_TrafficLights.Columns["Type"].HeaderText = "Тип";
            DGV_TrafficLights.Columns["Icon"].DisplayIndex = 2;
            DGV_TrafficLights.Columns["Icon"].Width = 50;
            //DGV_TrafficLights.Columns["Icon"].HeaderText = "Икнока";
            DGV_TrafficLights.Columns["GD"].DisplayIndex = 3;
            DGV_TrafficLights.Columns["GD"].Width = 70;
            //DGV_TrafficLights.Columns["GD"].HeaderText = "Таймер зеленого сигнала";
            DGV_TrafficLights.Columns["RD"].DisplayIndex = 4;
            DGV_TrafficLights.Columns["RD"].Width = 70;
            //DGV_TrafficLights.Columns["GD"].HeaderText = "Таймер зеленого сигнала";
            DGV_TrafficLights.Columns["SkipD"].DisplayIndex = 5;
            DGV_TrafficLights.Columns["SkipD"].Width = 70;
            //DGV_TrafficLights.Columns["RD"].HeaderText = "Таймер красного сигнала";
            DGV_TrafficLights.Columns["StartSC"].DisplayIndex = 6;
            DGV_TrafficLights.Columns["StartSC"].Width = 70;
            //DGV_TrafficLights.Columns["StartSC"].HeaderText = "Начальный цвет сигнала";
            DGV_TrafficLights.Columns["OL"].DisplayIndex = 7;
            DGV_TrafficLights.Columns["OL"].Width = 70;
            //DGV_TrafficLights.Columns["OL"].HeaderText = "Длина поля видимости";
            DGV_TrafficLights.Columns["LimitedSection"].DisplayIndex = 8;
            DGV_TrafficLights.Columns["LimitedSection"].Width = 85;
            //DGV_TrafficLights.Columns["LimitedSection"].HeaderText = "Ограниченная секция дороги";
            DGV_TrafficLights.Columns["OD"].Visible = false;
            DGV_TrafficLights.Columns["CurrentSC"].Visible = false;
            DGV_TrafficLights.Columns["PreviousSC"].Visible = false;
            DGV_TrafficLights.Columns["Timer"].Visible = false;
            DGV_TrafficLights.Columns["TimerCounter"].Visible = false;
            DGV_TrafficLights.Columns["BaseRoute"].Visible = false;
            DGV_TrafficLights.Columns["Number"].Visible = false;
        }
        /// <summary>
        /// Событие после закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrafficLightsEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Список светофоров не пуст?
            if (TrafficLight.TLsList.Count != 0)
                TrafficLight.TLsList[choosenRow].CurrentSC = TrafficLight.TLsList[choosenRow].StartSC; // Сбрасываем цвет выделенного светофора
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие прорисовки строк, но до прорисовки ячеек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            object head = DGV_TrafficLights.Rows[e.RowIndex].HeaderCell.Value;
            if (head == null || !head.Equals(e.RowIndex.ToString()))
                DGV_TrafficLights.Rows[e.RowIndex].HeaderCell.Value = e.RowIndex.ToString();
        }
        /// <summary>
        /// Событие выделения строки/ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            choosenRow = e.RowIndex;
            // Выделяем редактируемый маршрут
            for (int i = 0; i < TrafficLight.TLsList.Count; i++)
                TrafficLight.TLsList[i].CurrentSC = TrafficLight.TLsList[i].StartSC;
            TrafficLight.TLsList[choosenRow].CurrentSC = Color.Gray;
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие нажатия на клавишу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_KeyDown(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Delete и выделена какая-либо строка
            if (e.KeyCode == Keys.Delete && choosenRow >= 0)
                BS.RemoveAt(choosenRow); // Удаление маршрута из списка через binding (в том числе и из таблицы)
        }
        /// <summary>
        /// Нажатие на кнопку "Сохранить в файл"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_SaveFile_Click(object sender, EventArgs e)
        {
            TrafficLight.RewriteFile(Project.Current.Directory.FullName + "\\traffic_lights.txt");
            MessageBox.Show("Сохранение выполнено успешно");
        }
        /// <summary>
        /// Событие перед добавлением последней строки в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_AddingNew(object sender, AddingNewEventArgs e)
        {
            TrafficLight trafficLight = new TrafficLight("X=0; Y=0; Speed=0; Type=usual; GD=0; RD=0; SSC=Red; OL=0; LS=False");
            e.NewObject = trafficLight;
        }
        /// <summary>
        /// Событие при изменении данных списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_ListChanged(object sender, ListChangedEventArgs e)
        {
            modelForm.Invalidate();
            DGV_TrafficLights.Invalidate();
        }
        /// <summary>
        /// Событие при окончании редактирования содержимого ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string columnName = DGV_TrafficLights.Columns[e.ColumnIndex].Name; // Имя выбранной колонки
            if (columnName == "Position")
            {
                TrafficLight editingTL = TrafficLight.TLsList[e.RowIndex]; // Объект изменяемого светофора
                Classes.Point editingPosition = editingTL.Position;
                // Определяем новую позицию, совпадающую точку и старую позицию
                Classes.Point newPosition = new Classes.Point(e.Value.ToString()); // Новое значение изменяемой точки
                Classes.Point similarPoint = Classes.Point.PointsList.Find(p => p != newPosition && p.Equals(newPosition));
                Classes.Point oldPosition = editingPosition.Clone(); // Значение изменяемой точки до изменений
                // Если у позиции изменили только скорость
                if (similarPoint != null && newPosition.X == oldPosition.X && newPosition.Y == oldPosition.Y)
                    editingPosition.Speed = newPosition.Speed;
                // Иначе
                else
                {
                    // Если в коллекции нет такой точки, то меняем координаты
                    if (similarPoint == null)
                    {
                        editingPosition.X = newPosition.X;
                        editingPosition.Y = newPosition.Y;
                        editingPosition.Speed = newPosition.Speed;
                    }
                    // Иначе привязываем к уже существующей аналогичной точке
                    else
                    {
                        Route.EditingSimilarPoints(editingPosition, similarPoint);
                        editingPosition = similarPoint;
                        Classes.Point.PointsList.Remove(oldPosition);
                    }
                }
                e.Value = editingPosition;
            }
            e.ParsingApplied = true;
        }
        /// <summary>
        /// Событие клика по ячейке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_TrafficLights.CurrentCell.OwningColumn.Name == "StartSC")
            {
                // Делаем из TextBox-ячейки ComboBox-ячейку
                DataGridViewComboBoxCell dgv_cb_SSCs = new DataGridViewComboBoxCell();
                dgv_cb_SSCs.Items.Add(Color.Green);
                dgv_cb_SSCs.Items.Add(Color.Red);
                DGV_TrafficLights[e.ColumnIndex, e.RowIndex] = dgv_cb_SSCs;
            }
        }
        /// <summary>
        /// Событие при отведении указателя мыши от ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_TrafficLights.CurrentCell != null && DGV_TrafficLights.CurrentCell.OwningColumn.Name == "StartSC")
            {
                if (DGV_TrafficLights.CurrentCell is DataGridViewComboBoxCell)
                {
                    DGV_TrafficLights[e.ColumnIndex, e.RowIndex] = new DataGridViewTextBoxCell
                    {
                        Value = DGV_TrafficLights.CurrentCell.Value
                    };
                }
            }
        }
        /// <summary>
        /// Событие обработки ошибки данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show("Ошибка данных (DataError)", "Ошибка");
        }
        /// <summary>
        /// Событие отображение элемента управления для редактирования ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_TrafficLights_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox cb_SSCs)
            {
                cb_SSCs.DrawMode = DrawMode.OwnerDrawFixed;
                cb_SSCs.DrawItem -= CB_DrawItem;
                cb_SSCs.DrawItem += new DrawItemEventHandler(CB_DrawItem);
                cb_SSCs.SelectionChangeCommitted -= CB_SelectionChangeCommitted;
                cb_SSCs.SelectionChangeCommitted += new EventHandler(CB_SelectionChangeCommitted);
            }
        }
        /// <summary>
        /// Событие прорисовки элемента комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is ComboBox cb)
            {
                using (SolidBrush signalColor = new SolidBrush((Color)cb.Items[e.Index]))
                {
                    string signalName = signalColor.Color.Name;
                    e.Graphics.FillRectangle(signalColor, e.Bounds);
                    e.Graphics.DrawString(signalName, e.Font, Brushes.Black, e.Bounds);
                }
            }
        }
        /// <summary>
        /// Событие после изменения значения у выбранного комбобокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                if (DGV_TrafficLights.CurrentCell is DataGridViewComboBoxCell)
                {
                    DGV_TrafficLights[DGV_TrafficLights.CurrentCell.ColumnIndex, DGV_TrafficLights.CurrentCell.RowIndex] = new DataGridViewTextBoxCell
                    {
                        Value = cb.Text
                    };
                }
            }
        }
    }
}