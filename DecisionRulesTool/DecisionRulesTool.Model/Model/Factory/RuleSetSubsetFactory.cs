using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model.Factory
{
    public class RuleSetSubsetFactory : IRuleSetSubsetFactory
    {
        public RuleSetSubset Create(RuleSet ruleSet)
        {
            return new RuleSetSubset(ruleSet);
        }

        public RuleSetSubset Create(RuleSetSubset initialRuleSet)
        {
            return new RuleSetSubset(initialRuleSet);
        }

        public RuleSetSubset Create(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet)
        {
            return new RuleSetSubset(initialRuleSet, rootRuleSet);
        }
    }
}
