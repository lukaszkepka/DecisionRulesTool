using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class RuleSetSubsetGenerator : IRuleSubsetGenerator
    {
        private IList<IRuleFilterApplier> ruleFilters;
        private RuleSetSubset rootRuleSet;

        public IEnumerable<IRuleFilterApplier> Filters => ruleFilters;

        public RuleSetSubsetGenerator(RuleSetSubset rootRuleSet)
        {
            ruleFilters = new List<IRuleFilterApplier>();
            this.rootRuleSet = rootRuleSet;
        }

        public void RemoveFilterApplier(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilterApplier(IRuleFilterApplier ruleFilter)
        {
            if (ruleFilter != null)
            {
                ruleFilters.Add(ruleFilter);
            }
        }

        public void GenerateSubsets()
        {
            //Create stack that holds collection of rule sets 
            //for actual iteration
            List<RuleSetSubset> actualSubsetLevel = new List<RuleSetSubset>();

            //Create list that holds parent rule sets
            //for next iteration
            List<RuleSetSubset> nextIterationParents = new List<RuleSetSubset>
            {
                rootRuleSet
            };

            foreach (IRuleFilterApplier seriesFilter in ruleFilters)
            {
                //Collection of parent rule setes are copied to actual                 
                //processed level collection
                if (nextIterationParents.Any())
                {
                    actualSubsetLevel = new List<RuleSetSubset>(nextIterationParents.ToList());
                }

                //Clear rule set parents for next level
                nextIterationParents.Clear();

                foreach (var actualRuleSet in actualSubsetLevel)
                {
                    RuleSetSubset[] childRuleSets = seriesFilter.ApplyFilterSeries(actualRuleSet);
                    nextIterationParents.AddRange(childRuleSets);
                }
            }
        }
    }
}
