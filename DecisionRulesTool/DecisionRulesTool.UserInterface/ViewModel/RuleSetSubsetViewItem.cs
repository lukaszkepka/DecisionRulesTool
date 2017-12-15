using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class RuleSetSubsetViewItem : RuleSetSubset
    {
        public bool IsSelected { get; set; }

        public bool IsExpanded { get; set; }


        public RuleSetSubsetViewItem(string name) : base(name)
        {
        }

        public RuleSetSubsetViewItem(RuleSet ruleSet) : base(ruleSet)
        {
        }

        public RuleSetSubsetViewItem(RuleSetSubset initialRuleSet) : base(initialRuleSet)
        {
        }

        public RuleSetSubsetViewItem(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet) : base(initialRuleSet, rootRuleSet)
        {
        }

        public override object Clone()
        {
            return new RuleSetSubsetViewItem(InitialRuleSet, RootRuleSet)
            {
                Rules = this.Rules,
                Subsets = this.Subsets,
                ruleFilters = this.ruleFilters,
            };
        }
    }
}
