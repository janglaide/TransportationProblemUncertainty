using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ClassLibrary;
using Microsoft.Win32;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для SolutionWindow.xaml
    /// </summary>
    public partial class SolutionWindow : Window
    {
        private const char V = '\n';
        private readonly Problem _data;
        public SolutionWindow(FullSolution solution, Problem data)
        {
            _data = data;
            InitializeComponent();

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
            MatrixesCLabel.Margin = new Thickness(23, 120 + 35 * solution.SolutionWithoutChange.N, 0, 0);

            var XsLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N, 
                    120 + 35 * solution.SolutionWithoutChange.N, 0, 0),
                Content = "Solutions X:"
            };


            CoolGrid.Children.Add(XsLabel);


            for (var i = 0; i < solution.SolutionWithoutChange.Cs.Length; i++)
            {
                var newTxtBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 35 * solution.SolutionWithoutChange.N,
                    Width = 65 * solution.SolutionWithoutChange.N,
                    Text = solution.SolutionWithoutChange.Cs[i],
                    Margin = new Thickness(23, 170 + 35 * solution.SolutionWithoutChange.N + 
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0)
                };
                var newXBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 35 * solution.SolutionWithoutChange.N,
                    Width = 65 * solution.SolutionWithoutChange.N,
                    Text = solution.SolutionWithoutChange.Xs[i],
                    Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N, 
                        170 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0)
                };
                var FsForXLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        165 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "F:"
                };
                var FsForXValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        170 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.FsForX[i]
                };
                var FsForXsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        185 + 35 * solution.SolutionWithoutChange.N + 
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "F':"
                };
                var FsForXsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        190 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.FsForXs[i]
                };
                var DeltasLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        205 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "Delta:"
                };
                var DeltasValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        210 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Deltas[i]
                };
                var YsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        225 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "Y:"
                };
                var YsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        230 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Ys[i]
                };
                var DistancesLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        245 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "Distance:"
                };
                var DistancesValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        250 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Distances[i]
                };
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

            var height = 180 + 35 * solution.SolutionWithoutChange.N +
                        (solution.SolutionWithoutChange.Cs.Length *
                        35 * (solution.SolutionWithoutChange.N - 1));

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
                Height = 35 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Content = "Optimal X:",
                Margin = new Thickness(23, height + 45, 0, 0)
            };
            CoolGrid.Children.Add(optimalXLabel);
            var alphaChangedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 35 * solution.SolutionWithChange.N,
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
                Height = 35 * solution.SolutionWithChange.N,
                Width = 65 * solution.SolutionWithChange.N,
                Text = solution.SolutionWithChange.OptimalX,
                Margin = new Thickness(23, height + 90, 0, 0)
            };
            CoolGrid.Children.Add(changedOtimalX);


            var alphaChanged = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 35 * solution.SolutionWithChange.N,
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
            var CsMatrixesChanged = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(23,
                    height + 50 + 35 * solution.SolutionWithChange.N, 0, 0),
                Content = "Matrixes C:"
            };
            CoolGrid.Children.Add(CsMatrixesChanged);
            var XsChangedLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N,
                    height + 50 + 35 * solution.SolutionWithChange.N, 0, 0),
                Content = "Solutions X:"
            };
            CoolGrid.Children.Add(XsChangedLabel);

            for (var i = 0; i < solution.SolutionWithChange.Cs.Length; i++)
            {
                var newTxtBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 35 * solution.SolutionWithChange.N,
                    Width = 65 * solution.SolutionWithChange.N,
                    Text = solution.SolutionWithChange.Cs[i],
                    Margin = new Thickness(23, height + 100 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0)
                };
                var newXBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 35 * solution.SolutionWithChange.N,
                    Width = 65 * solution.SolutionWithChange.N,
                    Text = solution.SolutionWithChange.Xs[i],
                    Margin = new Thickness(50 + 65 * solution.SolutionWithChange.N,
                         height + 100 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0)
                };


                var FsForXLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 95 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "F:"
                };
                var FsForXValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 100 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.FsForX[i]
                };

                var FsForXsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 115 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "F':"
                };
                var FsForXsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 120 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.FsForXs[i]
                };
                var DeltasLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 135 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "Delta:"
                };
                var DeltasValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 140 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Deltas[i]
                };
                var YsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 155 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "Y:"
                };
                var YsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 160 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Ys[i]
                };
                var DistancesLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 175 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "Distance:"
                };
                var DistancesValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 180 + 35 * solution.SolutionWithChange.N +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Distances[i]
                };
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
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                
                if (saveFileDialog.ShowDialog() != true)
                    throw new Exception("File save dialog does not open");

                var fullText = _data.N.ToString() + "\n\n";
                fullText += _data.R.ToString() + V;
                foreach(var c in _data.Cs)
                {
                    for(var i = 0; i < c.Length; i++)
                    {
                        if (i % Math.Sqrt(c.Length) == 0)
                            fullText += V;
                        fullText += c[i].ToString() + ' ';
                    }
                    fullText += V;
                }
                fullText += V;
                foreach(var a in _data.A)
                    fullText += a.ToString() + ' ';
                fullText += "\n\n";
                foreach (var b in _data.B)
                    fullText += b.ToString() + ' ';
                fullText += "\n\n";
                foreach (var l in _data.L)
                    fullText += l.ToString() + ' ';
                fullText += "\n\n";
                foreach (var alpha in _data.Alpha)
                    fullText += alpha.ToString() + ' ';
                fullText += V;

                File.WriteAllText(saveFileDialog.FileName, fullText);
            }
            catch (Exception)
            { }
            
        }
    }
}
