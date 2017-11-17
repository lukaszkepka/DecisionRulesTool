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

        private void ItemsControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


        }

        private void testRequestCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var p = (ListBox)sender;
            for (int i = 0; i < p.Items.Count; i++)
            {
                var lbi = (ListBoxItem)p.ItemContainerGenerator.ContainerFromIndex(i);
                lbi.Width = (p.ActualWidth / 3) - 10;
            }
        }

        private void testRequestCollection_Loaded(object sender, RoutedEventArgs e)
        {
            var p = (ListBox)sender;
            for (int i = 0; i < p.Items.Count; i++)
            {
                var lbi = (ListBoxItem)p.ItemContainerGenerator.ContainerFromIndex(i);
                if (lbi != null)
                {
                    lbi.Width = (p.ActualWidth / 3) - 10;
                }
            }
        }
    }
}
