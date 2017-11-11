using DecisionRulesTool.UserInterface.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DecisionRulesTool.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for TestConfigurationTab.xaml
    /// </summary>
    public partial class TestConfigurationTab : UserControl
    {
        public TestConfigurationTab()
        {
            InitializeComponent();
        }

        private void FilterTestRequests_All(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute(TestConfiguratorViewModel.TestRequestFilter.All);
        }

        private void FilterTestRequests_ForSelectedRuleSet(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute(TestConfiguratorViewModel.TestRequestFilter.ForSelcetedRuleSet);
        }

        private void FilterTestRequests_ForSelectedTestSet(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute(TestConfiguratorViewModel.TestRequestFilter.ForSelectedTestSet);
        }
    }
}
