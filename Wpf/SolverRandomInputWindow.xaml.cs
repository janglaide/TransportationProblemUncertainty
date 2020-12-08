using ClassLibrary;
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

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для SolverRandomInputWindow.xaml
    /// </summary>
    public partial class SolverRandomInputWindow : Window
    {
        public SolverRandomInputWindow()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(SizeBox.Text, out int N)) throw new Exception("The 'N' is not natural number");
                if (N <= 0) throw new Exception("The 'N' must be positive");

                if (!int.TryParse(RBox.Text, out int R)) throw new Exception("The 'R' is not natural number");
                if (R <= 0) throw new Exception("The 'R' must be positive");
                if (DistributionComboBox.SelectedItem == null)
                    throw new Exception("The distribution is not chosen yet");

                double delay, deviation, a, b;

                var item = (ComboBoxItem)DistributionComboBox.SelectedItem;
                switch (item.Name)
                {
                    case "exp":
                        if (!double.TryParse(DelayMeanBox.Text, out delay)) throw new Exception("The delay must be a number");
                        if (delay <= 0) throw new Exception("The delay must be positive");
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        break;
                    case "norm":
                        if (!double.TryParse(DelayMeanBox.Text, out delay)) throw new Exception("The delay must be a number");
                        if (delay <= 0) throw new Exception("The delay must be positive");
                        if (!double.TryParse(DeviationBox.Text, out deviation)) throw new Exception("The deviation must be a number");
                        if (deviation <= 0) throw new Exception("The deviation must be positive");
                        a = -1.0;
                        b = -1.0;
                        break;
                    default:
                        if (!double.TryParse(DelayMeanBox.Text, out a) || !double.TryParse(DeviationBox.Text, out b)) 
                            throw new Exception("The range must consist of numbers");
                        if (b - a < 0) throw new Exception("The range must be positive");
                        if (a < 0) throw new Exception("The range must be of positive");
                        delay = -1.0;
                        deviation = -1.0;
                        break;
                }
                ExceptionLabel.Content = "";

                Problem problem;
                switch (item.Name)
                {
                    case "exp":
                        problem = new Problem(N, R, item.Name, delay);
                        break;
                    case "norm":
                        problem = new Problem(N, R, item.Name, delay, deviation);
                        break;
                    default:
                        problem = new Problem(N, R, item.Name, a, b);
                        break;
                }
                var optimalX = problem.Run();
                var window = new SolutionWindow(optimalX);
                window.Show();
            }
            catch (Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }
        }

        private void Distribution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = (ComboBoxItem)DistributionComboBox.SelectedItem;
            if (x.Name == "exp")
                DeviationBox.IsEnabled = false;
            else
                DeviationBox.IsEnabled = true;
            switch (x.Name)
            {
                case "exp":
                    ParametersLabel.Content = "Delay mean:";
                    DeviationBox.IsEnabled = false;
                    break;
                case "norm":
                    ParametersLabel.Content = "Delay mean and deviation:";
                    DeviationBox.IsEnabled = true;
                    break;
                default:
                    ParametersLabel.Content = "Range for generator:";
                    DeviationBox.IsEnabled = true;
                    break;
            }
        }
    }
}
