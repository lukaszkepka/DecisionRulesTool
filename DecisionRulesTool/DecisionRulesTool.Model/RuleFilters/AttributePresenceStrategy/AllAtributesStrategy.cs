using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.AttributePresenceStrategy
{
    internal class AllAtributesStrategy : IAttributePresenceStrategy
    {
        public bool CheckCondition(Rule rule, string[] attributeNames)
        {
            bool result = true;
            foreach (var attributeName in attributeNames)
            {
                if(!rule.Conditions.Any(x => x.Attribute.Name.Equals(attributeName)))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}
