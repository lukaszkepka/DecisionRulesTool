using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class LengthSeriesFilter : IRuleFilterApplier
    {
        public Relation RelationBetweenRulesLengths { get; }
        public int MinLength { get; }
        public int MaxLength { get; }

        public LengthSeriesFilter(int minLength, int maxLength, Relation relation)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            RelationBetweenRulesLengths = relation;
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


        public RuleSetSubset[] ApplyFilterSeries(RuleSetSubset ruleSet)
        {
            List<RuleSetSubset> actualSubsetLevel = new List<RuleSetSubset>();
            foreach (var filter in GenerateSeries())
            {
                //Use filter to generate new subset
                RuleSetSubset subset = new RuleSetSubset(ruleSet);
                subset.AddFilter(filter);
                subset.ApplyFilters();

                //Set new subset name 
                SetSubsetName(subset);

                //Attach new subset to parent
                ruleSet.Subsets.Add(subset);
                //Mark new subset as parent for next level
                actualSubsetLevel.Add(subset);
            }


            var yy = actualSubsetLevel.ToList();
            for (int j  = 0; j < yy.Count; j++)
            {
                var item = yy[j];
                var g = item.Filters.LastOrDefault();
                var t = (LengthFilter)g;

                for (int i = GetLowerBound(t); i <= GetUpperBound(t, item); i++)
                {
                    LengthFilter lengthFiler = new LengthFilter(Relation.Equality, i);
                    //Use filter to generate new subset
                    RuleSetSubset subset = new RuleSetSubset(item);
                    subset.AddFilter(lengthFiler);
                    subset.ApplyFilters();

                    //Set new subset name 
                    SetSubsetName(subset);

                    //Attach new subset to parent
                    item.Subsets.Add(subset);
                    //Mark new subset as parent for next level
                    actualSubsetLevel.Add(subset);
                }               
            }

            return actualSubsetLevel.ToArray();
        }

        public int GetLowerBound(LengthFilter lengthFilter)
        {
            switch (RelationBetweenRulesLengths)
            {
                case Relation.Greather:
                    return lengthFilter.DesiredLength - 1;
                case Relation.GreatherOrEqual:
                    return lengthFilter.DesiredLength;
                case Relation.Less:
                case Relation.LessOrEqual:
                    return 1;
                default:
                    return 0;
            }
        }

        public int GetUpperBound(LengthFilter lengthFilter, RuleSet ruleSet)
        {
            switch (RelationBetweenRulesLengths)
            {
                case Relation.Greather:
                case Relation.GreatherOrEqual:
                    return ruleSet.Rules.Max(x => x.Conditions.Count);
                case Relation.Less:
                    return lengthFilter.DesiredLength - 1;
                case Relation.LessOrEqual:
                    return lengthFilter.DesiredLength;
                default:
                    return 0;
            }
        }

        public void SetSubsetName(RuleSetSubset subset)
        {
            subset.Name = $"{subset.RootRuleSet.Name}: {subset.Filters.LastOrDefault()?.ToString()} ";
        }
    }
}
