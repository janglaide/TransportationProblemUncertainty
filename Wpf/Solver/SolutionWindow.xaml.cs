﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.ForWPF;
using ClassLibrary.ForWPF.SolutionBundles;
using ClassLibrary.Logic;
using ClassLibrary.Logic.Services;
using Microsoft.Win32;
using Wpf.PersistenceTest;

namespace Wpf.Solver
{
    /// <summary>
    /// Логика взаимодействия для SolutionWindow.xaml
    /// </summary>
    public partial class SolutionWindow : Window
    {
        private readonly Problem _data;
        private readonly FullSolution _fullSolution;
        public SolutionWindow(FullSolution solution, Problem data)
        {
            _data = data;
            _fullSolution = solution;
            InitializeComponent();
            SaveInputButton.Margin = new Thickness(Width - 100, 77, 0, 0);

            OptimalValueBlock.Text = solution.SolutionWithoutChange.FunctionValue;
            OptimalValueBlock.Margin = new Thickness(13 + ValueLabel.Content.ToString().Length * 9, 13, 0, 0);
            AlphaBlock.Text = solution.SolutionWithoutChange.Alpha;
            if(solution.SolutionWithoutChange.Alpha != solution.SolutionWithoutChange.AlphaChanged)
            {
                AlphaBlockChanged.Text = solution.SolutionWithoutChange.AlphaChanged;
            }
            else
            {
                AlphaBlockChangedLable.Content = "";
            }
            ABlock.Text = solution.A;
            BBlock.Text = solution.B;
            LBlock.Text = solution.L;
            OutputBlock.Text = solution.SolutionWithoutChange.OptimalX;
            OutputBlock.VerticalAlignment = VerticalAlignment.Top;
            OutputBlock.HorizontalAlignment = HorizontalAlignment.Left;

            SaveInputButton.Margin = new Thickness(23, 77, 0, 0);
            SaveInputButton.Width = 200;
            SaveResult.Margin = new Thickness(23, 135, 0, 0);
            SaveResult.Width = 200;
            AccuracyLabel.Margin = new Thickness(23, 199, 0, 0);
            AccuracyComboBox.Margin = new Thickness(275, 204, 0, 0);
            FindPercentButton.Margin = new Thickness(275, 241, 0, 0);

            var startHeight = 80;
            OptimalXsLabel.Margin = new Thickness(23, FindPercentButton.Margin.Top + 40, 0, 0);
            OutputBlock.Margin = new Thickness(23, FindPercentButton.Margin.Top + startHeight, 0, 0);

            //OutputBlock.Width = 65 * solution.SolutionWithoutChange.N;
            //OutputBlock.Height = 35 * solution.SolutionWithoutChange.N;

            MatrixesCLabel.VerticalAlignment = VerticalAlignment.Top;
            MatrixesCLabel.HorizontalAlignment = HorizontalAlignment.Left;
            var labelsHeight = FindPercentButton.Margin.Top + startHeight + 24 * solution.SolutionWithoutChange.N + 20;
            MatrixesCLabel.Margin = new Thickness(23, labelsHeight, 0, 0);

            var XsLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N, 
                    labelsHeight, 0, 0),
                Content = Properties.Resources.SolutionsXLabel
            };


            CoolGrid.Children.Add(XsLabel);


            for (var i = 0; i < solution.SolutionWithoutChange.Cs.Length; i++)
            {
                var newTxtBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 24 * solution.SolutionWithoutChange.N,
                    Width = 65 * solution.SolutionWithoutChange.N,
                    Text = solution.SolutionWithoutChange.Cs[i],
                    Margin = new Thickness(23, labelsHeight + 50 + 
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0)
                };
                var newXBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 24 * solution.SolutionWithoutChange.N,
                    Width = 65 * solution.SolutionWithoutChange.N,
                    Text = solution.SolutionWithoutChange.Xs[i],
                    Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N, 
                        labelsHeight + 50 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0)
                };
                var FsForXLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 50 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Content = "F:"
                };
                var FsForXValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 50 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Text = solution.SolutionWithoutChange.FsForX[i]
                };
                var FsForXsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 67 + 
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Content = "F':"
                };
                var FsForXsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 70 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Text = solution.SolutionWithoutChange.FsForXs[i]
                };
                var DeltasLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 85 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Content = "Δ:"
                };
                var DeltasValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 90 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Text = solution.SolutionWithoutChange.Deltas[i]
                };
                var YsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 105 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Content = "Y:"
                };
                var YsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 110 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Text = solution.SolutionWithoutChange.Ys[i]
                };
                var DistancesLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 125 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Content = Properties.Resources.DistanceLabel
                };
                var DistancesValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        labelsHeight + 130 +
                        (i * 24 * (solution.SolutionWithoutChange.N)), 0, 0),
                    Text = solution.SolutionWithoutChange.Distances[i]
                };
                labelsHeight += 40;
                CoolGrid.Children.Add(newTxtBlock);
                CoolGrid.Children.Add(newXBlock);
                CoolGrid.Children.Add(FsForXLabel);
                CoolGrid.Children.Add(FsForXValues);
                CoolGrid.Children.Add(FsForXsLabel);
                CoolGrid.Children.Add(FsForXsValues);
                CoolGrid.Children.Add(DeltasLabel);
                CoolGrid.Children.Add(DeltasValues);
                CoolGrid.Children.Add(YsLabel);
                CoolGrid.Children.Add(YsValues);
                CoolGrid.Children.Add(DistancesLabel);
                CoolGrid.Children.Add(DistancesValues);
            }

            var height = labelsHeight + 60 +
                        (solution.SolutionWithoutChange.Cs.Length *
                        24 * (solution.SolutionWithoutChange.N));

            var ChangesLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(23, height
                        , 0, 0),
                Content = $"{Properties.Resources.AfterChanges} ({solution.PersentOfChange}%) {Properties.Resources.OptimalValueLabel}\t{solution.SolutionWithChange.FunctionValue}"
            };
            CoolGrid.Children.Add(ChangesLabel);


            var optimalXLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 24 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Content = Properties.Resources.OptimalXLabel,
                Margin = new Thickness(23, height + 45, 0, 0)
            };
            CoolGrid.Children.Add(optimalXLabel);
            var alphaChangedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 24 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Content = Properties.Resources.AlphaChangedLabel,
                Margin = new Thickness(50 + 65 * solution.SolutionWithChange.N,
                         height + 45, 0, 0)
            };
            CoolGrid.Children.Add(alphaChangedLabel);
            var changedOtimalX = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 24 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Text = solution.SolutionWithChange.OptimalX,
                Margin = new Thickness(23, height + 90, 0, 0)
            };
            CoolGrid.Children.Add(changedOtimalX);


            var alphaChanged = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 24 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Text = solution.SolutionWithChange.AlphaChanged,
                Margin = new Thickness(200 + 65 * solution.SolutionWithChange.N,
                         height + 50, 0, 0)
            };
            if (solution.SolutionWithoutChange.AlphaChanged == solution.SolutionWithChange.AlphaChanged)
            {
                alphaChanged.Text = "";
                alphaChangedLabel.Content = "";
            }
            CoolGrid.Children.Add(alphaChanged);
            height += 50;
            var CsMatrixesChanged = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(23,
                    height + 50 + 24 * solution.SolutionWithChange.N, 0, 0),
                Content = Properties.Resources.MatrixesCLabel
            };
            CoolGrid.Children.Add(CsMatrixesChanged);
            var XsChangedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N,
                    height + 50 + 24 * solution.SolutionWithChange.N, 0, 0),
                Content = Properties.Resources.OptimalXLabel
            };
            CoolGrid.Children.Add(XsChangedLabel);

            for (var i = 0; i < solution.SolutionWithChange.Cs.Length; i++)
            {
                var newTxtBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 24 * solution.SolutionWithChange.N,
                    Width = 65 * solution.SolutionWithChange.N,
                    Text = solution.SolutionWithChange.Cs[i],
                    Margin = new Thickness(23, height + 100 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0)
                };
                var newXBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 24 * solution.SolutionWithChange.N,
                    Width = 65 * solution.SolutionWithChange.N,
                    Text = solution.SolutionWithChange.Xs[i],
                    Margin = new Thickness(50 + 65 * solution.SolutionWithChange.N,
                         height + 100 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0)
                };


                var FsForXLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 95 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Content = "F:"
                };
                var FsForXValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 100 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Text = solution.SolutionWithChange.FsForX[i]
                };

                var FsForXsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 115 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Content = "F':"
                };
                var FsForXsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 120 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Text = solution.SolutionWithChange.FsForXs[i]
                };
                var DeltasLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 135 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Content = "Δ:"
                };
                var DeltasValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 140 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Text = solution.SolutionWithChange.Deltas[i]
                };
                var YsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 155 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Content = "Y:"
                };
                var YsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 160 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Text = solution.SolutionWithChange.Ys[i]
                };
                var DistancesLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 175 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Content = Properties.Resources.DistanceLabel
                };
                var DistancesValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 180 + 24 * solution.SolutionWithChange.N +
                        (i * 24 * (solution.SolutionWithChange.N)), 0, 0),
                    Text = solution.SolutionWithChange.Distances[i]
                };
                height += 40;
                CoolGrid.Children.Add(newTxtBlock);
                CoolGrid.Children.Add(newXBlock);
                CoolGrid.Children.Add(FsForXLabel);
                CoolGrid.Children.Add(FsForXValues);
                CoolGrid.Children.Add(FsForXsLabel);
                CoolGrid.Children.Add(FsForXsValues);
                CoolGrid.Children.Add(DeltasLabel);
                CoolGrid.Children.Add(DeltasValues);
                CoolGrid.Children.Add(YsLabel);
                CoolGrid.Children.Add(YsValues);
                CoolGrid.Children.Add(DistancesLabel);
                CoolGrid.Children.Add(DistancesValues);
            }
        }

        private void SaveInputButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = $"condition_{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    //throw new Exception("File save dialog does not open");
                    FileProcessing.WriteProblemIntoFile(_data, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void SaveResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = $"solution_{DateTime.Now:MM-dd-yyyy_HH-mm-ss}.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    //throw new Exception("File save dialog does not open");
                    FileProcessing.WriteSolutionIntoFile(_fullSolution, saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FindPercentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var accuracy = double.Parse(AccuracyComboBox.Text);
                var random = new Random();
                var solver = new ClassLibrary.Logic.Solver();
                //var parametersForDefined = FileProcessing.ReadSolutionForPersistenceTest(_filename);

                (double, double) cParameters = PercentFinder.GetCsRange(_data.ParametersForDefined);
                (double, double) abParameters = PercentFinder.GetABRange(_data.ParametersForDefined);
                (double, double) lParameters = PercentFinder.GetLRange(_data.ParametersForDefined);

                DistributionParametersService distributionParameters = new DistributionParametersService();
                var distributionParametersIds = distributionParameters.GetAppropriateIds(
                    cParameters, abParameters, lParameters);
                if (distributionParametersIds.Count < 5)
                    throw new Exception(Properties.Resources.ExceptionLabelNOTEnoughData);

                PercentageService percentageService = new PercentageService();
                var percentages = percentageService.GetAppropriate(_data.ParametersForDefined.A.Length, distributionParametersIds, _data.R);

                if (percentages.Count < 5)
                    throw new Exception(Properties.Resources.ExceptionLabelNOTEnoughDataForParameters);

                var valueFromDB = percentages.Average();

                //var percent = PercentFinder.FindPercentOfChange(parametersForDefined, solver, random);
                _data.ParametersForDefined.Clear();
                var percent = PercentFinder.SearchMeanPercent(PercentFinder.FindPercentOfChange, _data.ParametersForDefined, accuracy, solver, random);

                var window = new Result(valueFromDB, percent, percentages);
                window.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Accuracy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindPercentButton.IsEnabled = AccuracyComboBox.SelectedItem != null;
        }
    }
}
