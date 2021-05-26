using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using ClassLibrary.Enums;
using ClassLibrary.ForWPF;
using ClassLibrary.Generators;

namespace Wpf.Solver
{
    /// <summary>
    /// Логика взаимодействия для SolverRandomInputWindow.xaml
    /// </summary>
    public partial class SolverRandomInputWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public SolverRandomInputWindow()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(SizeBox.Text, out int N)) throw new Exception(Properties.Resources.ExceptionLabelIntegerN);
                if (N <= 0) throw new Exception(Properties.Resources.ExceptionLabelPositiveN);

                if (!int.TryParse(RBox.Text, out int R)) throw new Exception(Properties.Resources.ExceptionLabelIntegerR);
                if (R <= 0) throw new Exception(Properties.Resources.ExceptionLabelPositiveR);
                if (DistributionComboBox.SelectedItem == null)
                    throw new Exception(Properties.Resources.ExceptionLabelDistributionCNOTChosen);
                if (DistributionComboBoxAB.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelDistributionABNOTChosen);
                if (DistributionComboBoxL.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelDistributionLNOTChosen);


                double delay, deviation, a, b;

                (double, double) paramsC;
                (double, double) paramsAB;
                (double, double) paramsL;
                (string, string, string) distribution;

                var item = (ComboBoxItem)DistributionComboBox.SelectedItem;
                switch (item.Name)
                {
                    case "exp":
                        if (!double.TryParse(DelayMeanBox.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        distribution.Item1 = "Exponential";
                        paramsC = (delay, delay);
                        break;
                    case "norm":
                        if (!double.TryParse(DelayMeanBox.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        if (!double.TryParse(DeviationBox.Text, out deviation)) throw new Exception(Properties.Resources.ExceptionLabelDeviationNumeric);
                        if (deviation <= 0) throw new Exception(Properties.Resources.ExceptionLabelDeviationPositive);
                        a = -1.0;
                        b = -1.0;
                        distribution.Item1 = "Normal";
                        paramsC = (delay, deviation);
                        break;
                    default:
                        if (!double.TryParse(DelayMeanBox.Text, out a) || !double.TryParse(DeviationBox.Text, out b)) 
                            throw new Exception(Properties.Resources.ExceptionLabelRangeNumbers);
                        if (b - a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangePositive);
                        if (a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangeStartsInPositive);
                        delay = -1.0;
                        deviation = -1.0;
                        distribution.Item1 = "Uniform";
                        paramsC = (a, b);
                        break;
                }

                var itemAB = (ComboBoxItem)DistributionComboBoxAB.SelectedItem;
                switch (itemAB.Name)
                {
                    case "exp1":
                        if (!double.TryParse(DelayMeanBoxAB.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        distribution.Item2 = "Exponential";
                        paramsAB = (delay, delay);
                        break;
                    case "norm1":
                        if (!double.TryParse(DelayMeanBoxAB.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        if (!double.TryParse(DeviationBoxAB.Text, out deviation)) throw new Exception(Properties.Resources.ExceptionLabelDeviationNumeric);
                        if (deviation <= 0) throw new Exception(Properties.Resources.ExceptionLabelDeviationPositive);
                        a = -1.0;
                        b = -1.0;
                        distribution.Item2 = "Normal";
                        paramsAB = (delay, deviation);
                        break;
                    default:
                        if (!double.TryParse(DelayMeanBoxAB.Text, out a) || !double.TryParse(DeviationBoxAB.Text, out b))
                            throw new Exception(Properties.Resources.ExceptionLabelRangeNumbers);
                        if (b - a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangePositive);
                        if (a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangeStartsInPositive);
                        delay = -1.0;
                        deviation = -1.0;
                        distribution.Item2= "Uniform";
                        paramsAB = (a, b);
                        break;
                }

                var itemL = (ComboBoxItem)DistributionComboBoxL.SelectedItem;
                switch (itemL.Name)
                {
                    case "exp2":
                        if (!double.TryParse(DelayMeanBoxL.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        distribution.Item3 = "Exponential";
                        paramsL = (delay, delay);
                        break;
                    case "norm2":
                        if (!double.TryParse(DelayMeanBoxL.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        if (!double.TryParse(DeviationBoxL.Text, out deviation)) throw new Exception(Properties.Resources.ExceptionLabelDeviationNumeric);
                        if (deviation <= 0) throw new Exception(Properties.Resources.ExceptionLabelDeviationPositive);
                        a = -1.0;
                        b = -1.0;
                        distribution.Item3 = "Normal";
                        paramsL = (delay, deviation);
                        break;
                    default:
                        if (!double.TryParse(DelayMeanBoxL.Text, out a) || !double.TryParse(DeviationBoxL.Text, out b))
                            throw new Exception(Properties.Resources.ExceptionLabelRangeNumbers);
                        if (b - a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangePositive);
                        if (a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangeStartsInPositive);
                        delay = -1.0;
                        deviation = -1.0;
                        distribution.Item3 = "Uniform";
                        paramsL = (a, b);
                        break;
                }

                ExceptionLabel.Content = "";
                var generator = new GeneratorTaskCondition(distribution, paramsC, paramsAB, paramsL);
                Problem problem = new Problem(N, R, generator, CChangeParameters.Default);
                var solution = problem.Run();
                var window = new SolutionWindow(solution, problem);
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
                    ParametersLabel.Content = Properties.Resources.ParametersExpLabel;
                    DeviationBox.IsEnabled = false;
                    break;
                case "norm":
                    ParametersLabel.Content = Properties.Resources.ParametersNormalLabel;
                    DeviationBox.IsEnabled = true;
                    break;
                default:
                    ParametersLabel.Content = Properties.Resources.ParametersUniformLabel;
                    DeviationBox.IsEnabled = true;
                    break;
            }
        }

        private void DistributionAB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = (ComboBoxItem)DistributionComboBoxAB.SelectedItem;
            if (x.Name == "exp1")
                DeviationBoxAB.IsEnabled = false;
            else
                DeviationBoxAB.IsEnabled = true;
            switch (x.Name)
            {
                case "exp1":
                    ParametersABLabel.Content = Properties.Resources.ParametersExpLabel;
                    DeviationBoxAB.IsEnabled = false;
                    break;
                case "norm1":
                    ParametersABLabel.Content = Properties.Resources.ParametersNormalLabel;
                    DeviationBoxAB.IsEnabled = true;
                    break;
                default:
                    ParametersABLabel.Content = Properties.Resources.ParametersUniformLabel;
                    DeviationBoxAB.IsEnabled = true;
                    break;
            }
        }

        private void DistributionL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = (ComboBoxItem)DistributionComboBoxL.SelectedItem;
            if (x.Name == "exp2")
                DeviationBoxL.IsEnabled = false;
            else
                DeviationBoxL.IsEnabled = true;
            switch (x.Name)
            {
                case "exp2":
                    ParametersLLabel.Content = Properties.Resources.ParametersExpLabel;
                    DeviationBoxL.IsEnabled = false;
                    break;
                case "norm2":
                    ParametersLLabel.Content = Properties.Resources.ParametersNormalLabel;
                    DeviationBoxL.IsEnabled = true;
                    break;
                default:
                    ParametersLLabel.Content = Properties.Resources.ParametersUniformLabel;
                    DeviationBoxL.IsEnabled = true;
                    break;
            }
        }

        private void Back_Clicked(object sender, RoutedEventArgs e)
        {
            var window = new SolverInputWindow();
            window.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
