using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public abstract class BaseRuleSetSubsetGenerator : IRuleSubsetGenerator
    {
        protected IList<IRuleFilterApplier> ruleFilters;
        protected RuleSetSubset rootRuleSet;

        public IEnumerable<IRuleFilterApplier> Filters => ruleFilters;

        public BaseRuleSetSubsetGenerator(RuleSetSubset rootRuleSet)
        {
            ruleFilters = new List<IRuleFilterApplier>();
            this.rootRuleSet = rootRuleSet;
        }

        public void RemoveFilterApplier(int index)
        {
            ruleFilters.RemoveAt(index);
        }

        public void AddFilterApplier(IRuleFilterApplier ruleFilter)
        {
            if (ruleFilter != null)
            {
                ruleFilters.Add(ruleFilter);
            }
        }

        public abstract void GenerateSubsets();

    }
}
