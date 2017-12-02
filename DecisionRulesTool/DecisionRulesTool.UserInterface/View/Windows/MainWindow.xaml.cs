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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int h = 1;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Count > 0 && e.AddedItems[0] == this.resultViewerTab)
            //{
            //    dynamic dataContext = resultViewerTab.DataContext;
            //    dataContext.TestResultComparisionViewModel.CalculateResultTable.Execute(null);
            //}
        }
    }
}
