using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace PSMoTR.Classes
{
    public class Project
    {
        // Свойства
        public string Name { get; set; }
        public DirectoryInfo Directory { get; set; }
        public string PicturePath { get; set; }
        public static Project Current { get; set; }
        // Списки
        public static List<Project> ProjectsList { get; private set; } = new List<Project>(); // Список проектов
        /// <summary>
        /// Выбор изображения фона модели
        /// </summary>
        public void SelectBackgroundPicture()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Файлы изображений (*.png, *.jpg, *.bmp)|*.png;*.jpg;*.bmp";
                // Файл выбран?
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileExtention = dialog.SafeFileName.Remove(0, dialog.SafeFileName.Length - 4); // Определяем расширение выбранного файла
                    File.Copy(dialog.FileName, Directory.FullName + "\\map" + fileExtention, true); // Копируем в директорию под именем map.???
                    PicturePath = Directory.FullName + "\\map" + fileExtention;
                }
            }
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        public Project(string name, DirectoryInfo directory)
        {
            Name = name;
            Directory = directory;
            // Директория определена?
            if (directory != null)
            {
                // Существует в директории файл map.png?
                if (File.Exists(directory.FullName + "\\map.png"))
                    PicturePath = directory.FullName + "\\map.png";
                // Существует в директории файл map.jpg?
                else if (File.Exists(directory.FullName + "\\map.jpg"))
                    PicturePath = directory.FullName + "\\map.jpg";
                // Существует в директории файл map.bmp?
                else if (File.Exists(directory.FullName + "\\map.bmp"))
                    PicturePath = directory.FullName + "\\map.bmp";
                // Иначе открываем диалог с выбором изображения для фона модели
                else
                    SelectBackgroundPicture();
            }
        }
        /// <summary>
        /// Загрузка и заполнение списка
        /// </summary>
        public static void Load()
        {
            if (ProjectsList.Count > 0)
                ProjectsList.Clear();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\Projects");
            // Директория не существует? - создать
            if (!di.Exists)
                di.Create();
            foreach (var dir in di.GetDirectories())
                ProjectsList.Add(new Project(dir.Name, dir));
            
            // Если нет проектов - создать проект по умолчанию из Resources
            if (ProjectsList.Count == 0)
            {
                try
                {
                    // Создаем директорию для проекта по умолчанию
                    DirectoryInfo defaultProjectDir = di.CreateSubdirectory("Default");
                    
                    // Копируем файлы из Resources
                    string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                    
                    if (System.IO.Directory.Exists(resourcesPath))
                    {
                        // Копируем map.png
                        string mapSource = Path.Combine(resourcesPath, "map.png");
                        if (File.Exists(mapSource))
                            File.Copy(mapSource, Path.Combine(defaultProjectDir.FullName, "map.png"), true);
                        
                        // Копируем points.txt
                        string pointsSource = Path.Combine(resourcesPath, "points.txt");
                        if (File.Exists(pointsSource))
                            File.Copy(pointsSource, Path.Combine(defaultProjectDir.FullName, "points.txt"), true);
                        
                        // Копируем routes.txt
                        string routesSource = Path.Combine(resourcesPath, "routes.txt");
                        if (File.Exists(routesSource))
                            File.Copy(routesSource, Path.Combine(defaultProjectDir.FullName, "routes.txt"), true);
                        
                        // Копируем traffic_lights.txt
                        string trafficLightsSource = Path.Combine(resourcesPath, "traffic_lights.txt");
                        if (File.Exists(trafficLightsSource))
                            File.Copy(trafficLightsSource, Path.Combine(defaultProjectDir.FullName, "traffic_lights.txt"), true);
                        
                        // Добавляем проект в список
                        ProjectsList.Add(new Project("Default", defaultProjectDir));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания проекта по умолчанию:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
