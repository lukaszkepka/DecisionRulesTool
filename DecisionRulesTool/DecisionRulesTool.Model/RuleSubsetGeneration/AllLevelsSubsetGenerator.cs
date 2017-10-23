using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.Appliers;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleSubsetGeneration
{
    public class AllLevelsSubsetGenerator : BaseRuleSetSubsetGenerator
    {
        public AllLevelsSubsetGenerator(RuleSetSubset rootRuleSet) : base(rootRuleSet)
        {
        }

        public void InnerFuncton(RuleSetSubset rootRuleSet, IRuleFilterApplier seriesFilter)
        {
            if (rootRuleSet.Subsets.Any())
            {
                foreach (var item in rootRuleSet.Subsets)
                {
                    InnerFuncton(item, seriesFilter);
                    //seriesFilter.ApplyFilterSeries(item);
                }
                seriesFilter.ApplyFilterSeries(rootRuleSet);
            }
            else
            {
                seriesFilter.ApplyFilterSeries(rootRuleSet);
            }
        }

        public override void GenerateSubsets()
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
                InnerFuncton(rootRuleSet, seriesFilter);
            }
        }
    }
}
