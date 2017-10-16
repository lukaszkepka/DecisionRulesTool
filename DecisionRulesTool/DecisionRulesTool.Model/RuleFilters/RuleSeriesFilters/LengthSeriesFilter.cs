using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class LengthFilterApplier : ValueBasedFiltersApplier
    {
        public LengthFilterApplier(int minLength, int maxLength, Relation relation) : base(minLength, maxLength, relation)
        {
        }

        public override int GetUpperBound(ValueBasedFilter lengthFilter, RuleSet ruleSet)
        {
            switch (RelationBetweenRulesLengths)
            {
                case Relation.Greather:
                case Relation.GreatherOrEqual:
                    return ruleSet.Rules.Max(x => x.Conditions.Count);
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
            return new LengthFilter(relation, desiredValue);
        }
    }
}
