using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class LengthFilter : IRuleFilter
    {
        protected Relation RelationBetweenRulesLengths { get; }
        protected IAttributeValuesComparer RuleLengthComparer { get; }
        public int DesiredLength { get; }

        /// <summary>
        /// Constructor for length filter
        /// </summary>
        /// <param name="relation">Describes relation between desired rule length and actual rule length</param>
        /// <param name="desiredLength">Desired rule length</param>
        /// <param name="ruleLengthComparer">Provides comparision between numbers based on relation. IntegerAttributeComparer by default</param>
        public LengthFilter(Relation relation, int desiredLength, IAttributeValuesComparer ruleLengthComparer = null)
        {
            this.RelationBetweenRulesLengths = relation;
            this.DesiredLength = desiredLength;
            if(ruleLengthComparer == null)
            {
                this.RuleLengthComparer = new IntegerAttributeComparer();
            }
        }

        public bool CheckCondition(Rule rule)
        {
            return RuleLengthComparer.AreInRelation(RelationBetweenRulesLengths, rule.Conditions.Count, DesiredLength);
        }

        public RuleSet FilterRules(RuleSet ruleSet)
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

        public override string ToString()
        {
            return $"Length {Tools.GetRelationString(RelationBetweenRulesLengths)} {DesiredLength}";
        }
    }
}
