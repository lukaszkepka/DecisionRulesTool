using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
    using Model;
    using RuleFilters;

    public class RuleSetSubset : RuleSet
    {
        private RuleSetSubset rootRuleSet;
        private List<IRuleFilter> ruleFilters;

        public RuleSetSubset RootRuleSet
        {
            get
            {
                return rootRuleSet ?? (this);
            }
            private set
            {
                rootRuleSet = value;
            }
        }
        public RuleSetSubset InitialRuleSet { get; private set; }
        public IList<RuleSetSubset> Subsets { get; private set; }
        public IEnumerable<IRuleFilter> Filters => ruleFilters;

        //
        public IList<IRuleFilterApplier> FilterAppliers;

        public override string Name { get; set; }

        public string DisplayAs
        {
            get
            {
                return $"{Name}: {Filters.LastOrDefault()?.ToString()} [{Rules.Count}]";
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

            if (FilterAppliers == null)
            {
                FilterAppliers = new List<IRuleFilterApplier>();
            }

        }

        public RuleSetSubset(string name) : base(name)
        {
            Initialize();
        }

        public RuleSetSubset(RuleSet ruleSet) : base(ruleSet.Name, ruleSet.Attributes, ruleSet.Rules, ruleSet.DecisionAttribute)
        {
            Initialize();
            RootRuleSet = this;
        }

        public RuleSetSubset(RuleSetSubset initialRuleSet) : this(initialRuleSet, initialRuleSet.RootRuleSet)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
            RootRuleSet = initialRuleSet.RootRuleSet;

            this.ruleFilters.AddRange(initialRuleSet.Filters);
        }

        public RuleSetSubset(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet) : base(initialRuleSet.Name, initialRuleSet.Attributes, initialRuleSet.Rules, initialRuleSet.DecisionAttribute)
        {
            Initialize();
            InitialRuleSet = initialRuleSet;
            RootRuleSet = rootRuleSet;
        }

        public void RemoveFilter(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilter(IRuleFilter ruleFilter)
        {
            ruleFilters.Add(ruleFilter);
        }

        public void ApplyFilters()
        {
            RuleSet filteredRuleSet = null;

            foreach (IRuleFilter filter in ruleFilters)
            {
                filteredRuleSet = filter.FilterRules(this);
                if (filteredRuleSet.Rules.Count == 0)
                {
                    break;
                }
            }

            this.Attributes = filteredRuleSet.Attributes;
            this.Rules = filteredRuleSet.Rules;
        }

        public override object Clone()
        {
            return new RuleSetSubset(InitialRuleSet, RootRuleSet)
            {
                Rules = this.Rules,
                Subsets = this.Subsets,
                ruleFilters = this.ruleFilters,
            };
        }
    }
}
