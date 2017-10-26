using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class TestRequestGeneratorViewModel : BaseDialogViewModel
    {
        private TestRequestGeneratorOptionsViewModel settingsViewModel;
        private ICollection<SelectableItem<ConflictResolvingMethod>> conflictResolvingMethods;
        private ICollection<RuleSetSubset> ruleSets;
        private DataSet testSet;

        public ICommand ShowSettings { get; private set; }
        public ICommand SelectRuleSet { get; private set; }

        #region Properties
        public ICollection<RuleSetSubset> RuleSets
        {
            get
            {
                return ruleSets;
            }
            set
            {
                ruleSets = value;
                OnPropertyChanged("RuleSets");
            }
        }
        public ICollection<SelectableItem<ConflictResolvingMethod>> ConflictResolvingMethods
        {
            get
            {
                return conflictResolvingMethods;
            }
            set
            {
                conflictResolvingMethods = value;
                OnPropertyChanged("ConflictResolvingMethods");
            }
        }
        public TestRequestGeneratorOptionsViewModel SettingsViewModel
        {
            get
            {
                return settingsViewModel;
            }
            set
            {
                settingsViewModel = value;
                OnPropertyChanged("SettingsViewModel");
            }
        }
        #endregion
        
        public TestRequestGeneratorViewModel(ICollection<RuleSetSubset> ruleSets, DataSet testSet)
        {
            this.testSet = testSet;
            this.settingsViewModel = new TestRequestGeneratorOptionsViewModel();

            InitializeCommands();
            InitializeConflictResolvingMethods();
            InitializeRuleSets(ruleSets);
        }

        private void InitializeCommands()
        {
            ShowSettings = new RelayCommand(OnShowSettings);
        }

        private void InitializeRuleSets(ICollection<RuleSetSubset> ruleSets)
        {
            this.ruleSets = new ObservableCollection<RuleSetSubset>();
            foreach (var ruleSet in ruleSets)
            {
                if (ruleSet.Attributes.SequenceEqual(testSet.Attributes))
                {
                    this.ruleSets.Add(ruleSet);
                }
            }
        }

        private void InitializeConflictResolvingMethods()
        {
            var conflictResolvingMethodsArray = (ConflictResolvingMethod[])Enum.GetValues(typeof(ConflictResolvingMethod));
            this.conflictResolvingMethods = new ObservableCollection<SelectableItem<ConflictResolvingMethod>>(conflictResolvingMethodsArray.Select(x => new SelectableItem<ConflictResolvingMethod>(x)));
        }

        private void OnShowSettings()
        {
            try
            {
                dialogService.ShowDialog(settingsViewModel);
            }
            catch(Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        public IEnumerable<TestRequest> GetTestRequests()
        {
            IList<TestRequest> testRequests = new List<TestRequest>();
            foreach (RuleSet ruleSet in GetSelectedRuleSets())
            {
                foreach (ConflictResolvingMethod conflictResolvingMethod in GetSelectedConflictResolvingStrategies())
                {
                    testRequests.Add(new TestRequest(ruleSet, testSet, conflictResolvingMethod));
                }
            }
            return testRequests;
        }

        private List<RuleSet> GetSelectedRuleSets()
        {
           return GetSelectedRuleSets(ruleSets);
        }

        private List<RuleSet> GetSelectedRuleSets(ICollection<RuleSetSubset> ruleSets)
        {
            List<RuleSet> result = new List<RuleSet>();
            if (ruleSets.Any())
            {
                foreach (var ruleSet in ruleSets)
                {
                    if (((RuleSetSubsetViewItem)ruleSet).IsSelected)
                    {
                        result.Add(ruleSet);
                    }
                    result.AddRange(GetSelectedRuleSets(ruleSet.Subsets));
                }
            }
            return result;
        }

        private List<ConflictResolvingMethod> GetSelectedConflictResolvingStrategies()
        {
            return conflictResolvingMethods.Where(x => x.IsSelected).Select(x => x.Item).ToList();
        }
    }
}