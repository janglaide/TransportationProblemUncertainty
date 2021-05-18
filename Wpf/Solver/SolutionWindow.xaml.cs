using System;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary.ForWPF;
using ClassLibrary.ForWPF.SolutionBundles;
using Microsoft.Win32;

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
            OutputBlock.Margin = new Thickness(23, 199, 0, 0);

            //OutputBlock.Width = 65 * solution.SolutionWithoutChange.N;
            //OutputBlock.Height = 35 * solution.SolutionWithoutChange.N;

            MatrixesCLabel.VerticalAlignment = VerticalAlignment.Top;
            MatrixesCLabel.HorizontalAlignment = HorizontalAlignment.Left;
            var labelsHeight = 199 + 24 * solution.SolutionWithoutChange.N + 20;
            MatrixesCLabel.Margin = new Thickness(23, labelsHeight, 0, 0);

            var XsLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N, 
                    labelsHeight, 0, 0),
                Content = "Solutions X:"
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
                    Content = "Delta:"
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
                    Content = "Distance:"
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
                Content = $"After changes ({solution.PersentOfChange}%) Optimal Value:\t{solution.SolutionWithChange.FunctionValue}"
            };
            CoolGrid.Children.Add(ChangesLabel);


            var optimalXLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 24 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Content = "Optimal X:",
                Margin = new Thickness(23, height + 45, 0, 0)
            };
            CoolGrid.Children.Add(optimalXLabel);
            var alphaChangedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 24 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Content = "Alpha Changed:",
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
                Content = "Matrixes C:"
            };
            CoolGrid.Children.Add(CsMatrixesChanged);
            var XsChangedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N,
                    height + 50 + 24 * solution.SolutionWithChange.N, 0, 0),
                Content = "Solutions X:"
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
                    Content = "Delta:"
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
                    Content = "Distance:"
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

        private void Resized(object sender, SizeChangedEventArgs e)
        {
            SaveInputButton.Margin = new Thickness(Width - 200, 77, 0, 0);
            SaveResult.Margin = new Thickness(Width - 200, 135, 0, 0);
            AccuracyLabel.Margin = new Thickness(Width - 435, 199, 0, 0);
            AccuracyComboBox.Margin = new Thickness(Width - 200, 204, 0, 0);
            FindPercentButton.Margin = new Thickness(Width - 200, 261, 0, 0);
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
            var accuracy = double.Parse(AccuracyComboBox.Text);
        }

        private void Accuracy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindPercentButton.IsEnabled = AccuracyComboBox.SelectedItem != null;
        }
    }
}
