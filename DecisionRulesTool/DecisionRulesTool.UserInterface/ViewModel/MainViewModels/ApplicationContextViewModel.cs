using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class ApplicationContextViewModel : BaseWindowViewModel
    {
        protected ApplicationCache applicationCache;

        public ICommand MoveToTestConfigurator { get; private set; }
        public ICommand MoveToTestResultViewer { get; private set; }
        public ICommand MoveToRuleSetManager { get; private set; }

        public ApplicationContextViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository)
            : base(servicesRepository)
        {
            this.applicationCache = applicationCache;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            MoveToRuleSetManager = new RelayCommand(OnMoveToRuleSetManager);
            MoveToTestConfigurator = new RelayCommand(OnMoveToTestConfigurator);
            MoveToTestResultViewer = new RelayCommand(OnMoveToTestResultViewer);
        }

        protected virtual void OnMoveToTestConfigurator()
        {
            try
            {
                servicesRepository.WindowNavigatorService.SwitchContext(new TestConfiguratorViewModel(applicationCache, servicesRepository));
                OnCloseRequest();
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        protected virtual void OnMoveToTestResultViewer()
        {
            try
            {
                servicesRepository.WindowNavigatorService.SwitchContext(new TestResultViewerViewModel(applicationCache, servicesRepository));
                OnCloseRequest();
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        protected virtual void OnMoveToRuleSetManager()
        {
            try
            {
                servicesRepository.WindowNavigatorService.SwitchContext(new RuleSetManagerViewModel(applicationCache, servicesRepository));
                OnCloseRequest();
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }
    }
}
