using PSMoTR.Classes;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace PSMoTR.Forms
{
    public partial class ProjectEditForm : Form
    {
        // Поля
        static int oldListCount = 0; // Прежнее количество объектов списка
        int choosenRow = -1; // Позиция выделенной строки таблицы
        static bool firstLoad = true; // Флаг первого запуска
        private MenuForm menuForm; // Форма 1-ой модели
        /// <summary>
        /// Конструктор
        /// </summary>
        public ProjectEditForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectEditForm_Load(object sender, EventArgs e)
        {
            menuForm = Owner as MenuForm; // Задаем родителя
            Project.Load(); // Загружаем проекты
            bindingSource.DataSource = Project.ProjectsList; // Создаем связь binding со списком
            bindingSource.AllowNew = true; // Разрешение на добавление нового элемента
            DGV_Projects.DataSource = bindingSource; // Указываем источником таблицы для отображения нашу связь binding
            oldListCount = bindingSource.Count;
            // Даем названия заголовкам колонок
            DGV_Projects.RowHeadersWidth = 50; // Ширина системной колонки
            DGV_Projects.Columns["Name"].DisplayIndex = 0;
            DGV_Projects.Columns["Name"].Width = 180;
            DGV_Projects.Columns["Name"].HeaderText = "Name";
            DGV_Projects.Columns["Directory"].Visible = false;
            DGV_Projects.Columns["PicturePath"].Visible = false;
        }
        /// <summary>
        /// Событие после закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (firstLoad)
                Environment.Exit(0);
            firstLoad = false;
        }
        /// <summary>
        /// Событие перед добавлением последней строки в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_AddingNew(object sender, AddingNewEventArgs e)
        {
            string newProjectName = "project" + Project.ProjectsList.Count;
            Project newProject = new Project(newProjectName, null);
            e.NewObject = newProject;
        }
        /// <summary>
        /// Событие при изменении данных списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BS_ListChanged(object sender, ListChangedEventArgs e)
        {
            DGV_Projects.Invalidate();
        }
        /// <summary>
        /// Событие выделения строки/ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_Projects_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            choosenRow = e.RowIndex;
        }
        /// <summary>
        /// Событие нажатия на клавишу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_Projects_KeyDown(object sender, KeyEventArgs e)
        {
            Project selectedProject = Project.ProjectsList[choosenRow];
            // Нажата клавиша Enter и выделена строка?
            if (e.KeyCode == Keys.Enter && choosenRow >= 0)
            {
                // В таблице добавляется новый проект?
                if (Project.ProjectsList.Count != oldListCount && oldListCount != 0)
                {
                    string newProjectPath = Environment.CurrentDirectory + "\\Projects\\" + Project.ProjectsList[choosenRow].Name;
                    // Есть такая папка?
                    if (Directory.Exists(newProjectPath))
                    {
                        MessageBox.Show("Проект с таким названием уже существует");
                        return;
                    }
                    selectedProject.Directory = Directory.CreateDirectory(newProjectPath);
                    selectedProject.SelectBackgroundPicture();
                    if (selectedProject.PicturePath is null)
                    {
                        MessageBox.Show("Вы не выбрали фон модели");
                        return;
                    }
                    oldListCount++;
                }
                // Загрузка данных проекта
                if (menuForm.LoadProjectData(selectedProject))
                {
                    // Закрываем форму только если загрузка успешна
                    firstLoad = false;
                    Close();
                }
            }
            // Если нажата клавиша Delete и выделена какая-либо строка
            if (e.KeyCode == Keys.Delete && choosenRow >= 0)
            {
                // Удаляется текущий проект?
                if (Project.Current != null && selectedProject.Name == Project.Current.Name)
                {
                    menuForm.ClearProjectData();
                    if (selectedProject.Directory != null && selectedProject.Directory.Exists)
                        Directory.Delete(selectedProject.Directory.FullName, true);
                    Application.Restart();
                }
                else
                {
                    // Папка существует?
                    if (selectedProject.Directory != null && selectedProject.Directory.Exists)
                        // Удаляем папку
                        Directory.Delete(selectedProject.Directory.FullName, true);
                    bindingSource.Remove(selectedProject); // Удаление проекта из списка через binding (в том числе и из таблицы)
                    oldListCount--;
                }
            }
        }
        /// <summary>
        /// Событие при окончании редактирования содержимого ячейки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGV_Projects_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string columnName = DGV_Projects.Columns[e.ColumnIndex].Name; // Имя выбранной колонки
            Project parsingProject = Project.ProjectsList[e.RowIndex]; // Выбранный объект
            if (columnName == "Name")
            {
                string parsingProjectPath = Environment.CurrentDirectory + "\\Projects\\" + e.Value;
                // Есть уже такая папка?
                if (Directory.Exists(parsingProjectPath))
                {
                    MessageBox.Show("Проект с таким названием уже существует");
                    e.Value = parsingProject.Name;
                    e.ParsingApplied = true;
                    return;
                }
                // Папка существует?
                if (parsingProject.Directory is null)
                    // Создаем папку
                    Directory.CreateDirectory(parsingProjectPath);
                else
                    // Переименуем папку
                    Directory.Move(parsingProject.Directory.FullName, parsingProjectPath);

                // Обновляем объект изменияемого проекта
                parsingProject.Name = e.Value.ToString();
                parsingProject.Directory = new DirectoryInfo(parsingProjectPath);
                // Валидация
                e.ParsingApplied = true;
            }
        }
    }
}