using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Wpf.Analysis;
using Wpf.Solver;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public MainWindow()
        {     
            InitializeComponent();
        }

        private void Loaded_MainWindow(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void SolverButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SolverInputWindow();
            window.Show();
            Close();
        }

        private void AnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AnalysisInputWindow();
            window.Show();
            Close();
        }

        private void PersistenceButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new PersistenceTest.InputWindow();
            window.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
