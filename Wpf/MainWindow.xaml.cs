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
    }
}
