using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public abstract class BaseFilterApplier : IRuleFilterApplier
    {
        public abstract IList<IRuleFilter> GenerateSeries();

        public virtual RuleSetSubset ApplySingleFilter(IRuleFilter filter, RuleSetSubset ruleSet)
        {
            //Use filter to generate new subset
            RuleSetSubset subset = new RuleSetSubset(ruleSet);
            subset.AddFilter(filter);
            subset.ApplyFilters();

            //Attach new subset to parent
            ruleSet.Subsets.Add(subset);
            return subset;
        }

        public virtual RuleSetSubset[] ApplyFilterSeries(RuleSetSubset ruleSet)
        {
            List<RuleSetSubset> actualSubsetLevel = new List<RuleSetSubset>();
            foreach (var filter in GenerateSeries())
            {
                var subset = ApplySingleFilter(filter, ruleSet);
                //Mark new subset as parent for next level
                actualSubsetLevel.Add(subset);
            }

            return actualSubsetLevel.ToArray();
        }

        public void SetSubsetName(RuleSetSubset subset)
        {
            subset.Name = $"{subset.RootRuleSet.Name}: {subset.Filters.LastOrDefault()?.ToString()} [{subset.Rules.Count}]";
        }
    }
}
