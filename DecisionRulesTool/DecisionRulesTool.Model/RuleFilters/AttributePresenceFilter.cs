using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class AttributePresenceFilter : IRuleFilter
    {
        private string[] attributeNames;

        public AttributePresenceFilter(params string[] attributeNames)
        {
            this.attributeNames = attributeNames;
        }

        public bool CheckCondition(Rule rule, string attributeName)
        {
            return rule.Conditions.Any(x => x.Attribute.Name.Equals(attributeName));
        }

        public RuleSet FilterRules(RuleSet ruleSet)
        {
            RuleSet newRuleSet = new RuleSet(ruleSet.Name, ruleSet.Attributes, new List<Rule>(), ruleSet.DecisionAttribute);
            foreach (Rule rule in ruleSet.Rules)
            {
                bool ruleSatisfiesCondition = true;
                foreach(string attributeName in attributeNames)
                {
                    if(!CheckCondition(rule, attributeName))
                    {
                        ruleSatisfiesCondition = false;
                        break;
                    }
                }
                if(ruleSatisfiesCondition)
                {
                    newRuleSet.Rules.Add(rule);
                }
            }
            return newRuleSet;
        }
    }
}
