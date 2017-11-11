﻿using DecisionRulesTool.Model.Model;
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
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public RuleSetManagerViewModel RuleSetManagerViewModel { get; }
        public TestConfiguratorViewModel TestConfiguratorViewModel { get; }
        public TestResultViewerViewModel TestResultViewerViewModel { get; }

        public MainWindowViewModel(RuleSetManagerViewModel ruleSetManagerViewModel, TestConfiguratorViewModel testConfiguratorViewModel,
                                   TestResultViewerViewModel testResultViewerViewModel, ApplicationCache applicationCache,
                                   ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.RuleSetManagerViewModel = ruleSetManagerViewModel;
            this.TestConfiguratorViewModel = testConfiguratorViewModel;
            this.TestResultViewerViewModel = testResultViewerViewModel;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
        }
    }
}
