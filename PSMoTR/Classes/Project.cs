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
        }
    }
}
