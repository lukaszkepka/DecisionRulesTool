using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DecisionRulesTool.Model.RuleTester
{
    /// <summary>
    /// Class responsible for creating test request and running rule testing
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class RuleTesterManager
    {
        private IList<TestObject> testRequests;
        public virtual IEnumerable<TestObject> TestRequests
        {
            get
            {
                return testRequests;
            }
        }

        public RuleTesterManager()
        {
            testRequests = new List<TestObject>();
        }

        public RuleTesterManager(IEnumerable<TestObject> testRequests)
        {
            this.testRequests = testRequests.ToList();
        }

        public virtual IEnumerable<TestObject> GenerateTests(DataSet testSet, IEnumerable<RuleSet> ruleSets, IEnumerable<ConflictResolvingMethod> conflictResolvingMethods)
        {
            IList<TestObject> testRequests = new List<TestObject>();
            foreach (RuleSet ruleSet in ruleSets)
            {
                foreach (ConflictResolvingMethod conflictResolvingMethod in conflictResolvingMethods)
                {
                    testRequests.Add(new TestObject(ruleSet, testSet, conflictResolvingMethod));
                }
            }
            return testRequests;
        }

        public virtual IEnumerable<TestResult> RunTesting(IRuleTester testingStrategy)
        {
            return testingStrategy.RunTesting(testRequests);
        }

        public virtual void AddTestRequest(TestObject testRequest)
        {
            testRequests.Add(testRequest);
        }

        public virtual void AddTestRequests(IEnumerable<TestObject> testRequests)
        {
            foreach (var testRequest in testRequests)
            {
                AddTestRequest(testRequest);
            }
        }

        public virtual void DeleteTestRequest(int index)
        {
            testRequests.RemoveAt(index);
        }

        public virtual void Clear()
        {
            testRequests.Clear();
        }
    }
}

