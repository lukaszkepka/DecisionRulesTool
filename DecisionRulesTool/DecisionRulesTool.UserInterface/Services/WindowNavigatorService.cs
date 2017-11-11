using DecisionRulesTool.UserInterface.Services.Interfaces;
using DecisionRulesTool.UserInterface.View;
using DecisionRulesTool.UserInterface.View.Dialogs;
using DecisionRulesTool.UserInterface.View.Dialogs.TestRequestGenerator;
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
    public class WindowNavigatorService : IWindowNavigatorService
    {
        protected Window GetWindow(BaseWindowViewModel viewModel)
        {
            Window newWindow = null;
            switch (viewModel)
            {
                case RuleSetManagerViewModel ruleSetManagerViewModel:
                    newWindow = new MainWindow();
                    break;
                case TestConfiguratorViewModel testConfigurationViewModel:
                    newWindow = new TestConfigurationWindow();
                    break;
                case TestResultViewerViewModel testManagerViewModel:
                    newWindow = new TestResultViewer();
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
                case TestSetViewModel testSetViewModel:
                    newWindow = new TestSetDialog();
                    break;
                case TestRequestFromRuleSetGeneratorViewModel testRequestFromRuleSetGeneratorViewModel:
                    newWindow = new TestRequestFromRuleSetGeneratorDialog();
                    break;
                case TestRequestFromTestSetGeneratorViewModel testRequestFromRuleSetGeneratorViewModel:
                    newWindow = new TestRequestFromTestSetGeneratorDialog();
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
