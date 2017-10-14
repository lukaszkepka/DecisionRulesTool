using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public abstract class ValueBasedFiltersApplier : BaseFilterApplier
    {
        public Relation RelationBetweenRulesLengths { get; }
        public int MinLength { get; }
        public int MaxLength { get; }

        public ValueBasedFiltersApplier(int minLength, int maxLength, Relation relation)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            RelationBetweenRulesLengths = relation;
        }

        public int GetLowerBound(ValueBasedFilter lengthFilter)
        {
            switch (RelationBetweenRulesLengths)
            {
                case Relation.Greather:
                    return lengthFilter.DesiredValue - 1;
                case Relation.GreatherOrEqual:
                    return lengthFilter.DesiredValue;
                case Relation.Less:
                case Relation.LessOrEqual:
                    return 1;
                default:
                    return 0;
            }
        }
        public int GetUpperBound(ValueBasedFilter lengthFilter, RuleSet ruleSet)
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
    }
}
