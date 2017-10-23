using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model.Factory
{
    public class RuleSetSubsetViewItemFactory : IRuleSetSubsetFactory
    {
        public RuleSetSubset Create(RuleSet ruleSet)
        {
            return new RuleSetSubsetViewItem(ruleSet);
        }

        public RuleSetSubset Create(RuleSetSubset initialRuleSet)
        {
            return new RuleSetSubsetViewItem(initialRuleSet);
        }

        public RuleSetSubset Create(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet)
        {
            return new RuleSetSubsetViewItem(initialRuleSet, rootRuleSet);
        }
    }
}
