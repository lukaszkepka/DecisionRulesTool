using DecisionRulesTool.UserInterface.View;
using DecisionRulesTool.UserInterface.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace DecisionRulesTool.UserInterface.Services.Dialog
{
    public class DialogService : WindowNavigatorService, IDialogService
    {
        public void ShowInformationMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarningMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowMessageBox(string caption, string message, MessageBoxButton buttons)
        {
            System.Windows.MessageBox.Show(message, caption, buttons);
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

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
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

        public string SaveFileDialog(SaveFileDialogSettings options)
        {
            string filePath = null;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = options.ExtensionFilter,
                InitialDirectory = options.InitialDirectory
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }

            return filePath;
        }

        public string BrowseFolderDialog(string initialPath)
        {
            string folderPath = string.Empty;
            var folderDialog = new FolderBrowserDialog()
            {
                SelectedPath = initialPath
            };

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderDialog.SelectedPath;
            }

            return folderPath;
        }

    }
}