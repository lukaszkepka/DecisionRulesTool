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
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public RuleSetManagerViewModel RuleSetManagerViewModel { get; }
        public TestManagerViewModel TestManagerViewModel { get; }

        public MainWindowViewModel(RuleSetManagerViewModel ruleSetManagerViewModel, TestManagerViewModel testManagerViewModel,
                                   ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.RuleSetManagerViewModel = ruleSetManagerViewModel;
            this.TestManagerViewModel = testManagerViewModel;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
        }
    }
}
