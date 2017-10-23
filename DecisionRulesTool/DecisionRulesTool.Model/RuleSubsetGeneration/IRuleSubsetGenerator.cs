using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.Appliers;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public interface IRuleSubsetGenerator
    {
        void RemoveFilterApplier(int index);
        void AddFilterApplier(IRuleFilterApplier ruleFilter);
        void GenerateSubsets();
    }
}