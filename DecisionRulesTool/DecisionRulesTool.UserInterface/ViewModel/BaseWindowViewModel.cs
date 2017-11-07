﻿using DecisionRulesTool.Model.IO.Parsers.Factory;
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
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public abstract class BaseWindowViewModel : BaseViewModel
    {
        public ICommand MoveToTestConfigurator { get; private set; }
        public ICommand MoveToTestResultViewer { get; private set; }
        public ICommand MoveToRuleSetManager { get; private set; }

        public event EventHandler CloseRequest;

        public BaseWindowViewModel(IUnityContainer container) : base(container)
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

        protected void OnCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
