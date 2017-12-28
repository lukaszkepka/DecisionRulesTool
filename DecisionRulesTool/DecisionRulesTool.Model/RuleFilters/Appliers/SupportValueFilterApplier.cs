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
    public class SupportValueFilterApplier : ValueBasedFiltersApplier
    {
        public SupportValueFilterApplier(int minLength, int maxLength, Relation relation, IRuleSetSubsetFactory ruleSetSubsetFactory) : base(minLength, maxLength, relation, ruleSetSubsetFactory)
        {
        }

        public override int GetUpperBound(NumberBasedFilter lengthFilter, RuleSet ruleSet)
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

        public override NumberBasedFilter InstantiateSingleFilter(int desiredValue, Relation relation)
        {
            return new SupportValueFilter(relation, desiredValue);
        }
    }
}
