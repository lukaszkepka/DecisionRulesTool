using DecisionRulesTool.UserInterface.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionRulesTool.UserInterface.Services
{
    class WindowNavigatorService
    {
        public void NavigateToWindow(BaseWindowViewModel viewModel)
        {
            switch (viewModel)
            {
                case RuleSubsetGenerationViewModel ruleSubsetGenerationViewModel:
                    break;
                default:
                    break;
            }
            //Window newWindow = null;
            //if (viewModel is CategoryViewModel)
            //{
            //    newWindow = new CategoryWindow();
            //}
            //else if (viewModel is GoalViewModel)
            //{
            //    newWindow = new GoalWindow();
            //}
            //else if (viewModel is EntryViewModel)
            //{
            //    newWindow = new EntryWindow();
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}

            //if (newWindow != null)
            //{
            //    newWindow.DataContext = viewModel;
            //    newWindow.Show();
            //    viewModel.CloseRequest += (sender, e) => newWindow.Close();
            //}

            Window mew = new MainWindow();

            OpenFileDialog a = new OpenFileDialog();
            a.ShowDialog(mew);

        }

        public void NavigateToWindow(string windowName)
        {
            //BaseWindowViewModel newViewModel = null;

            switch (windowName)
            {
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
            //NavigateToWindow(newViewModel);
        }
    }
}
