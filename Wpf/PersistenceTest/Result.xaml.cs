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

namespace Wpf.PersistenceTest
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Window
    {
        
        public Result(double dbPercentage, double countedPercentage, List<double> percentages)
        {
            InitializeComponent();
            
            CountedPercentageLabel.Content = countedPercentage.ToString();
            DBPercentageLabel.Content = dbPercentage.ToString();
            if(countedPercentage >= dbPercentage){
                ResultDescriptionBlock.Text = "The percentage of change of input data is acceptable " +
                    "for the given parameters. The compromise solution is stable enough";
                CountedPercentageLabel.BorderBrush = Brushes.ForestGreen;
            }
            else
            {
                ResultDescriptionBlock.Text = "The percentage of change of input data is not acceptable. " +
                    "This means that the given compromise solution is not stable enough";
                CountedPercentageLabel.BorderBrush = Brushes.Red;
            }

            PercentagesList.ItemsSource = percentages;

        }
    }
}
