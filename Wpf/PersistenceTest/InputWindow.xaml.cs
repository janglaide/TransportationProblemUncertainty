using ClassLibrary.ForWPF;
using ClassLibrary.Logic;
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
                if (AccuracyComboBox.SelectedItem == null) throw new Exception("Choose an accuracy");

                var random = new Random();
                var solver = new ClassLibrary.Logic.Solver();
                var accuracy = double.Parse(AccuracyComboBox.Text);
                var parametersForDefined = FileProcessing.ReadSolutionForPersistenceTest(_filename);

                (double, double) cParameters = GetCsRange(parametersForDefined);
                (double, double) abParameters = GetABRange(parametersForDefined);
                (double, double) lParameters = GetLRange(parametersForDefined);

                var percent = PercentFinder.FindPercentOfChange(parametersForDefined, solver, random);
            }
            catch(Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }
        }
        private (double, double) GetCsRange(ParametersForDefined parameters)
        {
            double averMin = 0.0, averMax = 0.0;
            foreach(var c in parameters.Cs)
            {
                var min = double.MaxValue;
                var max = double.MinValue;
                foreach(var x in c)
                {
                    if (x < min)
                        min = x;

                    if (x > max)
                        max = x;
                }
                averMin += min;
                averMax += max;
            }
            return ((averMin / parameters.Cs.Length), (averMax / parameters.Cs.Length));
        }
        private (double, double) GetABRange(ParametersForDefined parameters)
        {
            double minA = double.MaxValue, maxA = double.MinValue, minB = double.MaxValue, maxB = double.MinValue;
            
            for(var i = 0; i < parameters.A.Length; i++)
            {
                if (parameters.A[i] < minA)
                    minA = parameters.A[i];

                if (parameters.A[i] > maxA)
                    maxA = parameters.A[i];

                if (parameters.B[i] < minB)
                    minB = parameters.B[i];

                if (parameters.B[i] > maxB)
                    maxB = parameters.B[i];
            }

            return (((minA + minB) / 2), ((maxA + maxB) / 2));
        }

        private (double, double) GetLRange(ParametersForDefined parameters)
        {
            double min = double.MaxValue, max = double.MinValue;

            foreach (var l in parameters.L)
            {
                if (l < min)
                    min = l;

                if (l > max)
                    max = l;
            }

            return (min, max);
        }
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                if (openFileDialog.ShowDialog() != true) throw new Exception("The file is not chosen");
                

                _filename = openFileDialog.FileName;
                Filename.Text = _filename.Split("\\").Last();
                Run.IsEnabled = true;
                ExceptionLabel.Foreground = Brushes.DarkGreen;
                ExceptionLabel.Content = "File chosen successfully";
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
