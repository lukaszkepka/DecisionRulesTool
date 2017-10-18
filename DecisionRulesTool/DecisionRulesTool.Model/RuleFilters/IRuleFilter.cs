using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters
{
    public interface IRuleFilter
    {
        /// <summary>
        /// Creates new instance of rule set with 
        /// rules reduced by filter. 
        /// </summary>
        /// <param name="ruleSet">Initial rule set</param>
        /// <returns>Filtered rule set</returns>
        RuleSet FilterRules(RuleSet ruleSet);
    }
}
