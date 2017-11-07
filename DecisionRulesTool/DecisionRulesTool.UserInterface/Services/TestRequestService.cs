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
        public ICollection<TestRequest> Filter(DataSet testSet, ICollection<TestRequest> testRequests)
        {
            return testRequests.Where(x => x.TestSet.Equals(testSet)).ToList();
        }

        public ICollection<TestRequest> Filter(RuleSet ruleSet, ICollection<TestRequest> testRequests)
        {
            return testRequests.Where(x => x.RuleSet.Equals(ruleSet)).ToList();
        }
    }
}
