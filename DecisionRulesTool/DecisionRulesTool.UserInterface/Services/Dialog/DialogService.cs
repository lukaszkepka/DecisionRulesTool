using DecisionRulesTool.UserInterface.View;
using DecisionRulesTool.UserInterface.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace DecisionRulesTool.UserInterface.Services.Dialog
{
    public class DialogService : WindowNavigatorService, IDialogService
    {
        public void ShowInformationMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowMessageBox(string caption, string message, MessageBoxButton buttons)
        {
            MessageBox.Show(message, caption, buttons);
        }

        public bool ShowDialog(BaseDialogViewModel dialogViewModel)
        {
            bool result = false;
            Window window = GetDialogWindow(dialogViewModel);
            if (window != null)
            {
                window.DataContext = dialogViewModel;
                dialogViewModel.CloseRequest += (sender, e) => window.Close();
                window.ShowDialog();
                result = (window.DataContext as BaseDialogViewModel).Result;
                dialogViewModel.CloseRequest -= (sender, e) => window.Close();
            }
            return result;
        }

        public string[] OpenFileDialog(OpenFileDialogSettings options)
        {
            string[] filePaths = new string[0];

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = options.Multiselect,
                Filter = options.Filter,
                InitialDirectory = options.InitialDirectory
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePaths = openFileDialog.FileNames;

            }

            return filePaths;
        }

    }
}