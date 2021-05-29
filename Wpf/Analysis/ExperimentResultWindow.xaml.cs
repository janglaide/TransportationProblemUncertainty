using LiveCharts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LiveCharts.Wpf;
using Microsoft.Win32;
using ClassLibrary.ForWPF;
using System.IO;
using System.Windows.Controls;

namespace Wpf.Analysis
{
    /// <summary>
    /// Логика взаимодействия для ExperimentResultWindow.xaml
    /// </summary>
    public partial class ExperimentResultWindow : Window
    {
        private readonly List<List<(int, double)>> _list;
        private readonly List<string> _messages;
        public ExperimentResultWindow(List<List<(int, double)>> list, List<string> messages)
        {
            InitializeComponent();
            _list = list;
            _messages = messages;
        }

        private void ExperimentResult_Loaded(object sender, RoutedEventArgs e)
        {
            var series = new SeriesCollection();
            static string formatFunc(double x) => string.Format("{0:0.000}", x);
            
            var stringValues = new List<string> ();
            foreach (var x in _list[0])
            {
                stringValues.Add(x.Item1.ToString());
            }
            chart.AxisX.Add(new Axis
            {
                Title = Properties.Resources.GraphSizeOfMatrixX,
                Labels = stringValues,
                FontSize = 14
            });
            chart.AxisY.Add(new Axis
            {
                Title = Properties.Resources.GraphPercentageY,
                FontSize = 14,
                LabelFormatter = formatFunc
            });

            for (int i = 0; i < _list.Count; i++)
            {
                var doubleValues = new ChartValues<double>();
                foreach (var x in _list[i])
                {
                    doubleValues.Add(x.Item2);
                }
                var line = new LineSeries
                {
                    Title = $"{_messages[i]}: {Properties.Resources.GraphPercentageLine}",
                    Values = doubleValues
                };

                series.Add(line);
            }
            chart.Series = series;
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = $"experiment-results_{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileProcessing.WriteExperimentDataIntoFile(_list, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SaveDataButton.Margin = new Thickness(84, Height - 80, 0, 0);
            SaveGraphButton.Margin = new Thickness(750, Height - 80, 0, 0);
        }

        private void SaveGraphButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = $"graph_{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.png"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var ParentPanelCollection = (chart.Parent as Panel).Children;
                    ParentPanelCollection.Remove(chart);
                    if (chart != null)
                    {
                        TakeTheChart(saveFileDialog.FileName);
                        ParentPanelCollection.Add(chart);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void TakeTheChart(string filename)
        {
            var viewbox = new Viewbox
            {
                Child = chart
            };
            viewbox.Measure(chart.RenderSize);
            viewbox.Arrange(new Rect(new Point(0, 0), chart.RenderSize));
            viewbox.UpdateLayout();

            SaveToPng(chart, filename);

            viewbox.Child = null;
        }
        private void SaveToPng(FrameworkElement visual, string filename)
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(visual, filename, encoder);
        }

        private static void EncodeVisual(FrameworkElement visual, string filename, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using var stream = File.Create(filename); 
            encoder.Save(stream);
        }
    }
}
