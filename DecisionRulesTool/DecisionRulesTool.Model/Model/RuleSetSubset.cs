using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    using Model;
    using PropertyChanged;
    using RuleFilters;

    public class RuleSetSubset : RuleSet
    {
        private IList<IRuleFilter> ruleFilters;

        public RuleSet RootRuleSet { get; private set; }
        public RuleSet InitialRuleSet { get; private set; }
        public IList<RuleSetSubset> Subsets { get; private set; }
        public IEnumerable<IRuleFilter> Filters => ruleFilters;

        public void Initialize()
        {
            ruleFilters = new List<IRuleFilter>();
            Subsets = new List<RuleSetSubset>();
        }

        public RuleSetSubset(string name) : base(name)
        {
            Initialize();
        }

        public RuleSetSubset(RuleSetSubset initialRuleSet) : base(initialRuleSet.Name, initialRuleSet.Attributes, initialRuleSet.Rules, initialRuleSet.DecisionAttribute)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
            RootRuleSet = initialRuleSet.RootRuleSet;
        }

        public RuleSetSubset(RuleSet ruleSet, RuleSet rootRuleSet) : base(ruleSet.Name, ruleSet.Attributes, ruleSet.Rules, ruleSet.DecisionAttribute)
        {
            Initialize();
            InitialRuleSet = ruleSet;
            RootRuleSet = rootRuleSet;
        }

        public void ApplyFilters()
        {
            var ruleSetToFilter = (RuleSet)InitialRuleSet.Clone();

            foreach (IRuleFilter filter in ruleFilters)
            {
                ruleSetToFilter = filter.FilterRules(ruleSetToFilter);
                if (ruleSetToFilter.Rules.Count == 0)
                {
                    break;
                }
            }

            this.Attributes = ruleSetToFilter.Attributes;
            this.Rules = ruleSetToFilter.Rules;
        }


        public void RemoveFilter(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilter(IRuleFilter ruleFilter)
        {
            ruleFilters.Add(ruleFilter);
        }
    }
}
