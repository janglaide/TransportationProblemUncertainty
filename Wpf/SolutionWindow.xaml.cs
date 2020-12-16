using System.Windows;
using System.Windows.Controls;
using ClassLibrary;

namespace Wpf
{
    /// <summary>
    /// Логика взаимодействия для SolutionWindow.xaml
    /// </summary>
    public partial class SolutionWindow : Window
    {
        public SolutionWindow(FullSolution solution)
        {
            InitializeComponent();

            OptimalValueBlock.Text = solution.SolutionWithoutChange.FunctionValue;
            AlphaBlock.Text = solution.SolutionWithoutChange.Alpha;
            AlphaBlockChanged.Text = solution.SolutionWithoutChange.AlphaChanged;
            OutputBlock.Text = solution.SolutionWithoutChange.OptimalX;
            OutputBlock.VerticalAlignment = VerticalAlignment.Top;
            OutputBlock.HorizontalAlignment = HorizontalAlignment.Left;
            OutputBlock.Margin = new Thickness(23, 101, 0, 0);

            //OutputBlock.Width = 65 * solution.SolutionWithoutChange.N;
            //OutputBlock.Height = 35 * solution.SolutionWithoutChange.N;

            MatrixesCLabel.VerticalAlignment = VerticalAlignment.Top;
            MatrixesCLabel.HorizontalAlignment = HorizontalAlignment.Left;
            MatrixesCLabel.Margin = new Thickness(23, 70 + 35 * solution.SolutionWithoutChange.N, 0, 0);

            var XsLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50 + 65 * solution.SolutionWithoutChange.N, 
                    70 + 35 * solution.SolutionWithoutChange.N, 0, 0),
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
                    Margin = new Thickness(23, 120 + 35 * solution.SolutionWithoutChange.N + 
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
                        120 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0)
                };
                var FsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        115 + 35 * solution.SolutionWithoutChange.N + 
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "F:"
                };
                var FsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        120 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Fs[i]
                };
                var DeltasLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        135 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "Delta:"
                };
                var DeltasValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        140 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Deltas[i]
                };
                var YsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        155 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "Y:"
                };
                var YsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        160 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Ys[i]
                };
                var DistancesLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithoutChange.N,
                        175 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Content = "Distance:"
                };
                var DistancesValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithoutChange.N,
                        180 + 35 * solution.SolutionWithoutChange.N +
                        (i * 35 * (solution.SolutionWithoutChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithoutChange.Distances[i]
                };
                CoolGrid.Children.Add(newTxtBlock);
                CoolGrid.Children.Add(newXBlock);
                CoolGrid.Children.Add(FsLabel);
                CoolGrid.Children.Add(FsValues);
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
                Content = "After changes:"
            };
            CoolGrid.Children.Add(ChangesLabel);


            for (var i = 0; i < solution.SolutionWithChange.Cs.Length; i++)
            {
                var newTxtBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 35 * solution.SolutionWithChange.N,
                    Width = 65 * solution.SolutionWithChange.N,
                    Text = solution.SolutionWithChange.Cs[i],
                    Margin = new Thickness(23, height + 50 +
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
                         height + 50 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0)
                };
                var FsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 45 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "F:"
                };
                var FsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 50 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Fs[i]
                };
                var DeltasLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 65 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "Delta:"
                };
                var DeltasValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 70 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Deltas[i]
                };
                var YsLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 85 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "Y:"
                };
                var YsValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 90 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Ys[i]
                };
                var DistancesLabel = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(75 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 105 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Content = "Distance:"
                };
                var DistancesValues = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(150 + 2 * 65 * solution.SolutionWithChange.N,
                         height + 110 +
                        (i * 35 * (solution.SolutionWithChange.N - 1)), 0, 0),
                    Text = solution.SolutionWithChange.Distances[i]
                };
                CoolGrid.Children.Add(newTxtBlock);
                CoolGrid.Children.Add(newXBlock);
                CoolGrid.Children.Add(FsLabel);
                CoolGrid.Children.Add(FsValues);
                CoolGrid.Children.Add(DeltasLabel);
                CoolGrid.Children.Add(DeltasValues);
                CoolGrid.Children.Add(YsLabel);
                CoolGrid.Children.Add(YsValues);
                CoolGrid.Children.Add(DistancesLabel);
                CoolGrid.Children.Add(DistancesValues);
            }
        }
    }
}
