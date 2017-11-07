using DecisionRulesTool.Model.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace DecisionRulesTool.Model.RuleTester
{
    [AddINotifyPropertyChangedInterface]
    public class TestRequest
    {
        public const int MaxProgress = 100;

        public RuleSet RuleSet { get; }
        public DataSet TestSet { get; }
        public ConflictResolvingMethod ResolvingMethod { get; }
        public int Progress { get; set; }
        public TestResult TestResult { get; set; }

        public TestRequest(RuleSet ruleSet, DataSet testSet, ConflictResolvingMethod resolvingMethod)
        {
            RuleSet = ruleSet;
            TestSet = testSet;
            ResolvingMethod = resolvingMethod;
        }

    }
}

