using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model.Factory
{
    public class RuleSetViewModelFactory : IRuleSetSubsetFactory
    {
        public RuleSetSubset Create(RuleSet ruleSet)
        {
            return new RuleSetViewModel(ruleSet);
        }

        public RuleSetSubset Create(RuleSetSubset initialRuleSet)
        {
            return new RuleSetViewModel(initialRuleSet);
        }

        public RuleSetSubset Create(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet)
        {
            return new RuleSetViewModel(initialRuleSet, rootRuleSet);
        }
    }
}
