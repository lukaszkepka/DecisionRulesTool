using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public abstract class BaseWindowViewModel : INotifyPropertyChanged
    {
        protected DialogService dialogService;
        protected WindowNavigatorService windowNavigatorService;
        protected RuleSetLoaderService ruleSetLoaderService;
        public ICommand NavigationMenuItemClicked { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CloseRequest;

        public BaseWindowViewModel()
        {
            this.windowNavigatorService = new WindowNavigatorService();
            this.dialogService = new DialogService();
            this.ruleSetLoaderService = new RuleSetLoaderService(new RuleSetParserFactory(), dialogService);

            InitializeInternalCommands();
            InitializeCommands();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        protected void OnNavigateToWindowRequest(BaseWindowViewModel viewModel)
        {
            //windowNavigator.NavigateToWindow(viewModel);
            OnCloseRequest();
        }

        private void OnNavigationMenuItemClicked(object windowName)
        {
            //windowNavigator.NavigateToWindow(windowName.ToString());
            OnCloseRequest();
        }

        private void InitializeInternalCommands()
        {
            //NavigationMenuItemClicked = new CommandWithParameter(OnNavigationMenuItemClicked, true);
        }

        protected abstract void InitializeCommands();
    }
}
