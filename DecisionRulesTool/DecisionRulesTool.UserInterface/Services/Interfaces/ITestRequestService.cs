using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using System.Collections.Generic;
using System.Data;

namespace DecisionRulesTool.UserInterface.Services
{
    using DecisionRulesTool.Model.Model;
    public interface ITestRequestService
    {
        ICollection<TestObject> Filter(DataSet testSet, ICollection<TestObject> testRequests);
        ICollection<TestObject> Filter(RuleSet ruleSet, ICollection<TestObject> testRequests);
    }
}