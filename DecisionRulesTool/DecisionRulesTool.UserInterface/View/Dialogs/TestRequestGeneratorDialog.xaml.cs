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

namespace DecisionRulesTool.UserInterface.View.Dialogs
{
    /// <summary>
    /// Interaction logic for TestRequestGeneratorDialog.xaml
    /// </summary>
    public partial class TestRequestGeneratorDialog : Window
    {
        public TestRequestGeneratorDialog()
        {
            InitializeComponent();
        }

        private void SelectRuleSets(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.SelectRuleSets.Execute(null);
        }

        private void UnselectRuleSets(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            context.UnselectRuleSets.Execute(null);
        }
    }
}
