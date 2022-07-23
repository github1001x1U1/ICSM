using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Controls
{
    /// <summary>
    /// Instrument.xaml 的交互逻辑
    /// </summary>
    public partial class Instrument : UserControl
    {
        public int ScaleTextSize
        {
            get { return (int)GetValue(ScaleTextSizeProperty); }
            set { SetValue(ScaleTextSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleTextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleTextSizeProperty =
            DependencyProperty.Register("ScaleTextSize", typeof(int), typeof(Instrument),
                new PropertyMetadata(default(int), new PropertyChangedCallback(OnPropertyChange)));


        public Brush PlateBackground
        {
            get { return (Brush)GetValue(PlateBackgroundProperty); }
            set { SetValue(PlateBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlateBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlateBackgroundProperty =
            DependencyProperty.Register("PlateBackground", typeof(Brush), typeof(Instrument), new PropertyMetadata(default(Brush)));


        // 依赖属性
        public int Value
        {
            get { return (int)GetValue(valueProperty); }
            set { SetValue(valueProperty, value); }
        }
        public static readonly DependencyProperty valueProperty
            = DependencyProperty.Register("Value", typeof(int), typeof(Instrument),
                new PropertyMetadata(default(int), new PropertyChangedCallback(OnPropertyChange)));



        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(Instrument), 
                new PropertyMetadata(default(int),new PropertyChangedCallback(OnPropertyChange)));



        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(Instrument),
                new PropertyMetadata(default(int), new PropertyChangedCallback(OnPropertyChange)));



        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(Instrument),
                new PropertyMetadata(default(int), new PropertyChangedCallback(OnPropertyChange)));

        public static void OnPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Instrument).Refresh();
        }
        // 描绘仪表盘刻度
        private void Refresh()
        {
            double radius = backEllipse.Width / 2;
            mainCanvas.Children.Clear();
            int min = Minimum, max = Maximum;
            int scaleCountArea = Interval;
            double step = 270.0 / (max - min);
            if (double.IsNaN(radius))
                return;
            for(int i = 0; i < max - min; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 13) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 13) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 2;
                mainCanvas.Children.Add(lineScale);
            }
            step = 270.0 / scaleCountArea;
            int scaleText = (int)min;
            for(int i = 0; i<= scaleCountArea; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 20) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 20) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 2;
                mainCanvas.Children.Add(lineScale);
                // 文本刻度值
                TextBlock textScale = new TextBlock();
                textScale.Width = 34;
                textScale.TextAlignment = TextAlignment.Center;
                textScale.FontSize = 14;
                textScale.Text = (scaleText + (max - min) / scaleCountArea * i).ToString();
                textScale.Foreground = Brushes.White;
                Canvas.SetLeft(textScale, radius - (radius - 36) * Math.Cos((i * step - 45) * Math.PI / 180) - 17);
                Canvas.SetTop(textScale, radius - (radius - 36) * Math.Sin((i * step - 45) * Math.PI / 180) - 10);
                mainCanvas.Children.Add(textScale);
            }
            string sData = "M{0} {1} A{0} {0} 0 1 1 {1} {2}";
            sData = string.Format(sData, radius / 2, radius, radius * 1.5);
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            circle.Data = converter.ConvertFrom(sData) as Geometry;

            step = 270.0 / (max - min);
            //rtPoint.Angle = Value * step - 45;
            // 指针动画
            //double value = double.IsNaN(Value) ? 0 : Value;
            DoubleAnimation da = new DoubleAnimation(Value * step - 45, new Duration(TimeSpan.FromMilliseconds(200)));
            rtPoint.BeginAnimation(RotateTransform.AngleProperty, da);

            sData = "M{0} {1},{1} {2},{1} {3}";
            sData = string.Format(sData, radius * 0.3, radius, radius - 5, radius + 5);
            point.Data = converter.ConvertFrom(sData) as Geometry;
        }

        public Instrument()
        {
            InitializeComponent();
            SizeChanged += Instrument_SizeChanged;
        }

        private void Instrument_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minSize = Math.Min(RenderSize.Width, RenderSize.Height);// 宽和高取小值
            backEllipse.Width = minSize;
            backEllipse.Height = minSize;
        }
    }
}
