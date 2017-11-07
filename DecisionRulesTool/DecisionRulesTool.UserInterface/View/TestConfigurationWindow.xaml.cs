using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DecisionRulesTool.UserInterface.View
{
    /// <summary>
    /// Interaction logic for TestConfigurationWindow.xaml
    /// </summary>
    public partial class TestConfigurationWindow : Window
    {
        public TestConfigurationWindow()
        {
            InitializeComponent();
        }

        private void FilterTestRequests_All(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute("All");
        }

        private void FilterTestRequests_ForSelectedRuleSet(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute("ForSelectedRuleSet");
        }

        private void FilterTestRequests_ForSelectedTestSet(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute("ForSelectedTestSet");
        }
    }
}
