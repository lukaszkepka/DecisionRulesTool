using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class RuleSetSubsetGenerator : IRuleSubsetGenerator
    {
        private IList<IRuleSeriesFilter> ruleFilters;
        //private IProgressNotifier progressNotifier;
        private RuleSetSubset rootRuleSet;

        public IEnumerable<IRuleSeriesFilter> Filters => ruleFilters;

        public RuleSetSubsetGenerator(RuleSetSubset rootRuleSet)
        {
            ruleFilters = new List<IRuleSeriesFilter>();
            this.rootRuleSet = rootRuleSet;
        }

        public void RemoveFilter(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilter(IRuleSeriesFilter ruleFilter)
        {
            ruleFilters.Add(ruleFilter);
        }

        public void GenerateSubsets()
        {
            //Create stack that holds collection of rule sets 
            //for actual iteration
            List<RuleSetSubset> actualSubsetLevel = new List<RuleSetSubset>();

            //Create stack that holds collection of parent rule sets
            //for next iteration
            Stack<RuleSetSubset> subsetParents = new Stack<RuleSetSubset>();
            subsetParents.Push(rootRuleSet);

            foreach (IRuleSeriesFilter seriesFilter in ruleFilters)
            {
                //TODO: Dont skip actual root when children nodes weren't created //Q: For qure you need that?


                //Collection of parent rule setes are copied to actual                 
                //processed level collection
                if(subsetParents.Any())
                {
                    actualSubsetLevel = new List<RuleSetSubset>(subsetParents.ToList());
                }
                
                //Clear stack for rule set parents for next level
                subsetParents.Clear();

                foreach (var actualRuleSet in actualSubsetLevel)
                {
                    //For every subset in actual level generate subsets using 
                    //series for one type of filter
                    foreach (var filter in seriesFilter.GenerateSeries())
                    {
                        //Use filter to generate new subset
                        RuleSetSubset subset = new RuleSetSubset(actualRuleSet, rootRuleSet);
                        subset.AddFilter(filter);
                        subset.ApplyFilters();

                        //Set new subset name 
                        SetSubsetName(subset);

                        //Attach new subset to parent
                        actualRuleSet.Subsets.Add(subset);
                        //Mark new subset as parent for next level
                        subsetParents.Push(subset);
                    }
                }
            }
        }

        public void SetSubsetName(RuleSetSubset subset)
        {
            subset.Name = $"{rootRuleSet.Name}: {subset.Filters.LastOrDefault()?.ToString()} ";
        }
    }
}

