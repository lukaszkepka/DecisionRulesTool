using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;

namespace DecisionRulesTool.UserInterface.Services
{
    public class TestRequestService : ITestRequestService
    {
        public ICollection<TestObject> Filter(DataSet testSet, ICollection<TestObject> testRequests)
        {
            return testRequests.Where(x => x.TestSet.Equals(testSet)).ToList();
        }

        public ICollection<TestObject> Filter(RuleSet ruleSet, ICollection<TestObject> testRequests)
        {
            return testRequests.Where(x => x.RuleSet.Equals(ruleSet)).ToList();
        }
    }
}
