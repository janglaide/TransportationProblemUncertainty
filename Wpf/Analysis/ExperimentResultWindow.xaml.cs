using LiveCharts;
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
using System.Linq;
using LiveCharts.Wpf;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для ExperimentResultWindow.xaml
    /// </summary>
    public partial class ExperimentResultWindow : Window
    {
        private List<(int, double)> _list;
        public ExperimentResultWindow(List<(int, double)> list)
        {
            InitializeComponent();
            _list = list;
        }

        private void ExperimentResult_Loaded(object sender, RoutedEventArgs e)
        {
            var series = new SeriesCollection();
            var doubleValues = new ChartValues<double> ();
            var stringValues = new List<string> ();

            foreach(var x in _list)
            {
                stringValues.Add(x.Item1.ToString());
                doubleValues.Add(x.Item2);
            }

            chart.AxisX.Add(new Axis
            {
                Title = "Size of matrix",
                Labels = stringValues, 
                FontSize = 14
            });
            chart.AxisY.Add(new Axis
            {
                Title = "Percentage of change by changing optimum",
                FontSize = 14
            });
            var line = new LineSeries
            {
                Title = "Percentage",
                Values = doubleValues
            };
            series.Add(line);
            chart.Series = series;
        }
    }
}
