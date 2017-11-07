using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using System.Collections.Generic;
using System.Data;

namespace DecisionRulesTool.UserInterface.Services
{
    using DecisionRulesTool.Model.Model;
    public interface ITestRequestService
    {
        ICollection<TestRequest> Filter(DataSet testSet, ICollection<TestRequest> testRequests);
        ICollection<TestRequest> Filter(RuleSet ruleSet, ICollection<TestRequest> testRequests);
    }
}