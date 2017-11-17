using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.ViewModel;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.Services
{
    [AddINotifyPropertyChangedInterface]
    public class TestRequestFromRuleSetGeneratorViewModel : TestRequestGeneratorViewModel
    {
        #region Commands

        #endregion Commands

        #region Properties
        public ICollection<SelectableItem<DataSet>> TestSets { get; private set; }
        public RuleSet RuleSet { get; }
        #endregion Properties

        public TestRequestFromRuleSetGeneratorViewModel(ICollection<DataSet> testSets, RuleSet ruleSet, ServicesRepository servicesRepository)
            : base(servicesRepository)
        {
            this.RuleSet = ruleSet;
            InitializeTestSets(testSets);
            InitializeCommands();
        }

        private void InitializeCommands()
        {

        }

        private void InitializeTestSets(ICollection<DataSet> testSets)
        {
            this.TestSets = new ObservableCollection<SelectableItem<DataSet>>
            (
                testSets.
                Where(x => x.Attributes.SequenceEqual(RuleSet.Attributes)).
                Select(x => new SelectableItem<DataSet>(x))
            );
        }

        public override IEnumerable<TestRequest> GenerateTestRequests()
        {
            IList<TestRequest> testRequests = new List<TestRequest>();
            foreach (DataSet testSet in TestSets.Where(x => x.IsSelected).Select(x => x.Item))
            {
                foreach (ConflictResolvingMethod conflictResolvingMethod in GetSelectedConflictResolvingStrategies())
                {
                    testRequests.Add(new TestRequest(RuleSet, testSet, conflictResolvingMethod));
                }
            }
            return testRequests;
        }
    }
}