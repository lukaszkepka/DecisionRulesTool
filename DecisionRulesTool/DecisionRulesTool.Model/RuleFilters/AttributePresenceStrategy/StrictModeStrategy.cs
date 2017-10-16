using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.AttributePresenceStrategy
{
    internal class ExactModeStrategy : IAttributePresenceStrategy
    {
        public bool CheckCondition(Rule rule, string[] attributeNames)
        {
            var attributesInRule = rule.Conditions.Select(x => x.Attribute.Name).Distinct();
            return attributesInRule.Count() == attributeNames.Length && attributesInRule.Intersect(attributeNames).Count() == attributeNames.Length;
        }
    }
}
