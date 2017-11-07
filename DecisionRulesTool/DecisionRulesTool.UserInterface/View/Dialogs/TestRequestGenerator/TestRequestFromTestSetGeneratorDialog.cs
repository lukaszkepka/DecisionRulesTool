using System.Windows;

namespace DecisionRulesTool.UserInterface.View.Dialogs
{
    /// <summary>
    /// Interaction logic for RuleSetPickerDialog.xaml
    /// </summary>
    public partial class TestRequestFromTestSetGeneratorDialog : Window
    {
        public TestRequestFromTestSetGeneratorDialog()
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
