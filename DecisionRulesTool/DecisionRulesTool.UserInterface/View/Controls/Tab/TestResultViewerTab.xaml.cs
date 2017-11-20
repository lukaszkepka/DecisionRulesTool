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
    /// Interaction logic for TestResultViewerTab.xaml
    /// </summary>
    public partial class TestResultViewerTab : UserControl
    {
        public TestResultViewerTab()
        {
            InitializeComponent();
        }

        private void TestSetDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.TestResultDataGrid.UpdateLayout();
        }

        private void ResizeTestRequests(object sender)
        {
            var p = (ListBox)sender;
            if (p != null && p.Items != null)
            {
                for (int i = 0; i < p.Items.Count; i++)
                {
                    var lbi = (ListBoxItem)p.ItemContainerGenerator.ContainerFromIndex(i);
                    if (lbi != null)
                    {
                        lbi.Width = p.ActualWidth - 30;
                    }
                }

            }
        }

        private void ItemsControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //
        }

        private void testRequestsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ResizeTestRequests(sender);
        }

        private void testRequestsView_LayoutUpdated(object sender, EventArgs e)
        {
            ResizeTestRequests(sender);
        }
    }
}
