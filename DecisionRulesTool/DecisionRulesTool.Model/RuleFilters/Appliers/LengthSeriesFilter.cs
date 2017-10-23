using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;

namespace DecisionRulesTool.Model.RuleFilters.Appliers
{
    public class LengthFilterApplier : ValueBasedFiltersApplier
    {
        public LengthFilterApplier(int minLength, int maxLength, Relation relation, IRuleSetSubsetFactory ruleSetSubsetFactory) : base(minLength, maxLength, relation, ruleSetSubsetFactory)
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
