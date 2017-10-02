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

        public RuleSet InitialRuleSet { get; private set; }
        public RuleSet FilteredRuleSet { get; private set; }

        public RuleFilterAggregator(RuleFilterAggregator ruleFilterAggregator)
        {

        }

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

        /// <summary>
        /// Applies filters on initial rule set. Initial rule set isn't modified
        /// </summary>
        /// <returns>New instance of rule set which is subset of initial rule set</returns>
        public RuleSet RunFiltering()
        {
            FilteredRuleSet = (RuleSet)InitialRuleSet.Clone();

            foreach (IRuleFilter filter in ruleFilters)
            {
                FilteredRuleSet = filter.FilterRules(FilteredRuleSet);
                if(FilteredRuleSet.Rules.Count == 0)
                {
                    break;
                }
            }

            return FilteredRuleSet;
        }

    }
}
