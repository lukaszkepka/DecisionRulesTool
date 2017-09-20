using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DecisionRulesTool.Model.RuleTester
{
    public class TestRequest
    {
        public RuleSet RuleSet { get; }
        public DataSet TestSet { get; }
        public ConflictResolvingMethod ResolvingMethod { get; }

        public TestRequest(RuleSet ruleSet, DataSet testSet, ConflictResolvingMethod resolvingMethod)
        {
            RuleSet = ruleSet;
            TestSet = testSet;
            ResolvingMethod = resolvingMethod;
        }

    }
}

