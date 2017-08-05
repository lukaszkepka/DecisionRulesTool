using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters
{
    public class RuleFilterAggreagtor
    {
        IEnumerable<IRuleFilter> Filters { get; set; }
        RuleSet InitialRuleSet { get; set; }
        RuleSet ResultRuleSet { get; set; }
    }
}
