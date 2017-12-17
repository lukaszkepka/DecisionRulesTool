using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class RuleSetViewModel : RuleSetSubset
    {
        public bool IsSelected { get; set; }

        public bool IsExpanded { get; set; }


        public RuleSetViewModel(string name) : base(name)
        {
        }

        public RuleSetViewModel(RuleSet ruleSet) : base(ruleSet)
        {
        }

        public RuleSetViewModel(RuleSetSubset initialRuleSet) : base(initialRuleSet)
        {
        }

        public RuleSetViewModel(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet) : base(initialRuleSet, rootRuleSet)
        {
        }

        public override object Clone()
        {
            return new RuleSetViewModel(InitialRuleSet, RootRuleSet)
            {
                Rules = this.Rules,
                Subsets = this.Subsets,
                ruleFilters = this.ruleFilters,
            };
        }
    }
}
