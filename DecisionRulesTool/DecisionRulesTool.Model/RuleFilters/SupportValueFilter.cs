using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Comparers;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class SupportValueFilter : IRuleFilter
    {
        private Relation relationBetweenRulesLengths;
        private IAttributeValuesComparer ruleLengthComparer;
        private int desiredValue;

        /// <summary>
        /// Constructor for support value filter
        /// </summary>
        /// <param name="relation">Describes relation between desired support value and actual rule's support value</param>
        /// <param name="desiredValue">Desired support value</param>
        /// <param name="ruleLengthComparer">Provides comparision between numbers based on relation. IntegerAttributeComparer by default</param>
        public SupportValueFilter(Relation relation, int desiredValue, IAttributeValuesComparer ruleLengthComparer = null)
        {
            this.relationBetweenRulesLengths = relation;
            this.desiredValue = desiredValue;
            if (ruleLengthComparer == null)
            {
                this.ruleLengthComparer = new IntegerAttributeComparer();
            }
        }

        public bool CheckCondition(Rule rule)
        {
            return ruleLengthComparer.AreInRelation(relationBetweenRulesLengths, rule.SupportValue, desiredValue);
        }

        public RuleSet FilterRules(RuleSet ruleSet)
        {
            RuleSet newRuleSet = new RuleSet(ruleSet.Name, ruleSet.Attributes, new List<Rule>(), ruleSet.DecisionAttribute);
            foreach (Rule rule in ruleSet.Rules)
            {
                if (CheckCondition(rule))
                {
                    Rule newRule = (Rule)rule.Clone();
                    newRule.RuleSet = newRuleSet;
                    newRuleSet.Rules.Add(newRule);
                }
            }
            return newRuleSet;
        }
    }
}
