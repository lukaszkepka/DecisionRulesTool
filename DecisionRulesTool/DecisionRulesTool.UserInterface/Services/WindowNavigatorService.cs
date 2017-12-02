using DecisionRulesTool.UserInterface.Services.Interfaces;
using DecisionRulesTool.UserInterface.View;
using DecisionRulesTool.UserInterface.View.Dialogs;
using DecisionRulesTool.UserInterface.View.Windows;
using DecisionRulesTool.UserInterface.ViewModel;
using DecisionRulesTool.UserInterface.ViewModel.Dialog;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
using DecisionRulesTool.UserInterface.ViewModel.Results;
using DecisionRulesTool.UserInterface.ViewModel.Windows;
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
                case TestResultViewModel testResultViewModel:
                    newWindow = new TestResultWindow();
                    break;
                case MainWindowViewModel mainWindowViewModel:
                    newWindow = new MainWindow();
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
                case AlgorithmsToTestSetsResultViewModel groupedTestResultViewModel:
                    newWindow = new GroupedTestResultWindow();
                    break;
                case RuleSubsetGenerationViewModel ruleSubsetGenerationViewModel:
                    newWindow = new GenerateSubsetsDialog();
                    break;
                case TestSetViewModel testSetViewModel:
                    newWindow = new TestSetDialog();
                    break;
                case TestRequestGeneratorViewModel testRequestGeneratorOptionsViewModel:
                    newWindow = new TestRequestGeneratorDialog();
                    break;
                default:
                    break;
            }
            return newWindow;
        }

        public virtual void ShowWindow(BaseWindowViewModel windowViewModel)
        {
            Window window = GetWindow(windowViewModel);
            if (window != null)
            {
                window.DataContext = windowViewModel;
                window.Topmost = true;
                window.Show();

            }
        }

        public virtual void SwitchContext(BaseWindowViewModel viewModel)
        {
            Window newWindow = GetWindow(viewModel);
            if (newWindow != null)
            {
                newWindow.DataContext = viewModel;
                newWindow.Show();
                viewModel.CloseRequest += (sender, e) => { newWindow.Close(); };
            }
        }
    }
}
