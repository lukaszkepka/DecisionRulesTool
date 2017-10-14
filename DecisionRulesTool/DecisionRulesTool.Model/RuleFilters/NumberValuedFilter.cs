using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Comparers;

namespace DecisionRulesTool.Model.RuleFilters
{
    public abstract class ValueBasedFilter : IRuleFilter
    {
        protected Relation Relation { get; }
        protected IAttributeValuesComparer RuleAttributeComparer { get; }
        public int DesiredValue { get; }

        public ValueBasedFilter(Relation relation, int desiredLength, IAttributeValuesComparer ruleLengthComparer = null)
        {
            this.Relation = relation;
            this.DesiredValue = desiredLength;
            if (ruleLengthComparer == null)
            {
                this.RuleAttributeComparer = new IntegerAttributeComparer();
            }
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

        public abstract bool CheckCondition(Rule rule);
    }
}
