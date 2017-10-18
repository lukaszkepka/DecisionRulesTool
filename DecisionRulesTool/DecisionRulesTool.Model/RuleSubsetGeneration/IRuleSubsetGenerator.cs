using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public interface IRuleSubsetGenerator
    {
        void RemoveFilterApplier(int index);
        void AddFilterApplier(IRuleFilterApplier ruleFilter);
        void GenerateSubsets();
    }
}