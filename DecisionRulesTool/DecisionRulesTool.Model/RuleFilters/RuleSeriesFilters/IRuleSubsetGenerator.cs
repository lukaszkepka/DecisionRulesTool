using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters
{
    public interface IRuleSubsetGenerator
    {
        void SetSubsetName(RuleSetSubset subset);
        void RemoveFilter(int index);
        void AddFilter(IRuleFilterApplier ruleFilter);
        void GenerateSubsets();
    }
}