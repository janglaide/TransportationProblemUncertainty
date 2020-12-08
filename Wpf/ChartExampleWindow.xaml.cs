using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для ChartExampleWindow.xaml
    /// </summary>
    public partial class ChartExampleWindow : Window
    {
        public ChartExampleWindow()
        {
            InitializeComponent();
        }

        private void Loaded_Window(object sender, RoutedEventArgs e)
        {
            var series = new SeriesCollection();
            var intValues = new ChartValues<int> { 1, 4, 9, 16, 25 };
            var stringValues = new List<string> { "1", "2", "3", "4", "5" };

            chart.AxisX.Add(new Axis
            {
                Title = "x",
                Labels = stringValues
            });

            var line = new LineSeries
            {
                Title = "y",
                Values = intValues
            };
            series.Add(line);

            chart.Series = series;
        }
    }
}
