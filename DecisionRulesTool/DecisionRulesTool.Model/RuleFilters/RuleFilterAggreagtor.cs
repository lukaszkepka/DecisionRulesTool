using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class RuleFilterAggregator
    {
        private IList<IRuleFilter> ruleFilters;
        public IEnumerable<IRuleFilter> Filters => ruleFilters;
        public RuleSet InitialRuleSet { get; }

        public RuleFilterAggregator(RuleSet initialRuleSet)
        {
            InitialRuleSet = initialRuleSet;
            ruleFilters = new List<IRuleFilter>();
        }

        public RuleFilterAggregator(RuleSet initialRuleSet, IEnumerable<IRuleFilter> filters) : this(initialRuleSet)
        {
            ruleFilters = new List<IRuleFilter>(filters);
        }

        public void RemoveFilter(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilter(IRuleFilter ruleFilter)
        {
            ruleFilters.Add(ruleFilter);
        }


        public RuleSet RunFiltering()
        {
            RuleSet filteredRuleSet = (RuleSet)InitialRuleSet.Clone();

            foreach (IRuleFilter filter in ruleFilters)
            {
                filteredRuleSet = filter.FilterRules(filteredRuleSet);
                if(filteredRuleSet.Rules.Count == 0)
                {
                    break;
                }
            }

            return filteredRuleSet;
        }

    }
}
