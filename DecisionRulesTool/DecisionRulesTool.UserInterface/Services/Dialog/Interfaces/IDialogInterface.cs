using DecisionRulesTool.UserInterface.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionRulesTool.UserInterface.Services.Dialog
{
    public interface IDialogService
    {
        void ShowInformationMessage(string message);
        void ShowWarningMessage(string message);
        void ShowErrorMessage(string message);
        void ShowMessageBox(string caption, string message, MessageBoxButton buttons);
        bool ShowDialog(BaseDialogViewModel dialogViewModel);
        string BrowseFolderDialog(string initialPath);
        string[] OpenFileDialog(OpenFileDialogSettings options);
        string SaveFileDialog(SaveFileDialogSettings options);
    }
}
