using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters
{
    public abstract class BaseFilter : IRuleFilter
    {
        public abstract RuleSet FilterRules(RuleSet ruleSet);
    }
}
