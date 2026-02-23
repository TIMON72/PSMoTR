using PSMoTR.Classes;
using PSMoTR.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace PSMoTR.Windows
{
    /// <summary>
    /// Логика взаимодействия для ModelWindow.xaml
    /// </summary>
    public partial class ModelWindow : UserControl
    {
        
        public ModelForm ModelForm { get; set; }
        List<Path> AutosList_Canvas { get; set; } = new List<Path>();
        
        public ModelWindow()
        {
            InitializeComponent();
            
            RenderSize = new Size(1700, 834);
        }

        private void Canvas_Model_Loaded(object sender, RoutedEventArgs e)
        {
            //RenderSize = new Size(ModelForm.Size.Width, ModelForm.Size.Height);
        }

        internal void TimerMove_Tick(List<Auto> autosList, float zoom)
        {
            if (autosList.Count > 0 && AutosList_Canvas.Count == 0)
            {

                Canvas_Model.Children.Clear();

                RectangleGeometry car_bodyRectangle = new RectangleGeometry(new Rect((autosList[0].X - autosList[0].Length / 2) * zoom, (autosList[0].Y - autosList[0].Width / 2) * zoom, 
                                                                                autosList[0].Length * zoom, autosList[0].Width * zoom));
                EllipseGeometry car_centerPoint = new EllipseGeometry(new Point(autosList[0].X * zoom, autosList[0].Y * zoom), 0.2 * zoom, 0.2 * zoom);
                EllipseGeometry car_frontPoint = new EllipseGeometry(new Point((autosList[0].X - autosList[0].Length / 2) * zoom, autosList[0].Y * zoom), 0.2 * zoom, 0.2 * zoom);
                GeometryGroup car = new GeometryGroup();
                car.Children.Add(car_bodyRectangle);
                car.Children.Add(car_centerPoint);
                car.Children.Add(car_frontPoint);
                Path carPath = new Path
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.2 * zoom,
                    Data = car
                };
                
                TransformGroup transform = new TransformGroup();

                RotateTransform rotate = new RotateTransform(autosList[0].Rotation, autosList[0].X * zoom, autosList[0].Y * zoom);
                //RotateTransform rotate = new RotateTransform(autosList[0].Rotation);
                //transform.Children.Add(rotate);

                //TranslateTransform move = new TranslateTransform();
                //move.X = autosList[0].X * zoom;
                //move.Y = autosList[0].Y * zoom;
                //transform.Children.Add(move);

                carPath.RenderTransform = transform;
                

                Canvas_Model.Children.Add(carPath);

                Canvas.SetLeft(Canvas_Model.Children[0], autosList[0].X);
                Canvas.SetTop(Canvas_Model.Children[0], autosList[0].Y);

                //AutosList_Canvas.Add(carPath);

            }

            //if (autosList.Count > 0)
            //    if (autosList[0] != null)
            //    {
            //        TransformGroup transform = new TransformGroup();
                    
            //        RotateTransform rotate = new RotateTransform(autosList[0].Rotation, 50 * zoom, 50 * zoom);
            //        //RotateTransform rotate = new RotateTransform(autosList[0].Rotation);
            //        transform.Children.Add(rotate);

            //        TranslateTransform move = new TranslateTransform();
            //        //AutosList_Canvas[0].
            //        //move.X = autosList[0].X * zoom;
            //        //move.Y = autosList[0].Y * zoom;
            //        transform.Children.Add(move);

            //        AutosList_Canvas[0].RenderTransform = transform;
            //    }



            //// Рисование авто, если еще нет ее фигуры 
            //if (ModelForm.AutosList.Count > M_AutosList.Count)
            //{
            //    int start = M_AutosList.Count;
            //    int end = ModelForm.AutosList.Count;
            //    for (int i = start; i < end; i++)
            //    {
            //        Rectangle rect = new Rectangle()
            //        {
            //            Height = ModelForm.AutosList[i].Width * ModelForm.Zoom,
            //            Width = ModelForm.AutosList[i].Length * ModelForm.Zoom,
            //            Margin = new Thickness((ModelForm.AutosList[i].X - ModelForm.AutosList[i].Length / 2) * ModelForm.Zoom, 
            //                                    (ModelForm.AutosList[i].Y - ModelForm.AutosList[i].Width / 2) * ModelForm.Zoom, 0, 0),
            //            Fill = Brushes.Gray
            //        };
            //        M_AutosList.Add(rect);
            //        Canvas_Model.Children.Add(rect);
            //        Ellipse ell = new Ellipse()
            //        {
            //            Height = 0.5 * ModelForm.Zoom,
            //            Width = 0.5 * ModelForm.Zoom,
            //            Margin = new Thickness((ModelForm.AutosList[i].X - 0.25) * ModelForm.Zoom,
            //                                    (ModelForm.AutosList[i].Y - 0.25) * ModelForm.Zoom, 0, 0),
            //            Fill = Brushes.Blue
            //        };
            //        Canvas_Model.Children.Add(ell);
            //    }
            //}
            //// 
            //for (int i = 0; i < M_AutosList.Count; i++)
            //{
            //    //[i].RenderTransform = new TranslateTransform(ModelForm.AutosList[i].X * ModelForm.Zoom, ModelForm.AutosList[i].Y * ModelForm.Zoom);
            //}


        }
    }
}
