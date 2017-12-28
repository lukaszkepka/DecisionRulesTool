using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.Appliers;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public abstract class BaseRuleSetSubsetGenerator : IRuleSubsetGenerator
    {
        protected IList<IRuleFilterApplier> filterAppliers;
        protected RuleSetSubset rootRuleSet;

        public IEnumerable<IRuleFilterApplier> Filters => filterAppliers;

        public BaseRuleSetSubsetGenerator(RuleSetSubset rootRuleSet)
        {
            filterAppliers = new List<IRuleFilterApplier>();
            this.rootRuleSet = rootRuleSet;
        }

        public void RemoveFilterApplier(int index)
        {
            filterAppliers.RemoveAt(index);
        }

        public void AddFilterApplier(IRuleFilterApplier ruleFilter)
        {
            if (ruleFilter != null)
            {
                filterAppliers.Add(ruleFilter);
            }
        }

        public abstract void GenerateSubsets();

    }
}
