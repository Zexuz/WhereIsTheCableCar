using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WhereIsTheCableCar;

namespace WpfCanvas
{
    public partial class MainWindow
    {
        private int _minX = 11892060;
        private int _maxX = 12004475;
        private int _minY = 57678144;
        private int _maxY = 57705963;

        private int deltaX = 112415;
        private int deltaY = 27819;


        public MainWindow()
        {
            InitializeComponent();
            SetWindowSize();
            DrawLines();
        }

        private void SetWindowSize()
        {
            Application.Current.MainWindow.Left = 30;
            Application.Current.MainWindow.Top = 30;
            Application.Current.MainWindow.Width = 1870;
            Application.Current.MainWindow.Height = 970;
            Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;

            var ib = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"C:\Users\Desktop\Pictures\image.png", UriKind.Relative))
            };
            MyCanvas.Background = ib;
        }

        private void MyCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var deltaLat = 60.11497326203 * e.GetPosition(MyCanvas).X;
            var realLat = Math.Round(deltaLat + _minX);


            var deltaLong = 28.6793814432 * e.GetPosition(MyCanvas).Y;
            var realLong = Math.Round(deltaLong + _minY);
        }

        public async void DrawLines()
        {
            while (true)
            {
                var res = await ApiHelper.GetLivemap();
                var cableCars = res.Vehicles.Where(v => v.GetType() == Vehicles.Tram).ToList();

                foreach (var cc in cableCars)
                {
                    var rect = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = new SolidColorBrush(Colors.Black),
                        Width = 2,
                        Height = 2
                    };

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = $"{cc.Name}";

                    var y = double.Parse(cc.Y);
                    var x = double.Parse(cc.X);

                    var yPos = (y - _minY) / deltaY * 970;
                    var xPos = (x - _minX) / deltaX * 1870;

                    Canvas.SetBottom(rect, yPos);
                    Canvas.SetLeft(rect, xPos);

                    Canvas.SetBottom(textBlock, yPos  +15);
                    Canvas.SetLeft(textBlock, xPos + 15);

                    Trace.WriteLine($"{yPos}:{xPos}");
                    MyCanvas.Children.Add(rect);
                    MyCanvas.Children.Add(textBlock);
                }

                await Task.Delay(500);
            }
        }
    }
}