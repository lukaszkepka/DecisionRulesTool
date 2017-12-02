using DecisionRulesTool.Model.RuleTester;
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
    /// Interaction logic for TestManagerTab.xaml
    /// </summary>
    public partial class TestManagerTab : UserControl
    {
        public TestManagerTab()
        {
            InitializeComponent();
        }

        private void TestSetDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //this.TestResultDataGrid.UpdateLayout();
        }

        private void FilterTestRequests_All(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute(TestManagerViewModel.TestRequestFilter.All);
        }

        private void FilterTestRequests_ForSelectedTestSet(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.FilterTestRequests.Execute(TestManagerViewModel.TestRequestFilter.ForSelectedTestSet);
        }

        private void ItemsControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dynamic context = DataContext;
            if(((dynamic)e.Source).DataContext is TestRequest testRequest)
            {
                context.ShowTestResults.Execute(testRequest);
            }
        }

        private void testRequestCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ResizeTestRequests(sender);
        }

        private void testRequestCollection_Loaded(object sender, RoutedEventArgs e)
        {
            ResizeTestRequests(sender);
        }

        private void ResizeTestRequests(object sender)
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
