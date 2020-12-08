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
    /// Логика взаимодействия для SolutionWindow.xaml
    /// </summary>
    public partial class SolutionWindow : Window
    {
        public SolutionWindow((string, string) optimals)
        {
            InitializeComponent();
            OutputBlock.Text = optimals.Item1;
            OptimalValueBlock.Text = optimals.Item2;
        }
    }
}
