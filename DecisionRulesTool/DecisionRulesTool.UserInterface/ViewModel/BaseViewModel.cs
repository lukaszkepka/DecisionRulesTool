using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected IUnityContainer containter;
        protected ITestRequestService testRequestService;
        protected DialogService dialogService;
        protected WindowNavigatorService windowNavigatorService;
        protected TestSetLoaderService dataSetLoaderService;
        protected RuleSetLoaderService ruleSetLoaderService;
        protected RuleSetSubsetService ruleSetSubsetService;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(IUnityContainer containter)
        {
            this.containter = containter;
            this.testRequestService = new TestRequestService();
            this.windowNavigatorService = new WindowNavigatorService();
            this.dialogService = new DialogService();
            this.ruleSetLoaderService = new RuleSetLoaderService(new RuleSetParserFactory(), dialogService);
            this.dataSetLoaderService = new TestSetLoaderService(new DataSetParserFactory(), dialogService);
            this.ruleSetSubsetService = new RuleSetSubsetService();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
