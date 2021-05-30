using ClassLibrary.ForWPF;
using ClassLibrary.Logic;
using ClassLibrary.Logic.Services;
using ClassLibrary.MethodParameters;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Wpf.PersistenceTest
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private string _filename;
        public InputWindow()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExceptionLabel.Foreground = Brushes.DarkRed;
                if (AccuracyComboBox.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelAccuracyNOTChosen);

                var random = new Random();
                var solver = new ClassLibrary.Logic.Solver();
                var accuracy = double.Parse(AccuracyComboBox.Text);
                var parametersForDefined = FileProcessing.ReadSolutionForPersistenceTest(_filename);

                if (parametersForDefined == null)
                    throw new Exception(Properties.Resources.ExceptionLabelIncorrectFileFormat);

                (double, double) cParameters = PercentFinder.GetCsRange(parametersForDefined);
                (double, double) abParameters = PercentFinder.GetABRange(parametersForDefined);
                (double, double) lParameters = PercentFinder.GetLRange(parametersForDefined);

                DistributionParametersService distributionParameters = new DistributionParametersService();
                var distributionParametersIds = distributionParameters.GetAppropriateIds(
                    cParameters, abParameters, lParameters);

                if (distributionParametersIds.Count < 5)
                    throw new Exception(Properties.Resources.ExceptionLabelNOTEnoughData);

                PercentageService percentageService = new PercentageService();
                var percentages = percentageService.GetAppropriate(parametersForDefined.A.Length, distributionParametersIds, parametersForDefined.Alpha.Length);

                if(percentages.Count < 5)
                    throw new Exception(Properties.Resources.ExceptionLabelNOTEnoughDataForParameters);

                var valueFromDB = percentages.Average();

                //var percent = PercentFinder.FindPercentOfChange(parametersForDefined, solver, random);
                var percent = PercentFinder.SearchMeanPercent(PercentFinder.FindPercentOfChange, parametersForDefined, accuracy, solver, random);

                ExceptionLabel.Content = "";
                var window = new Result(valueFromDB, percent, percentages);
                window.Show();
            }
            catch (Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }
        }
        
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                if (openFileDialog.ShowDialog() != true) throw new Exception(Properties.Resources.ExceptionLabelFileIsNOTChosen);
                

                _filename = openFileDialog.FileName;
                Filename.Text = _filename.Split("\\").Last();
                Run.IsEnabled = true;
                ExceptionLabel.Foreground = Brushes.DarkGreen;
                ExceptionLabel.Content = Properties.Resources.ExceptionLabelFileChosenSuccesslully;
            }
            catch (Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void SelectionChanged_Accuracy(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
