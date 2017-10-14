using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public class AttributePresenceFilterApplier : BaseFilterApplier
    { 
        private AttributePresenceFilter.Mode mode;
        private string[] attributeNames;

        public AttributePresenceFilterApplier(AttributePresenceFilter.Mode mode, string[] attributeNames)
        {
            this.mode = mode;
            this.attributeNames = attributeNames;
        }


        public override IList<IRuleFilter> GenerateSeries()
        {
            return new List<IRuleFilter>() { new AttributePresenceFilter(mode, attributeNames) };
        }
    }
}
