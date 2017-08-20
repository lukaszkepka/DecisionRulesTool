using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Comparers;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class LengthFilter : BaseFilter
    {
        private Relation relationBetweenRulesLengths;
        private IAttributeValuesComparer ruleLengthComparer;
        private int desiredLength;

        /// <summary>
        /// Constructor for length filter
        /// </summary>
        /// <param name="relation">Describes relation between desired rule length and actual rule length</param>
        /// <param name="desiredLength">Desired rule length</param>
        /// <param name="ruleLengthComparer">Provides comparision between numbers based on relation. IntegerAttributeComparer by default</param>
        public LengthFilter(Relation relation, int desiredLength, IAttributeValuesComparer ruleLengthComparer = null)
        {
            this.relationBetweenRulesLengths = relation;
            this.desiredLength = desiredLength;
            if(ruleLengthComparer == null)
            {
                this.ruleLengthComparer = new IntegerAttributeComparer();
            }
        }

        public bool CheckCondition(Rule rule)
        {
            return ruleLengthComparer.AreInRelation(relationBetweenRulesLengths, rule.Conditions.Count, desiredLength);
        }

        public override RuleSet FilterRules(RuleSet ruleSet)
        {
            RuleSet newRuleSet = new RuleSet(ruleSet.Name, ruleSet.Attributes, new List<Rule>(), ruleSet.DecisionAttribute);
            foreach (Rule rule in ruleSet.Rules)
            {
                if(CheckCondition(rule))
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
