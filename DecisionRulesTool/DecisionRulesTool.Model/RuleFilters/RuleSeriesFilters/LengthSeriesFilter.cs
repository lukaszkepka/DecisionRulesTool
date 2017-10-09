using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class LengthSeriesFilter : LengthFilter, IRuleSeriesFilter
    {
        public int MinLength { get; }
        public int MaxLength { get; }

        public LengthSeriesFilter(int minLength, int maxLength, Relation relation, IAttributeValuesComparer valueComparer = null) : base(relation, 0, valueComparer)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public IList<IRuleFilter> GenerateSeries()
        {
            IList<IRuleFilter> filters = new List<IRuleFilter>();
            for (int i = MinLength; i <= MaxLength; i++)
            {
                IRuleFilter ruleFilter = new LengthFilter(RelationBetweenRulesLengths, i);
                filters.Add(ruleFilter);
            }
            return filters;
        }
    }
}
