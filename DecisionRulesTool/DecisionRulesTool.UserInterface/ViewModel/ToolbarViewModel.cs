using DecisionRulesTool.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    /// <summary>
    /// Responsible for handling commands available in window common toolbar
    /// </summary>
    public class ToolbarViewModel : BaseViewModel
    {
        public ICommand MoveToTestConfigurator { get; private set; }
        public ICommand MoveToTestResultViewer { get; private set; }
        public ICommand MoveToRuleSetManager { get; private set; }

        public ToolbarViewModel(IUnityContainer container) : base(container)
        {
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
                windowNavigatorService.SwitchContext(containter.Resolve<TestConfiguratorViewModel>());
            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        protected virtual void OnMoveToTestResultViewer()
        {
            try
            {
                windowNavigatorService.SwitchContext(containter.Resolve<TestResultViewerViewModel>());
            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        protected virtual void OnMoveToRuleSetManager()
        {
            try
            {
                windowNavigatorService.SwitchContext(containter.Resolve<RuleSetManagerViewModel>());
            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }
    }
}
