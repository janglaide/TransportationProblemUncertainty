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
    /// Логика взаимодействия для AnalysisInputWindow.xaml
    /// </summary>
    public partial class AnalysisInputWindow : Window
    {
        public AnalysisInputWindow()
        {
            InitializeComponent();
        }

        private void RunButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AccuracyComboBox.SelectedItem == null) throw new Exception("Choose an accuracy");
                var accuracy = double.Parse(AccuracyComboBox.Text);
                if (!int.TryParse(StartSizeBox.Text, out int startSize) ||
                    !int.TryParse(FinalSizeBox.Text, out int finalSize)) throw new Exception("The range must consist of integers");
                if (finalSize - startSize <= 0) throw new Exception("Invalid range of sizing");
                if (startSize <= 0) throw new Exception("Sizes of matrixes must be positive");
                if (!int.TryParse(StepBox.Text, out int step)) throw new Exception("The step must be integer");
                if (step <= 0) throw new Exception("The step must be positive");
                if (finalSize - startSize < step) throw new Exception("Incorrect step for the sizing range");
                if (!int.TryParse(RBox.Text, out int R)) throw new Exception("The 'R' must be integer");
                if (R <= 0) throw new Exception("The 'R' must be positive");
                if (DistributionComboBox.SelectedItem == null) throw new Exception("The distribution is not chosen yet");

                double delay, deviation, a, b;
                Experiment experiment;

                var item = (ComboBoxItem)DistributionComboBox.SelectedItem;
                switch (item.Name)
                {
                    case "exp":
                        if (!double.TryParse(DelayBox.Text, out delay)) throw new Exception("The delay must be numeric");
                        if (delay <= 0) throw new Exception("The delay must be positive");
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        experiment = new Experiment(item.Name, (delay, delay), (delay, delay), (delay, delay));
                        break;
                    case "norm":
                        if (!double.TryParse(DelayBox.Text, out delay)) throw new Exception("The delay must be numeric");
                        if (delay <= 0) throw new Exception("The delay must be positive");
                        if (!double.TryParse(DeviationBox.Text, out deviation)) throw new Exception("The deviation must be numeric");
                        if (deviation <= 0) throw new Exception("The deviation must be positive");
                        a = -1.0;
                        b = -1.0;
                        experiment = new Experiment(item.Name, (delay, deviation), (delay, deviation), (delay, deviation));
                        break;
                    default:
                        if (!double.TryParse(DelayBox.Text, out a) || !double.TryParse(DeviationBox.Text, out b))
                            throw new Exception("The range must consist of numbers");
                        if (b - a < 0) throw new Exception("The range must be positive");
                        if (a < 0) throw new Exception("The range must be of positive");
                        delay = -1.0;
                        deviation = -1.0;
                        experiment = new Experiment(item.Name, (a, b), (a, b), (a, b));
                        break;
                }
                ExceptionLabel.Content = "";

                var results = experiment.RunExperiment(startSize, finalSize, step, R, accuracy);

                var window = new ExperimentResultWindow(results);
                window.Show();
            }
            catch(Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }
        }

        private void Accuracy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
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
