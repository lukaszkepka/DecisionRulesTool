using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.AttributePresenceStrategy
{
    internal class StrictModeStrategy : IAttributePresenceStrategy
    {
        public bool CheckCondition(Rule rule, string[] attributeNames)
        {
            var attributesInRule = rule.Conditions.Select(x => x.Attribute.Name).Distinct();
            return attributesInRule.Intersect(attributeNames).Count() == attributeNames.Length;
        }
    }
}
