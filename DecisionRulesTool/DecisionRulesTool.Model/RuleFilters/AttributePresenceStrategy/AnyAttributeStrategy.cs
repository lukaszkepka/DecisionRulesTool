using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.AttributePresenceStrategy
{
    internal class AnyAttributeStrategy : IAttributePresenceStrategy
    {
        public bool CheckCondition(Rule rule, string[] attributeNames)
        {
            bool result = false;
            foreach (var attributeName in attributeNames)
            {
                if(rule.Conditions.Any(x => x.Attribute.Name.Equals(attributeName)))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
