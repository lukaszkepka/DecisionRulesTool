using DecisionRulesTool.Model.RuleTester;
using System.Collections.Generic;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.TestSeries
{
    public class TestSeriesRequest : TestRequest
    {
        public TestSeriesRequest(RuleSet ruleSet, DataSet testSet, ConflictResolvingMethod resolvingMethod) : base(ruleSet, testSet, resolvingMethod)
        {
        }

        public List<LengthFilter> LengthFilters { get; set; }
        public List<SupportValueFilter> SupportValueFilters { get; set; }
        public List<AttributePresenceFilter> AttributeFilters { get; set; }
    }
}