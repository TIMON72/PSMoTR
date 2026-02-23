using PSMoTR.Forms;
using System;

namespace PSMoTR.Classes
{
    /// <summary>
    /// Часть, код в которой не следует изменять
    /// </summary>
    partial class Model1TrafficLight : TrafficLight
    {
        // Свойства
        static ModelForm ModelForm { get; set; } = null; // Форма модели
        // Конструкторы
        public Model1TrafficLight(TrafficLight tl) : base(tl.ToString()) { }
        /// <summary>
        /// Связывание с моделью
        /// </summary>
        public static void BindingWithModel(ModelForm modelForm)
        {
            ModelForm = modelForm; // Задаем модель
            // Конвертируем объекты TrafficLight в объекты Model1TrafficLight
            for (int i = 0; i < TLsList.Count; i++)
            {
                // В списке элемент класса TrafficLight? - приведение к Model1TrafficLight
                if (TLsList[i] is TrafficLight)
                    TLsList[i] = new Model1TrafficLight(TLsList[i]);
            }
            Initialization();
        }
    }
    /// <summary>
    /// Часть, код в которой можно только добавлять к уже существующим методам
    /// </summary>
    partial class Model1TrafficLight : TrafficLight
    {
        // Свойства
        public static string ModelName { get; private set; } = "Модель 1 - ССРС"; // Название для модели
        /// <summary>
        /// Инициализация свойств (перегрузка)
        /// </summary>
        public new static void Initialization()
        {
            TrafficLight.Initialization(); // Инициализация базовых свойств
            // TODO: здесь будет инициализация новых свойств или изменение старых (статические методы)
        }
        public override void PropertiesInitialization()
        {
            base.PropertiesInitialization(); // Инициализация базовых свойств
            // TODO: здесь будет инициализация новых свойств или изменение старых

        }
        /// <summary>
        /// Событие тика таймера (перегрузка)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void Timer_Tick(object sender, EventArgs e)
        {
            // TODO: здесь добавляются новые действия, связанные с таймером светофора

            // Вызов базового события
            base.Timer_Tick(sender, e);
        }
    }
    /// <summary>
    /// Часть класса, в которой реализуются новый алгоритм
    /// </summary>
    partial class Model1TrafficLight
    {
        // TODO: здесь пишется алгоритм работы светофора
    }
}
