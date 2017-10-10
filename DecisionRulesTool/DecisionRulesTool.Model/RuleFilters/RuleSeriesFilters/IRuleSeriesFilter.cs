using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public interface IRuleFilterApplier
    {
        RuleSetSubset[] ApplyFilterSeries(RuleSetSubset ruleSet);
       // IList<IRuleFilter> GenerateSeries();
    }
}
