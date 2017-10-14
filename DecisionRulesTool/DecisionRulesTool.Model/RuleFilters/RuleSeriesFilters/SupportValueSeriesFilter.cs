using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class SupportValueFilterApplier : ValueBasedFiltersApplier
    {
        public SupportValueFilterApplier(int minLength, int maxLength, Relation relation) : base(minLength, maxLength, relation)
        {
        }

        public override IList<IRuleFilter> GenerateSeries()
        {
            IList<IRuleFilter> filters = new List<IRuleFilter>();
            for (int i = MinLength; i <= MaxLength; i++)
            {
                IRuleFilter ruleFilter = new SupportValueFilter(this.RelationBetweenRulesLengths, i);
                filters.Add(ruleFilter);
            }
            return filters;
        }
    }
}
