using ClassLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для SolverInputWindow.xaml
    /// </summary>
    public partial class SolverInputWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private string _filename;
        public SolverInputWindow()
        {
            InitializeComponent();
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SolverRandomInputWindow();
            window.Show();
            Close();
        }

        private void FromFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                if (openFileDialog.ShowDialog() != true)
                    throw new Exception("The file is not chosen");

                _filename = openFileDialog.FileName;
                Filename.Text = _filename.Split("\\").Last();
                RunFromFile.IsEnabled = true;
                ExceptionLabel.Foreground = Brushes.DarkGreen;
                ExceptionLabel.Content = "File chosen successfully";
            }
            catch(Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private void RunFromFile_Click(object sender, RoutedEventArgs e)
        {
            using StreamReader stream = new StreamReader(_filename);
            var N = Convert.ToInt32(stream.ReadLine());
            stream.ReadLine();
            var R = Convert.ToInt32(stream.ReadLine());
            stream.ReadLine();

            double[][] Cs = new double[R][];
            var size = N * N;
            for (var r = 0; r < R; r++)
            {
                Cs[r] = new double[size];
                string matrix = "";
                for (var c = 0; c < N; c++)
                {
                    matrix += stream.ReadLine();
                }
                var i = 0;
                foreach (var col in matrix.Trim().Split(' '))
                {
                    Cs[r][i] = double.Parse(col);
                    i++;
                }
                stream.ReadLine();
            }

            double[] a = new double[N];
            string text = stream.ReadLine();

            var j = 0;
            foreach (var col in text.Trim().Split())
            {
                a[j] = double.Parse(col);
                j++;
            }
            stream.ReadLine();

            double[] b = new double[N];
            text = stream.ReadLine();

            j = 0;
            foreach (var col in text.Trim().Split())
            {
                b[j] = double.Parse(col);
                j++;
            }
            stream.ReadLine();

            double[] l = new double[R];
            text = stream.ReadLine();

            j = 0;
            foreach (var col in text.Trim().Split())
            {
                l[j] = double.Parse(col);
                j++;
            }
            stream.ReadLine();

            double[] alpha = new double[R];
            text = stream.ReadLine();

            j = 0;
            foreach (var col in text.Trim().Split())
            {
                alpha[j] = double.Parse(col);
                j++;
            }
            var problem = new Problem(a, b, l, alpha, Cs, CChangeParameters.Default, 0.001);
            ExceptionLabel.Content = "";
            var solution = problem.Run();
            var window = new SolutionWindow(solution, problem);
            window.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
