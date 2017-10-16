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
            IList<IRuleFilter> ruleFilters = new List<IRuleFilter>();
            List<string> actualAttributes = new List<string>();
            for (int i = 0; i < attributeNames.Length; i++)
            {
                actualAttributes.Add(attributeNames[i]);
                ruleFilters.Add(new AttributePresenceFilter(mode, actualAttributes.ToArray()));
            }

            return ruleFilters;
        }
    }
}
