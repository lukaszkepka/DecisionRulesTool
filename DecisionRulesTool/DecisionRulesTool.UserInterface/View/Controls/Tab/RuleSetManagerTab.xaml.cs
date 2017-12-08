using DecisionRulesTool.Model.Model;
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
    /// Interaction logic for RuleSetManagerTab.xaml
    /// </summary>
    public partial class RuleSetManagerTab : UserControl
    {

        private void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((TreeViewItem)sender).IsSelected = true;
            e.Handled = true;
        }

        public RuleSetManagerTab()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            if (((dynamic)e.Source).DataContext is RuleSetSubset ruleSetSubset)
            {
                context.DeleteRuleSet.Execute(ruleSetSubset);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            if (((dynamic)e.Source).DataContext is RuleSetSubset ruleSetSubset)
            {
                context.GenerateSubsets.Execute(ruleSetSubset);
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            if (((dynamic)e.Source).DataContext is RuleSetSubset ruleSetSubset)
            {
                context.DeleteSubsets.Execute(ruleSetSubset);
            }
        }
    }
}
