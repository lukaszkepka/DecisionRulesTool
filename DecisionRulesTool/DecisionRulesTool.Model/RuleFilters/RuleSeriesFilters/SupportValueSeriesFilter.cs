using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class SupportValueSeriesFilter : SupportValueFilter, IRuleSeriesFilter
    {
        public int MinLength { get; }
        public int MaxLength { get; }

        public SupportValueSeriesFilter(int minLength, int maxLength, Relation relation, IAttributeValuesComparer ruleLengthComparer = null) : base(relation, 0, ruleLengthComparer)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public IList<IRuleFilter> GenerateSeries()
        {
            IList<IRuleFilter> filters = new List<IRuleFilter>();
            for (int i = MinLength; i <= MaxLength; i++)
            {
                IRuleFilter ruleFilter = new SupportValueFilter(relationBetweenRulesSupport, i);
                filters.Add(ruleFilter);
            }
            return filters;
        }
    }
}
