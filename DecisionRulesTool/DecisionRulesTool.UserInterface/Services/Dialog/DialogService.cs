using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace DecisionRulesTool.UserInterface.Services.Dialog
{
    public class DialogService
    {
        private Window window;

        public DialogService(Window window)
        {
            this.window = window;
        }

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

        public string[] OpenFileDialog(OpenFileDialogSettings options)
        {
            string[] filePaths = new string[0];

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = options.Multiselect;
            openFileDialog.Filter = options.Filter;
            openFileDialog.InitialDirectory = options.InitialDirectory;

            if (openFileDialog.ShowDialog() == true)
            {
                filePaths = openFileDialog.FileNames;

            }

            return filePaths;
        }


    }
}