using DecisionRulesTool.Model.Utils;
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
            CombinationGenerator combinationGenerator = new CombinationGenerator();
            IEnumerable<string[]> attributeSubsets = combinationGenerator.GenerateAllSubsets(attributeNames).OrderBy(x => x.Length);

            //Exclude first element, which is empty subset
            attributeSubsets = attributeSubsets.Skip(1);

            foreach (string[] attributeSubset in attributeSubsets)
            {
                ruleFilters.Add(new AttributePresenceFilter(mode, attributeSubset));
            }

            return ruleFilters;
        }
    }
}
