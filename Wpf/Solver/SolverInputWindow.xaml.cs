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
using ClassLibrary.ForWPF;

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
            var problem = FileProcessing.ReadProblemFromFile(_filename);
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
