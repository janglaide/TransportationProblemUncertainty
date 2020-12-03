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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts.Wpf;
using LiveCharts;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Loaded_MainWindow(object sender, RoutedEventArgs e)
        {
            var series = new SeriesCollection();
            var intValues = new ChartValues<int> { 1, 4, 9, 16, 25};
            var stringValues = new List<string> { "1", "2", "3", "4", "5"};

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
