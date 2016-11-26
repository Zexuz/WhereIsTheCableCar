using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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



        public MainWindow()
        {
            InitializeComponent();
            DrawLines();
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
                        Stroke = new SolidColorBrush(Colors.Black), Fill = new SolidColorBrush(Colors.Black), Width = 10, Height = 10
                    };
                    Console.WriteLine($"Nr {cc.Name}, dir {cc.Direction}");

                    var y = (_maxY - double.Parse(cc.Y)) / 100;
                    var x = (_maxX - double.Parse(cc.X)) / 100;

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = $"{cc.Y}, {cc.X}";

                    Canvas.SetLeft(textBlock, x + 10);
                    Canvas.SetTop(textBlock, y + 10);
                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(rect, x);

                    PaintSurface.Children.Add(textBlock);
                    PaintSurface.Children.Add(rect);
                }

                await Task.Delay(2000 * 10);
            }
        }
    }
}