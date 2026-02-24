using PSMoTR.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PSMoTR.Forms
{
    public partial class MenuForm : Form
    {
        // Свойства
        public bool IsRoutesConstructMode { get; private set; }
        // Поля
        private static bool oddPauseClicked = true; // Флаг для кнопки "Пауза"
        internal int waves = 10; // Количество генераций машин
        private long timerCounter = 0; // Счетчик таймера
        private readonly ModelForm modelForm = new ModelForm(); // Форма модели 1
        private ProjectEditForm projectEditForm; // Форма изменения проекта
        private RoutesEditForm routeEditForm; // Форма редактора маршрутов
        private TrafficLightsEditForm trafficLightEditForm; // Форма редактора светофоров
        /// <summary>
        /// Конструктор
        /// </summary>
        public MenuForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuForm_Load(object sender, EventArgs e)
        {
            // Настраиваем форму
            L_TimerMove.Text = "0"; // Отображаемый текст для таймера
            B_PauseContinue.Enabled = false;
            B_Stop.Enabled = false;
            CB_NewGenerateAutos.Enabled = false;
            // Вызываем диалог с выбором проекта через событие кнопки
            B_EditProject.PerformClick();
        }
        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save(); // Сохраняем конфигурацию приложения
        }
        /// <summary>
        /// Событие нажатия кнопки "Управление проектами"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_EditProject_Click(object sender, EventArgs e)
        {
            // Задаем ребенка и запускаем форму
            projectEditForm = new ProjectEditForm
            {
                Owner = this
            };
            projectEditForm.ShowDialog();
        }
        /// <summary>
        /// Событие нажатия кнопки "Старт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_Start_Click(object sender, EventArgs e)
        {
            // Считываем данные с формы
            waves = Convert.ToInt32(TB_WavesCount.Text);
            // Активация основоного таймера
            Timer.Enabled = true;
            Timer.Start();
            // Меняем доступ к элементам управления меню
            B_Start.Enabled = false;
            B_PauseContinue.Enabled = true;
            B_Stop.Enabled = true;
            TB_WavesCount.Enabled = false;
            CB_NewGenerateAutos.Enabled = false;
            CLB_Models.Enabled = false;
            modelForm.TimerMove.Enabled = true;
            // Запускаем таймеры движения и светофоров
            modelForm.TimerMove.Start();
            TrafficLight.TimersAction("start", modelForm);
        }
        /// <summary>
        /// Событие нажатия кнопки "Пауза / Продолжить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_PauseContinue_Click(object sender, EventArgs e)
        {
            if (oddPauseClicked)
            {
                Timer.Stop(); // Останавливаем основной таймера
                oddPauseClicked = false;
                modelForm.TimerMove.Stop();
                TrafficLight.TimersAction("pause/continue", modelForm);
            }
            else
            {
                Timer.Start(); // Останавливаем основной таймера
                oddPauseClicked = true;
                modelForm.TimerMove.Start();
                TrafficLight.TimersAction("pause/continue", modelForm);
            }
        }
        /// <summary>
        /// Событие нажатия кнопки "Стоп"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_Stop_Click(object sender, EventArgs e)
        {
            // Остановка основного таймера
            Timer.Stop();
            Timer.Enabled = false;
            timerCounter = 0;
            // Меняем доступ к элементам управления меню
            B_Start.Enabled = true;
            B_PauseContinue.Enabled = false;
            B_Stop.Enabled = false;
            TB_WavesCount.Enabled = true;
            CB_NewGenerateAutos.Enabled = true;
            CB_NewGenerateAutos.Checked = false;
            CLB_Models.Enabled = true;
            modelForm.TimerMove.Stop();
            modelForm.TimerMove.Enabled = false;
            // Останавливаем таймеры движения и светофоров
            TrafficLight.TimersAction("stop", modelForm);
            modelForm.Invalidate();
            // Очистка списков авто у всех моделей
            Auto.AutosList.Clear();
            // Возвращаем флаг паузы в исходное состояние
            oddPauseClicked = true;
        }
        /// <summary>
        /// Событие отметки "Новая генерация авто"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_NewGenerateAutos_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_NewGenerateAutos.Checked)
                TB_WavesCount.Enabled = true;
            else
                TB_WavesCount.Enabled = false;
        }
        /// <summary>
        /// Событие выбора модели в CLB_Models
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CLB_Models_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < CLB_Models.Items.Count; i++)
            {
                if (CLB_Models.Items[i] != CLB_Models.SelectedItem)
                    CLB_Models.SetItemChecked(i, false);
            }
            //modelForm.Close();
            modelForm.StartLocation = new System.Drawing.Point(0, 0);
            modelForm.Owner = this;
            modelForm.FormSizeAdaptation(Size.Width);
            if (CLB_Models.SelectedItem.ToString() == "Model 1")
            {
                modelForm.Text = Model1TrafficLight.ModelName;
                modelForm.ModelNumber = 1;
            }
            else if (CLB_Models.SelectedItem.ToString() == "Model 2")
            {
                modelForm.Text = Model2TrafficLight.ModelName;
                modelForm.ModelNumber = 2;
            }
            else if (CLB_Models.SelectedItem.ToString() == "Model 3")
            {
                modelForm.Text = Model3TrafficLight.ModelName;
                modelForm.ModelNumber = 3;
            }
            modelForm.TrafficLightsInitialization();
            modelForm.Show();
        }
        /// <summary>
        /// Событие отметки "Отображение точек"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_ShowPoints_CheckedChanged(object sender, EventArgs e)
        {
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие отметки "Отображение координат"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_ShowPointsSpeed_CheckedChanged(object sender, EventArgs e)
        {
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие отметки "Отображение линий"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_ShowLines_CheckedChanged(object sender, EventArgs e)
        {
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие отметки "Отобразить путь авто"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_AutoWay_CheckedChanged(object sender, EventArgs e)
        {
            if (!CB_ShowAutoWay.Checked || TB_AutoNumber.Text.Length == 0)
                CB_ShowAutoWay.Checked = false;
            else
            {
                modelForm.PB_Model.Invalidate();
            }
        }
        /// <summary>
        /// Событие отметки "Отобразить поле зрения авто"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_AutoOverlook_CheckedChanged(object sender, EventArgs e)
        {
            modelForm.PB_Model.Invalidate();
        }
        /// <summary>
        /// Событие нажатия кнопки "Construct routes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_RoutesConstructor_Click(object sender, EventArgs e)
        {
            if (!IsRoutesConstructMode)
            {
                IsRoutesConstructMode = true;
                // Настройка элементов интерфейса
                modelForm.Focus();
                B_RoutesConstructor.BackColor = Color.Yellow;
                GB_ControlPanel.Enabled = false;
                GB_DisplayOptions.Enabled = false;
                CB_ShowRoutes.Checked = true;
                B_RoutesEditForm.Enabled = false;
                B_TrafficLightEditForm.Enabled = false;
                // Добавление временной точки указателя мыши
                Classes.Point.PointsList.Add(modelForm.MousePoint);
            }
            else
            {
                IsRoutesConstructMode = false;
                // Настройка элементов интерфейса
                B_RoutesConstructor.BackColor = DefaultBackColor;
                GB_ControlPanel.Enabled = true;
                GB_DisplayOptions.Enabled = true;
                CB_ShowRoutes.Checked = false;
                B_RoutesEditForm.Enabled = true;
                B_TrafficLightEditForm.Enabled = true;
                // Удаление временной точки указателя мыши
                Classes.Point.PointsList.RemoveAt(Classes.Point.PointsList.Count - 1);
                // Удаление временного маршрута
                Route.RoutesList.Remove(modelForm.Route_New);
                // Удаление временного светофора
                TrafficLight.TLsList.Remove(modelForm.TL_New);
            }
        }
        /// <summary>
        /// Нажатие на кнопку "Edit routes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_RoutesEditForm_Click(object sender, EventArgs e)
        {
            // Задаем владельца у формы редактора маршрутов
            routeEditForm = new RoutesEditForm
            {
                Owner = modelForm
            };
            // Делаем отображение точек, их скоростей и маршрутов
            CB_ShowRoutes.Checked = true;
            CB_ShowPointsSpeed.Checked = true;
            // Открывем окно редактора
            routeEditForm.ShowDialog();
        }
        /// <summary>
        /// Нажатие на кнопку "Edit traffic lights"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_TrafficLightEditForm_Click(object sender, EventArgs e)
        {
            // Задаем ребенка и запускаем форму
            trafficLightEditForm = new TrafficLightsEditForm
            {
                Owner = modelForm
            };
            trafficLightEditForm.ShowDialog();
        }
        /// <summary>
        /// Событие тика таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timerCounter % 2 == 0)
            {
                // Генерация автомобилей с каждым тиком, пока не иссякнут волны
                if (waves > 0)
                {
                    // Выбрана опция "Новая генерация авто"?
                    if (CB_NewGenerateAutos.Checked)
                    {
                        // Главный таймер только запущен?
                        if (timerCounter == 0)
                        {
                            Auto.GeneratedAutosList.Clear();
                            Auto.CountWaveAutosList.Clear();
                        }
                        Auto.GenerateAutos();
                    }
                    Auto.UseGeneratedAutos(Auto.AutosList, (int)timerCounter / 2);
                    waves--;
                }
            }
            timerCounter++;
            TimeSpan ts = TimeSpan.FromSeconds(timerCounter);
            L_TimerMove.Text = string.Format("{0} мин {1} сек", ts.Minutes, ts.Seconds); // Передаем значение времени с поправкой в MenuForm
        }
        /// <summary>
        /// Загрузка данных проекта
        /// </summary>
        /// <param name="project"></param>
        /// <returns>true если загрузка успешна, false если произошла ошибка</returns>
        public bool LoadProjectData(Project project)
        {
            if (project == null)
            {
                MessageBox.Show("Проект не выбран.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Назначение текущего проекта
            Project.Current = project;
            // Считывание изображения-схемы модели
            if (!string.IsNullOrEmpty(project.PicturePath) && System.IO.File.Exists(project.PicturePath))
            {
                try
                {
                    modelForm.PB_Model.Image = Image.FromFile(project.PicturePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show($"Файл изображения не найден:\n{project.PicturePath ?? "(путь не указан)"}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Проверка наличия директории проекта
            if (project.Directory == null || !project.Directory.Exists)
            {
                MessageBox.Show($"Директория проекта не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Считывание точек и маршрутов и заполнение путей
            try
            {
                Classes.Point.OpenFile(project.Directory.FullName + "\\points.txt");
                Route.OpenFile(project.Directory.FullName + "\\routes.txt");
                Way.FillWaysList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных маршрутов:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // !!! TODO: обнулить авто и таймеры !!!
            //B_Stop.PerformClick();
            //Auto.AutosList.Clear();
            //CB_NewGenerateAutos.Enabled = false;
            //CB_NewGenerateAutos.Checked = true;
            // Выбираем модель 1 в меню выбора модели (что вызывает считывание светофоров и смену алгоритма)
            TrafficLight.TLsList.Clear();
            CLB_Models.SetSelected(0, true);
            CLB_Models.SetItemChecked(0, true);
            // Задаем имя проекта в форме
            L_CurrentProjectName.Text = project.Name;
            // Настраиваем позицию текущей формы
            Location = new System.Drawing.Point(modelForm.Width, 0);
            
            return true;
        }
        /// <summary>
        /// Очистка данных проекта
        /// </summary>
        public void ClearProjectData()
        {
            Project.Current = null;
            modelForm.PB_Model.Image?.Dispose();
            modelForm.PB_Model.Image = null;
        }
    }
}