using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.TestSeries
{
    public class TestSeriesRequestBuilder
    {

        private readonly List<LengthFilter> lengthFilters;
        private readonly List<SupportValueFilter> supportValueFilters;
        private readonly List<AttributePresenceFilter> attributeFilters;

        public TestSeriesRequestBuilder()
        {
            List<LengthFilter> lengthFilters = new List<LengthFilter>();
            List<SupportValueFilter> supportValueFilters = new List<SupportValueFilter>();
            List<AttributePresenceFilter> attributeFilters = new List<AttributePresenceFilter>();
        }


        public void BuildLengthFilters(Relation relation, int minLength, int maxLength, IAttributeValuesComparer valueComparer = null)
        {
            for (int i = minLength; i <= maxLength; i++)
            {
                lengthFilters.Add(new LengthFilter(relation, i));
            }
        }

        public void BuildLengthFilters(Relation relation, RuleSet ruleSet, IAttributeValuesComparer valueComparer = null)
        {
            int maxLength = ruleSet.Rules.Max(x => x.Conditions.Count);
            for (int i = 1; i <= maxLength; i++)
            {
                lengthFilters.Add(new LengthFilter(relation, i));
            }
        }

        public void BuildSupportValueFilters(Relation relation, int minSupport, int maxSupport, IAttributeValuesComparer valueComparer = null)
        {
            for (int i = minSupport; i <= maxSupport; i++)
            {
                supportValueFilters.Add(new SupportValueFilter(relation, i));
            }
        }

        public void BuildSupportValueFilters(Relation relation, RuleSet ruleSet, IAttributeValuesComparer valueComparer = null)
        {
            int maxSupport = ruleSet.Rules.Max(x => x.SupportValue);
            for (int i = 1; i <= maxSupport; i++)
            {
                supportValueFilters.Add(new SupportValueFilter(relation, i));
            }
        }

        public void BuildLengthFilters(Relation relation, IAttributeValuesComparer valueComparer = null)
        {

        }

        public TestSeriesRequest GetTestSeriesRequest()
        {
            return null;
        }
    }
}
