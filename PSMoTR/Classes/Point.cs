using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace PSMoTR.Classes
{
    public class Point
    {
        // Свойства
        public float X { get; set; }
        public float Y { get; set; }
        public float Speed { get; set; }
        public int Number
        {
            get
            {
                return PointsList.IndexOf(this);
            }
        } // Номер в списке
        // Списки
        public static List<Point> PointsList { get; private set; } = new List<Point>();
        /// <summary>
        /// Конструкторы
        /// </summary>
        public Point() { }
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Point(float x, float y, float speed)
        {
            X = x;
            Y = y;
            Speed = speed;
        }
        public Point(string str)
        {
            str = str.Replace("X=", "");
            str = str.Replace("Y=", "");
            str = str.Replace("Speed=", "");
            str = str.Replace(" ", "");
            string[] buff = str.Split(';');
            X = Convert.ToSingle(buff[0], null);
            Y = Convert.ToSingle(buff[1], null);
            Speed = Convert.ToSingle(buff[2], null);
        }
        /// <summary>
        /// Перевод в string (перегрузка)
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "X=" + X + "; " + "Y=" + Y + "; " + "Speed=" + Speed;
        /// <summary>
        /// Сравнение двух объектов на равенство (перегрузка)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Point p = obj as Point;
            if (p as Point == null)
                return false;
            return p.X == X && p.Y == Y;
        }
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
        /// <summary>
        /// Клонирование объекта
        /// </summary>
        /// <returns></returns>
        public Point Clone()
        {
            Point clone = new Point
            {
                X = X,
                Y = Y,
                Speed = Speed
            };
            return clone;
        }
        /// <summary>
        /// Сортировка
        /// </summary>
        public static void Sort() => PointsList = PointsList.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
        /// <summary>
        /// Открытие файла
        /// </summary>
        public static bool OpenFile(string path) // Открытие через путь к файлу
        {
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                PointsList.Clear();
                string[] str;
                Point point;
                try
                {
                    if (file.Length == 0) // Если файл пустой, то загружаем в него содержимое из шаблона в ресурсах
                    {
                        string[] reader2 = Properties.Resources.points.Replace("\r", "").Split('\n');
                        for (int i = 0; i < reader2.Length; i++)
                        {
                            str = reader2[i].Split(' ');
                            point = new Point(Convert.ToSingle(str[0]), Convert.ToSingle(str[1]), Convert.ToSingle(str[2]));
                            PointsList.Add(point);
                        }
                    }
                    else // Иначе считываем файл
                    {
                        StreamReader reader = new StreamReader(file);
                        while (!reader.EndOfStream)
                        {
                            str = reader.ReadLine().Split(' ');
                            point = new Point(Convert.ToSingle(str[0]), Convert.ToSingle(str[1]), Convert.ToSingle(str[2]));
                            PointsList.Add(point);
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
        public static void RewriteFile(string path) // Запись через путь к файлу
        {
            Sort();
            // Открытие диалогового окна для выбора перезаписываемого файла
            using (StreamWriter writer = new StreamWriter(path, false)) // false - перезапись
            {
                try
                {
                    foreach (Point p in PointsList)
                        writer.WriteLine(p.X.ToString() + " " + p.Y.ToString() + " " + p.Speed.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        /// <summary>
        /// Расстояние между точками
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(Point a, Point b)
        {
            return (float)Math.Pow(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2), 0.5);
        }
    }
}