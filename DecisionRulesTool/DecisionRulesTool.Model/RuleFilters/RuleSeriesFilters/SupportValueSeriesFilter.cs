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

        public override int GetUpperBound(ValueBasedFilter lengthFilter, RuleSet ruleSet)
        {
            switch (RelationBetweenRulesLengths)
            {
                case Relation.Greather:
                case Relation.GreatherOrEqual:
                    return ruleSet.Rules.Any() ? ruleSet.Rules.Max(x => x.SupportValue) : 0;
                case Relation.Less:
                    return lengthFilter.DesiredValue - 1;
                case Relation.LessOrEqual:
                    return lengthFilter.DesiredValue;
                default:
                    return 0;
            }
        }

        public override ValueBasedFilter InstantiateSingleFilter(int desiredValue, Relation relation)
        {
            return new SupportValueFilter(relation, desiredValue);
        }
    }
}
