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
    /// Interaction logic for TestResultLoadingProgressWindow.xaml
    /// </summary>
    public partial class TestResultLoadingProgressWindow : Window
    {
        public TestResultLoadingProgressWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dynamic dc = DataContext;
            dc.OnCancel();
        }
    }
}
