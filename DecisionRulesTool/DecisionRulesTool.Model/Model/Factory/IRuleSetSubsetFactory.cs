using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model.Factory
{
    public interface IRuleSetSubsetFactory
    {

        /// <summary>
        /// Method for creating RuleSetSubset instance and filling basic RuleSet class fields
        /// </summary>
        RuleSetSubset Create(RuleSet ruleSet);

        /// <summary>
        /// Method for creating RuleSetSubset instance as child of initialRuleSet.
        /// Field InitialSubset is set to initialRuleSet and RootRuleSet is
        /// set to initialRuleSet rootRuleSet
        /// </summary>
        RuleSetSubset Create(RuleSetSubset initialRuleSet);

        /// <summary>
        /// Method for creating RuleSetSubset instance as child of initialRuleSet.
        /// Field InitialSubset is set to initialRuleSet and RootRuleSet is
        /// set to rootRuleSet
        /// </summary>
        RuleSetSubset Create(RuleSetSubset initialRuleSet, RuleSetSubset rootRuleSet);
    }
}
