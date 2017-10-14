using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.AttributePresenceStrategy
{
    internal class DefaultAttributeStrategy : IAttributePresenceStrategy
    {
        public bool CheckCondition(Rule rule, string[] attributeName)
        {
            return false;
        }
    }
}
