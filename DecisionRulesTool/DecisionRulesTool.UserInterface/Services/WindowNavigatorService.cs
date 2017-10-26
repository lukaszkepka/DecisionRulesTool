using DecisionRulesTool.UserInterface.View;
using DecisionRulesTool.UserInterface.View.Dialogs;
using DecisionRulesTool.UserInterface.ViewModel;
using DecisionRulesTool.UserInterface.ViewModel.Dialog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionRulesTool.UserInterface.Services
{
    public class WindowNavigatorService
    {
        protected Window GetWindow(BaseWindowViewModel viewModel)
        {
            Window newWindow = null;
            switch (viewModel)
            {
                case TestConfigurationViewModel testConfigurationViewModel:
                    newWindow = new TestConfigurationWindow();
                    break;
                case TestManagerViewModel testManagerViewModel:
                    newWindow = new TestManagerWindow();
                    break;
                default:
                    break;
            }
            return newWindow;
        }

        protected Window GetDialogWindow(BaseDialogViewModel viewModel)
        {
            Window newWindow = null;
            switch (viewModel)
            {
                case RuleSubsetGenerationViewModel ruleSubsetGenerationViewModel:
                    newWindow = new GenerateSubsetsDialog();
                    break;
                case ProgressDialogViewModel progressDialogViewModel:
                    newWindow = new ProgressDialog();
                    break;
                case TestSetViewModel testSetViewModel:
                    newWindow = new TestSetDialog();
                    break;
                case TestRequestGeneratorViewModel ruleSetPickerViewModel:
                    newWindow = new RuleSetPickerDialog();
                    break;
                case TestRequestGeneratorOptionsViewModel testRequestGeneratorOptionsViewModel:
                    newWindow = new TestRequestGeneratorOptionsDialog();
                    break;
                default:
                    break;
            }
            return newWindow;
        }

        public virtual void SwitchContext(BaseWindowViewModel viewModel)
        {
            Window newWindow = GetWindow(viewModel);
            if (newWindow != null)
            {
                newWindow.DataContext = viewModel;
                newWindow.Show();
                viewModel.CloseRequest += (sender, e) => newWindow.Close();
            }
        }
    }
}
