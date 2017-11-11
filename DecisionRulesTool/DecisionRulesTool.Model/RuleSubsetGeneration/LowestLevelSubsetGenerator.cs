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
    public class LowestLevelSubsetGenerator : BaseRuleSetSubsetGenerator
    {
        public LowestLevelSubsetGenerator(RuleSetSubset rootRuleSet) : base(rootRuleSet)
        {
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
                    IEnumerable<RuleSetSubset> ruleSetSubsetToAttach = seriesFilter.ApplyFilterSeries(actualRuleSet);
                    nextIterationParents.AddRange(ruleSetSubsetToAttach);
                }
            }
        }
    }
}
