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
    public class TestRequest : INotifyPropertyChanged
    {
        public const int MaxProgress = 100;
        public event PropertyChangedEventHandler PropertyChanged;

        public RuleSet RuleSet { get; }
        public DataSet TestSet { get; set; }
        public ConflictResolvingMethod ResolvingMethod { get; }
        public int Progress { get; set; }
        public TestResult TestResult { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCompleted
        {
            get
            {
                return Progress == MaxProgress;
            }
        }

        public int SeriesNumber { get; set; }

        public TestRequest(RuleSet ruleSet, DataSet testSet, ConflictResolvingMethod resolvingMethod)
        {
            RuleSet = ruleSet;
            TestSet = testSet;
            ResolvingMethod = resolvingMethod;
        }

        public string GetFileName()
        {
            return $"{ DateTime.Now.ToString("yyyyMMdd")}{GetShortenName()}.xlsx";
        }

        public string GetShortenName()
        {
            return $"s{SeriesNumber}_{RuleSet.GetShortenName()}({((RuleSetSubset)RuleSet).FiltersShortInfo})_{TestSet.GetShortenName()}_{ResolvingMethod}";
        }

    }
}

