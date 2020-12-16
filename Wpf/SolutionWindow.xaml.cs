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

            OutputBlock.Text = solution.SolutionWithoutChange.OptimalX;
            OptimalValueBlock.Text = solution.SolutionWithoutChange.FunctionValue;
        }
    }
}
