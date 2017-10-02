using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.UserInterface.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DecisionRulesTool.UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var dialogService = new DialogService(this);
            var viewModel = new MainWindowViewModel(dialogService);
            viewModel.CloseRequest += (sender, e) => Close();
            DataContext = viewModel;
        }
    }
}
