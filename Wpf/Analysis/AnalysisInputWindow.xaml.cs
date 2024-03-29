﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using ClassLibrary.Generators;
using ClassLibrary.Logic;

namespace Wpf.Analysis
{
    /// <summary>
    /// Логика взаимодействия для AnalysisInputWindow.xaml
    /// </summary>
    public partial class AnalysisInputWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private GeneratorTaskCondition _generator;
        private Experiment _experiment;
        private List<List<(int, double)>> _results;
        private int _startsize, _finalsize, _step, _R, _startR, _finalR, _stepR;
        private double _accuracy;
        private bool _isRangeRSelected = false;
        private bool? _isCheckedCDefault, _isCheckedCBasic, _isCheckedCNonbasic;
        private List<string> _messages;
        private ClassLibrary.Enums.CChangeParameters _parametersForChecked;
        public AnalysisInputWindow()
        {
            InitializeComponent();
        }

        private void RunButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AccuracyComboBox.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelAccuracyNOTChosen);
                var accuracy = double.Parse(AccuracyComboBox.Text);
                if (!int.TryParse(StartSizeBox.Text, out int startSize) ||
                    !int.TryParse(FinalSizeBox.Text, out int finalSize)) throw new Exception(Properties.Resources.ExceptionLabelNRangeIntegers);
                if (finalSize - startSize < 0) throw new Exception(Properties.Resources.ExceptionLabelInvalidRangeSizing);
                if (startSize <= 0) throw new Exception(Properties.Resources.ExceptionLabelSizesMatrixesPositive);
                if (!int.TryParse(StepBox.Text, out int step)) throw new Exception(Properties.Resources.ExceptionLabelStepInteger);
                if (step <= 0) throw new Exception(Properties.Resources.ExceptionLabelStepPositive);
                if (DistributionComboBox.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelDistributionCNOTChosen);
                if (DistributionComboBoxAB.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelDistributionABNOTChosen);
                if (DistributionComboBoxL.SelectedItem == null) throw new Exception(Properties.Resources.ExceptionLabelDistributionLNOTChosen);
                if (!_isRangeRSelected)
                {
                    if (!int.TryParse(RBox.Text, out int R)) throw new Exception(Properties.Resources.ExceptionLabelIntegerR);
                    if (R <= 0) throw new Exception(Properties.Resources.ExceptionLabelPositiveR);
                    if (checkboxForCDefault.IsChecked != true &&
                        checkboxForCBasic.IsChecked != true &&
                        checkboxForCNonbasic.IsChecked != true) throw new Exception(Properties.Resources.ExceptionCheckboxesC);
                    _isCheckedCDefault = checkboxForCDefault.IsChecked;
                    _isCheckedCBasic = checkboxForCBasic.IsChecked;
                    _isCheckedCNonbasic = checkboxForCNonbasic.IsChecked;
                    _R = R;
                }
                if (_isRangeRSelected)
                {
                    if (!int.TryParse(RStepTextBox.Text, out int stepR)) throw new Exception(Properties.Resources.ExceptionLabelRStepInteger);
                    if (stepR <= 0) throw new Exception(Properties.Resources.ExceptionLabelRStepPositive);
                    if (!int.TryParse(StartQuantityOfMatrixes.Text, out int startR) ||
                        !int.TryParse(FinalQuantityOfMatrixes.Text, out int finalR)) throw new Exception(Properties.Resources.ExceptionLabelRRangeIntegers);
                    if (finalR - startR < 0) throw new Exception(Properties.Resources.ExceptionLabelRInvalidQuatitySizing);
                    if (startR <= 0) throw new Exception(Properties.Resources.ExceptionLabelRRangePositive);
                    _stepR = stepR;
                    _startR = startR;
                    _finalR = finalR;

                    if (radioButtonForCBasic.IsChecked == true)
                        _parametersForChecked = ClassLibrary.Enums.CChangeParameters.Basic;
                    else if (radioButtonForCDefault.IsChecked == true)
                        _parametersForChecked = ClassLibrary.Enums.CChangeParameters.Default;
                    else
                        _parametersForChecked = ClassLibrary.Enums.CChangeParameters.NonBasic;
                }                

                double delay, deviation, a, b;

                (double, double) paramsC;
                (double, double) paramsAB;
                (double, double) paramsL;
                (string, string, string) distribution;

                var item = (ComboBoxItem)DistributionComboBox.SelectedItem;
                switch (item.Name)
                {
                    case "exp":
                        if (!double.TryParse(DelayBox.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        paramsC = (delay, delay);
                        distribution.Item1 = "Exponential";
                        //experiment = new Experiment(item.Name, (delay, delay), (delay, delay), (delay, delay));
                        break;
                    case "norm":
                        if (!double.TryParse(DelayBox.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        if (!double.TryParse(DeviationBox.Text, out deviation)) throw new Exception(Properties.Resources.ExceptionLabelDeviationNumeric);
                        if (deviation <= 0) throw new Exception(Properties.Resources.ExceptionLabelDeviationPositive);
                        a = -1.0;
                        b = -1.0;
                        paramsC = (delay, deviation);
                        distribution.Item1 = "Normal";
                        //experiment = new Experiment(item.Name, (delay, deviation), (delay, deviation), (delay, deviation));
                        break;
                    default:
                        if (!double.TryParse(DelayBox.Text, out a) || !double.TryParse(DeviationBox.Text, out b))
                            throw new Exception(Properties.Resources.ExceptionLabelRangeNumbers);
                        if (b - a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangePositive);
                        if (a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangeStartsInPositive);
                        delay = -1.0;
                        deviation = -1.0;
                        paramsC = (a, b);
                        distribution.Item1 = "Uniform";
                        //experiment = new Experiment(item.Name, (a, b), (a, b), (a, b));
                        break;
                }

                var itemAB = (ComboBoxItem)DistributionComboBoxAB.SelectedItem;
                switch (itemAB.Name)
                {
                    case "exp1":
                        if (!double.TryParse(DelayBoxAB.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        paramsAB = (delay, delay);
                        distribution.Item2 = "Exponential";
                        //experiment = new Experiment(item.Name, (delay, delay), (delay, delay), (delay, delay));
                        break;
                    case "norm1":
                        if (!double.TryParse(DelayBoxAB.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        if (!double.TryParse(DeviationBoxAB.Text, out deviation)) throw new Exception(Properties.Resources.ExceptionLabelDeviationNumeric);
                        if (deviation <= 0) throw new Exception(Properties.Resources.ExceptionLabelDeviationPositive);
                        a = -1.0;
                        b = -1.0;
                        paramsAB = (delay, deviation);
                        distribution.Item2 = "Normal";
                        //experiment = new Experiment(item.Name, (delay, deviation), (delay, deviation), (delay, deviation));
                        break;
                    default:
                        if (!double.TryParse(DelayBoxAB.Text, out a) || !double.TryParse(DeviationBoxAB.Text, out b))
                            throw new Exception(Properties.Resources.ExceptionLabelRangeNumbers);
                        if (b - a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangePositive);
                        if (a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangeStartsInPositive);
                        delay = -1.0;
                        deviation = -1.0;
                        paramsAB = (a, b);
                        distribution.Item2 = "Uniform";
                        //experiment = new Experiment(item.Name, (a, b), (a, b), (a, b));
                        break;
                }

                var itemL = (ComboBoxItem)DistributionComboBoxL.SelectedItem;
                switch (itemL.Name)
                {
                    case "exp2":
                        if (!double.TryParse(DelayBoxL.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        deviation = -1.0;
                        a = -1.0;
                        b = -1.0;
                        paramsL = (delay, delay);
                        distribution.Item3 = "Exponential";
                        //experiment = new Experiment(item.Name, (delay, delay), (delay, delay), (delay, delay));
                        break;
                    case "norm2":
                        if (!double.TryParse(DelayBoxL.Text, out delay)) throw new Exception(Properties.Resources.ExceptionLabelDelayNumeric);
                        if (delay <= 0) throw new Exception(Properties.Resources.ExceptionLabelDelayPositive);
                        if (!double.TryParse(DeviationBoxL.Text, out deviation)) throw new Exception(Properties.Resources.ExceptionLabelDeviationNumeric);
                        if (deviation <= 0) throw new Exception(Properties.Resources.ExceptionLabelDeviationPositive);
                        a = -1.0;
                        b = -1.0;
                        paramsL = (delay, deviation);
                        distribution.Item3 = "Normal";
                        //experiment = new Experiment(item.Name, (delay, deviation), (delay, deviation), (delay, deviation));
                        break;
                    default:
                        if (!double.TryParse(DelayBoxL.Text, out a) || !double.TryParse(DeviationBoxL.Text, out b))
                            throw new Exception(Properties.Resources.ExceptionLabelRangeNumbers);
                        if (b - a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangePositive);
                        if (a < 0) throw new Exception(Properties.Resources.ExceptionLabelRangeStartsInPositive);
                        delay = -1.0;
                        deviation = -1.0;
                        paramsL = (a, b);
                        distribution.Item3 = "Uniform";
                        break;
                }
                
                ExceptionLabel.Content = "";
                ProgressingBar.Visibility = Visibility.Visible;
                
                ProgressingBar.Maximum = 100;
                ProgressingBar.Minimum = 0;
                ProgressingBar.Value = 0;

                _generator = new GeneratorTaskCondition(distribution, paramsC, paramsAB, paramsL);
                _experiment = new Experiment(_generator);
                _startsize = startSize;
                _finalsize = finalSize;
                _step = step;
                _accuracy = accuracy;                

                BackgroundWorker worker = new BackgroundWorker();
                worker.RunWorkerCompleted += Completed;
                worker.WorkerReportsProgress = true;
                worker.DoWork += Run;
                worker.ProgressChanged += ProgressChanged;
                worker.RunWorkerAsync();

                RunButton.IsEnabled = false;
                
            }
            catch(Exception ex)
            {
                ExceptionLabel.Content = ex.Message;
            }
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressingBar.Value = e.ProgressPercentage;
            ProgressTextBlock.Text = (string)e.UserState;
        }

        private void Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            var window = new ExperimentResultWindow(_results, _messages);
            ProgressingBar.Value = 0;
            ProgressingBar.Visibility = Visibility.Hidden;
            ProgressTextBlock.Text = "";
            RunButton.IsEnabled = true;
            window.Show();
        }

        private void Run(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            _results = new List<List<(int, double)>>();
            _messages = new List<string>();
            if (!_isRangeRSelected)
            {
                if (_isCheckedCDefault == true)
                {
                    _experiment.SetChangeParameters(ClassLibrary.Enums.CChangeParameters.Default);
                    _results.Add(_experiment.RunExperiment(_startsize, _finalsize, _step, _R, _accuracy, worker, Properties.Resources.ProcessingSize, $", {Properties.Resources.Variables}{Properties.Resources.CDefault}"));
                    _messages.Add(Properties.Resources.CDefault);
                }
                if (_isCheckedCBasic == true)
                {
                    _experiment.SetChangeParameters(ClassLibrary.Enums.CChangeParameters.Basic);
                    _results.Add(_experiment.RunExperiment(_startsize, _finalsize, _step, _R, _accuracy, worker, Properties.Resources.ProcessingSize, $", {Properties.Resources.Variables}{Properties.Resources.CBasic}"));
                    _messages.Add(Properties.Resources.CBasic);
                }
                if (_isCheckedCNonbasic == true)
                {
                    _experiment.SetChangeParameters(ClassLibrary.Enums.CChangeParameters.NonBasic);
                    _results.Add(_experiment.RunExperiment(_startsize, _finalsize, _step, _R, _accuracy, worker, Properties.Resources.ProcessingSize, $", {Properties.Resources.Variables}{Properties.Resources.CNonbasic}"));
                    _messages.Add(Properties.Resources.CNonbasic);
                }
            }
            else
            {
                _experiment.SetChangeParameters(_parametersForChecked);
                _results = _experiment.RunExperiment(_startsize, _finalsize, _step, _startR, _finalR, _stepR, _accuracy, worker, Properties.Resources.ProcessingSize, ", R = ");
                for (int r = _startR; r <= _finalR; r += _stepR)
                {
                    _messages.Add($"{Properties.Resources.Matrixs} {r}");
                }
            }
            worker.ReportProgress(100, Properties.Resources.ProgressTextBlockCompleted);
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
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        private void RangeR_Selected(object sender, RoutedEventArgs e)
        {
            if (_isRangeRSelected)
            {
                return;
            }
            _isRangeRSelected = true;
            QualityRLabel.Visibility = Visibility.Hidden;
            RBox.Visibility = Visibility.Hidden;
            checkboxForCDefault.Visibility = Visibility.Hidden;
            checkboxForCBasic.Visibility = Visibility.Hidden;
            checkboxForCNonbasic.Visibility = Visibility.Hidden;

            RangeRLabel.Visibility = Visibility.Visible;
            StartQuantityOfMatrixes.Visibility = Visibility.Visible;
            FinalQuantityOfMatrixes.Visibility = Visibility.Visible;
            radioButtonForCDefault.Visibility = Visibility.Visible;
            radioButtonForCBasic.Visibility = Visibility.Visible;
            radioButtonForCNonbasic.Visibility = Visibility.Visible;

            RStepBackgroundLabel.Visibility = Visibility.Visible;
            RStepLabel.Visibility = Visibility.Visible;
            RStepTextBox.Visibility = Visibility.Visible;

        }
        private void RangeR_Unselected(object sender, RoutedEventArgs e)
        {
            if (!_isRangeRSelected)
            {
                return;
            }
            _isRangeRSelected = false;
            QualityRLabel.Visibility = Visibility.Visible;
            RBox.Visibility = Visibility.Visible;
            checkboxForCDefault.Visibility = Visibility.Visible;
            checkboxForCBasic.Visibility = Visibility.Visible;
            checkboxForCNonbasic.Visibility = Visibility.Visible;

            RangeRLabel.Visibility = Visibility.Hidden;
            StartQuantityOfMatrixes.Visibility = Visibility.Hidden;
            FinalQuantityOfMatrixes.Visibility = Visibility.Hidden;
            radioButtonForCDefault.Visibility = Visibility.Hidden;
            radioButtonForCBasic.Visibility = Visibility.Hidden;
            radioButtonForCNonbasic.Visibility = Visibility.Hidden;

            RStepBackgroundLabel.Visibility = Visibility.Hidden;
            RStepLabel.Visibility = Visibility.Hidden;
            RStepTextBox.Visibility = Visibility.Hidden;
        }
    }
}
