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

        public override IList<IRuleFilter> GenerateSeries()
        {
            IList<IRuleFilter> filters = new List<IRuleFilter>();
            for (int i = MinLength; i <= MaxLength; i++)
            {
                IRuleFilter ruleFilter = InstantiateSingleFilter(i, this.RelationBetweenRulesLengths);
                filters.Add(ruleFilter);
            }
            return filters;
        }

        public override RuleSetSubset[] ApplyFilterSeries(RuleSetSubset ruleSet)
        {
            IList<RuleSetSubset> currentRuleSubsets = new List<RuleSetSubset>();

            //Apply basic filter
            foreach (var filter in GenerateSeries())
            {
                var subset = ApplySingleFilter(filter, ruleSet);
                //Mark new subset as parent for next level
                currentRuleSubsets.Add(subset);
            }

            if (CanGenerateAdditionalFilters())
            {
                //Apply additional filters
                IList<RuleSetSubset> additionalFilterParents = currentRuleSubsets.ToList();
                currentRuleSubsets.Clear();

                for (int j = 0; j < additionalFilterParents.Count; j++)
                {
                    var parentRuleSubset = additionalFilterParents[j];
                    var parentFilter = (ValueBasedFilter)parentRuleSubset.Filters.LastOrDefault();

                    //TODO : parentRuleSubset.RootRuleSet is temporary solution, fix it 
                    for (int i = GetLowerBound(parentFilter); i <= GetUpperBound(parentFilter, parentRuleSubset.RootRuleSet); i++)
                    {
                        ValueBasedFilter filter = InstantiateSingleFilter(i, Relation.Equality);
                        var subset = ApplySingleFilter(filter, parentRuleSubset);                   
                        //Mark new subset as parent for next level
                        currentRuleSubsets.Add(subset);
                    }
                }
            }

            return currentRuleSubsets.ToArray();
        }

        private bool CanGenerateAdditionalFilters()
        {
            return RelationBetweenRulesLengths != Relation.Equality;
        }

        public virtual int GetLowerBound(ValueBasedFilter lengthFilter)
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

        public abstract ValueBasedFilter InstantiateSingleFilter(int desiredValue, Relation relation);

        public abstract int GetUpperBound(ValueBasedFilter lengthFilter, RuleSet ruleSet);
    }
}
