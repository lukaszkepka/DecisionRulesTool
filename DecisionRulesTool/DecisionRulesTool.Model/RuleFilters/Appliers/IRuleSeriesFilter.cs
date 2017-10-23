using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.Appliers
{
    public interface IRuleFilterApplier
    {
        /// <summary>
        /// Applies filters to rule sets tree
        /// </summary>
        /// <param name="ruleSet">Initial rule set for applying filters</param>
        /// <returns>Generated rule sets</returns>
        RuleSetSubset[] ApplyFilterSeries(RuleSetSubset ruleSet);
        RuleSetSubset ApplySingleFilter(IRuleFilter filter, RuleSetSubset ruleSet);
    }
}
