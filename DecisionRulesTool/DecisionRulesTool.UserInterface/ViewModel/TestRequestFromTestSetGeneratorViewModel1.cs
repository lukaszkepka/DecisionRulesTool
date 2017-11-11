using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using Unity;
using System.Windows.Input;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.ObjectModel;
using PropertyChanged;
using GalaSoft.MvvmLight.Command;
using DecisionRulesTool.UserInterface.Services;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TestRequestFromTestSetGeneratorViewModel : TestRequestGeneratorViewModel
    {
        private DataSet testSet;

        public ICommand SelectRuleSets { get; private set; }
        public ICommand UnselectRuleSets { get; private set; }
        public ICollection<RuleSetSubset> RuleSets { get; private set; }

        public TestRequestFromTestSetGeneratorViewModel(ICollection<RuleSetSubset> ruleSets, DataSet testSet, ServicesRepository servicesRepository)
            : base(servicesRepository)
        {
            this.testSet = testSet;
            InitializeCommands();
            InitializeRuleSets(ruleSets);
        }

        private void InitializeCommands()
        {
            SelectRuleSets = new RelayCommand(OnSelectRuleSets);
            UnselectRuleSets = new RelayCommand(OnUnselectRuleSets);
        }

        private void InitializeRuleSets(ICollection<RuleSetSubset> ruleSets)
        {
            this.RuleSets = new ObservableCollection<RuleSetSubset>();
            foreach (var ruleSet in ruleSets)
            {
                if (ruleSet.Attributes.SequenceEqual(testSet.Attributes))
                {
                    this.RuleSets.Add(ruleSet);
                }
            }
        }

        private void OnSelectRuleSets()
        {
            servicesRepository.RuleSetSubsetService.SelectAllSubsets(RuleSets);
        }

        private void OnUnselectRuleSets()
        {
            servicesRepository.RuleSetSubsetService.UnselectEmptySubsets(RuleSets);
        }

        public override IEnumerable<TestRequest> GenerateTestRequests()
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
            return GetSelectedRuleSets(RuleSets);
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
    }
}
