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
        protected TestSetLoaderService dataSetLoaderService;
        protected RuleSetLoaderService ruleSetLoaderService;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CloseRequest;

        public BaseWindowViewModel()
        {
            this.windowNavigatorService = new WindowNavigatorService();
            this.dialogService = new DialogService();
            this.ruleSetLoaderService = new RuleSetLoaderService(new RuleSetParserFactory(), dialogService);
            this.dataSetLoaderService = new TestSetLoaderService(new DataSetParserFactory(), dialogService);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
