using LiveCharts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using LiveCharts.Wpf;
using Microsoft.Win32;
using ClassLibrary.ForWPF;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;

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

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() != true)
                    throw new Exception("File save dialog does not open");

                FileProcessing.WriteExperimentDataIntoFile(_list, saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SaveDataButton.Margin = new Thickness(84, Height - 40, 0, 0);
            SaveGraphButton.Margin = new Thickness(750, Height - 40, 0, 0);
        }

        private void SaveGraphButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*SaveFileDialog saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() != true)
                    throw new Exception("File save dialog does not open");

                */

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
