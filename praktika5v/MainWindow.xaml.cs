using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

/*
Построить график влажности в Питере за отчетные периоды: график 
должен строиться постоянно, однако, для грамотного отображения нужно 
использовать паузу. Должны выводиться текущие показания и средние (источник текущих показаний заменить ГСЧ).
*/
namespace praktika5v
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random _random = new Random();
        private List<double> _Humidity = new List<double>();
        private DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += UpdateGraph;
            _timer.Start();
        }
        private void UpdateGraph(object sender, EventArgs e)
        {
            double PercentageMoisture = _random.Next(30, 80); //ПроцентВлажности 
            _Humidity.Add(PercentageMoisture); //Записываем информацию о текущей влажности
            double averagaHumidity = _Humidity.Average(); //Средний процент влажности
            lvAveraga.Items.Add($"Средняя влажность(%): {averagaHumidity}");
            lvNow.Items.Add($"Текущая влажность(%): {PercentageMoisture}");
            Draw();
        }
        private void Draw()
        {
            Graphik.Children.Clear();
            double width = Graphik.ActualWidth;
            double height = Graphik.ActualHeight;   
            if (_Humidity.Count == 0) return;
            double maxHumidity = 80;
            double scale = height / maxHumidity;

            for (int i = 1; i < _Humidity.Count; i++)
            {
                double x1; // Додумать чему равно
                double y1;
                double x2;
                double y2;
                Line line = new Line
                {
                    //X1 = x1,
                    //Y1 = y1,
                    //X2 = x2,
                    //Y2 = y2,
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 2
                };
                Graphik.Children.Add(line);
            }
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (tbP.Text == "0")
            {
                _timer.Stop();
                tbP.Text = "1";
                btnPause.Content = "Возобновить";
            }
            else if (tbP.Text == "1")
            {
                _timer.Start();
                tbP.Text = "0";
                btnPause.Content = "Пауза";
            }
        }
    }
}