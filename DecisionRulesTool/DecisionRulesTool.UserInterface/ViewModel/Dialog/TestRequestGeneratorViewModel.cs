using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Model.Exceptions;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TestRequestGeneratorViewModel : BaseDialogViewModel
    {
        #region Fields
        private IEnumerable<DataSet> testSets;
        #endregion

        #region Properties
        public TestRequestGeneratorOptionsViewModel SettingsViewModel { get; }
        public ICollection<SelectableItem<ConflictResolvingMethod>> ConflictResolvingMethods { get; private set; }
        public ICollection<RuleSetSubset> RuleSets { get; private set; }
        public IEnumerable<DataSet> TestSets
        {
            get
            {
                return testSets;
            }
            set
            {
                SetTestSets(value);
            }
        }
        #endregion

        #region Commands
        public ICommand SelectRuleSets { get; private set; }
        public ICommand UnselectRuleSets { get; private set; }
        public ICommand ShowSettings { get; private set; }
        #endregion

        public TestRequestGeneratorViewModel(ApplicationRepository applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            this.SettingsViewModel = new TestRequestGeneratorOptionsViewModel(applicationCache, servicesRepository);
            InitializeCommands();
            InitializeConflictResolvingMethods();
        }

        private void InitializeCommands()
        {
            ShowSettings = new RelayCommand(OnShowSettings);
            SelectRuleSets = new RelayCommand(OnSelectRuleSets);
            UnselectRuleSets = new RelayCommand(OnUnselectRuleSets);
        }

        private void InitializeConflictResolvingMethods()
        {
            var conflictResolvingMethodsArray = (ConflictResolvingMethod[])Enum.GetValues(typeof(ConflictResolvingMethod));
            this.ConflictResolvingMethods = new ObservableCollection<SelectableItem<ConflictResolvingMethod>>(conflictResolvingMethodsArray.Select(x => new SelectableItem<ConflictResolvingMethod>(x)));
        }

        private void OnShowSettings()
        {
            try
            {
                servicesRepository.DialogService.ShowDialog(SettingsViewModel);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        private void SetTestSets(IEnumerable<DataSet> testSets)
        {
            if (testSets.Any())
            {
                DataSet sampleTestSet = testSets.First();
                bool areTestSetsCompatible = true;
                if (testSets.Count() > 1)
                {
                    foreach (var testSet in testSets)
                    {
                        if (!testSet.IsCompatibleWith(sampleTestSet))
                        {
                            areTestSetsCompatible = false;
                            break;
                        }
                    }
                }

                if (areTestSetsCompatible)
                {
                    FilterRuleSetsApplicableFor(sampleTestSet);
                    this.testSets = testSets;
                }
                else
                {
                    throw new IncompatibleTestSetsException("Selected test sets aren't compatible");
                }
            }
        }

        private void FilterRuleSetsApplicableFor(DataSet testSet)
        {
            this.RuleSets = new ObservableCollection<RuleSetSubset>();
            foreach (var ruleSet in applicationRepository.RuleSets)
            {
                if (ruleSet.Attributes.SequenceEqual(testSet.Attributes))
                {
                    this.RuleSets.Add(ruleSet);
                }
            }
        }

        public IEnumerable<TestObject> GenerateTestRequests()
        {
            IList<TestObject> testRequests = new List<TestObject>();
            foreach (DataSet testSet in TestSets)
            {
                foreach (RuleSet ruleSet in GetSelectedRuleSets())
                {
                    foreach (ConflictResolvingMethod conflictResolvingMethod in GetSelectedConflictResolvingStrategies())
                    {
                        testRequests.Add(new TestObject(ruleSet, testSet, conflictResolvingMethod));
                    }
                }
            }
            return testRequests;
        }

        private List<RuleSet> GetSelectedRuleSets(ICollection<RuleSetSubset> ruleSets)
        {
            List<RuleSet> result = new List<RuleSet>();
            if (ruleSets.Any())
            {
                foreach (var ruleSet in ruleSets)
                {
                    if (((RuleSetViewModel)ruleSet).IsSelected)
                    {
                        result.Add(ruleSet);
                    }
                    result.AddRange(GetSelectedRuleSets(ruleSet.Subsets));
                }
            }
            return result;
        }

        private List<RuleSet> GetSelectedRuleSets()
        {
            return GetSelectedRuleSets(RuleSets);
        }

        private List<ConflictResolvingMethod> GetSelectedConflictResolvingStrategies()
        {
            return ConflictResolvingMethods.Where(x => x.IsSelected).Select(x => x.Item).ToList();
        }

        private void OnSelectRuleSets()
        {
            servicesRepository.RuleSetSubsetService.SelectAllSubsets(RuleSets);
        }

        private void OnUnselectRuleSets()
        {
            servicesRepository.RuleSetSubsetService.UnselectEmptySubsets(RuleSets);
        }

    }
}