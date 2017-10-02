using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class RuleSetSubsetGenerator
    {
        private IList<IRuleSeriesFilter> ruleFilters;
        private RuleSetSubset rootRuleSet;

        public IEnumerable<IRuleSeriesFilter> Filters => ruleFilters;

        public RuleSetSubsetGenerator(RuleSetSubset rootRuleSet)
        {
            ruleFilters = new List<IRuleSeriesFilter>() { new LengthSeriesFilter(1, 3, new LengthFilter(Relation.Equality, 0))};
            this.rootRuleSet = rootRuleSet;
        }

        public void a()
        {
            Stack<RuleSetSubset> b = new Stack<RuleSetSubset>();
            b.Push(rootRuleSet);

            foreach (IRuleSeriesFilter seriesFilter in ruleFilters)
            {
                var gg = new List<RuleSetSubset>(b.ToList());
                b.Clear();
                foreach (var item in gg)
                {
                    foreach (var filter in seriesFilter.GenerateSeries())
                    {
                        var newRuleSet = filter.FilterRules(item);

                        RuleSetSubset subset = new RuleSetSubset(newRuleSet, rootRuleSet);
                        item.Subsets.Add(subset);
                        b.Push(subset);
                    }
                }
            }
        }





        //IList<RuleFilterAggregator> h = new List<RuleFilterAggregator>();
        //for (int i = MinLength; i < MaxLength; i++)
        //{
        //    RuleFilterAggregator r = new RuleFilterAggregator(ruleFilterAggregator.InitialRuleSet, ruleFilterAggregator.Filters);
        //    IRuleFilter ruleFilter = new LengthFilter(initialFilter.RelationBetweenRulesLengths, i);
        //    r.AddFilter(ruleFilter);

        //    h.Add(r);
        //}
        //return h;
    }
}

