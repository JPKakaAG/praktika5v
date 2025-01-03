﻿using System.Security.Cryptography.Xml;
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
            tbP.Text = "0";
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
                double x1 = (width / (_Humidity.Count - 1)) * (i - 1);
                double y1 = height - (_Humidity[i - 1] * scale);
                double x2 = (width / (_Humidity.Count - 1)) * i;
                double y2 = height - (_Humidity[i] * scale);
                Line line = new Line
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2,
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 2
                };
                Graphik.Children.Add(line);

                AddHumidityLabel(x2, y2, _Humidity[i]);
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
        private void AddHumidityLabel(double x, double y, double humidity)
        {
            TextBlock label = new TextBlock
            {
                Text = $"{humidity:F1}%", // Форматируем текст до одного знака после запятой
                Foreground = Brushes.Black,
                FontSize = 12,
                Margin = new Thickness(x + 5, y - 10, 0, 0) // Сдвигаем метку немного вправо и вверх
            };
            Graphik.Children.Add(label);
        }
    }
}