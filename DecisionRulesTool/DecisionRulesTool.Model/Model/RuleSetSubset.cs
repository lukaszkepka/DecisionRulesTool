using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    using Model;
    using RuleFilters;

    public class RuleSetSubset : RuleSet
    {
        private string name;
        private List<IRuleFilter> ruleFilters;

        public RuleSet RootRuleSet { get; private set; }
        public RuleSet InitialRuleSet { get; private set; }
        public IList<RuleSetSubset> Subsets { get; private set; }
        public IEnumerable<IRuleFilter> Filters => ruleFilters;

        public override string Name
        {
            get
            {
                return $"{RootRuleSet.Name}: {Filters.LastOrDefault()?.ToString()} [{Rules.Count}]";
            }
            set
            {
                name = value;
            }
        }

        public void Initialize()
        {
            if (ruleFilters == null)
            {
                ruleFilters = new List<IRuleFilter>();
            }

            if (Subsets == null)
            {
                Subsets = new List<RuleSetSubset>();
            }
        }

        public RuleSetSubset(string name) : base(name)
        {
            Initialize();
        }

        public RuleSetSubset(RuleSetSubset initialRuleSet) : this(initialRuleSet, initialRuleSet.RootRuleSet)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
            RootRuleSet = initialRuleSet.RootRuleSet;

            this.ruleFilters.AddRange(initialRuleSet.Filters);
        }

        public RuleSetSubset(RuleSet initialRuleSet, RuleSet rootRuleSet) : base(initialRuleSet.Name, initialRuleSet.Attributes, initialRuleSet.Rules, initialRuleSet.DecisionAttribute)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
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
