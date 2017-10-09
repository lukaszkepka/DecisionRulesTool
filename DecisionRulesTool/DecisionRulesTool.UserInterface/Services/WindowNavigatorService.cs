using DecisionRulesTool.UserInterface.View;
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
                case RuleSubsetGenerationViewModel ruleSubsetGenerationViewModel:
                    newWindow = new GenerateSubsetsDialog();
                    break;
                case ProgressDialogViewModel progressDialogViewModel:
                    newWindow = new ProgressDialog();
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

        public virtual void NavigateToWindow(string windowName)
        {
            BaseWindowViewModel newViewModel = null;

            switch (windowName)
            {
                case "GenerateSubsetsDialog":
                   // newViewModel = new RuleSubsetGenerationViewModel();
                    break;

                //case "CategoryWindow":
                //    newViewModel = new CategoryViewModel();
                //    break;

                //case "GoalWindow":
                //    newViewModel = new GoalViewModel();
                //    break;

                //case "EntryWindow":
                //    newViewModel = new EntryViewModel();
                //    break;

                default:
                    throw new NotImplementedException();
            }

            SwitchContext(newViewModel);
        }
    }
}
