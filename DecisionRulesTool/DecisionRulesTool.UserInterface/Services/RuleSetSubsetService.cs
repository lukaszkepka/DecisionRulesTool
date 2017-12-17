using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Services
{
    public class RuleSetSubsetService : IRuleSetSubsetService
    {
        public void WalkSubsetTree(ICollection<RuleSetSubset> ruleSets, Action<RuleSetSubset> action)
        {
            if (ruleSets.Any())
            {
                foreach (var ruleSet in ruleSets)
                {
                    action(ruleSet);
                    WalkSubsetTree(ruleSet.Subsets, action);
                }
            }
        }

        public void SelectAllSubsets(ICollection<RuleSetSubset> ruleSets)
        {
            WalkSubsetTree(ruleSets, (ruleSet) =>
            {
                if (ruleSet is RuleSetViewModel selectableRuleSet)
                {
                    selectableRuleSet.IsSelected = true;
                }
            });
        }

        public void UnselectEmptySubsets(ICollection<RuleSetSubset> ruleSets)
        {
            WalkSubsetTree(ruleSets, (ruleSet) =>
            {
                if (ruleSet is RuleSetViewModel selectableRuleSet)
                {
                    if (!ruleSet.Rules.Any())
                    {
                        selectableRuleSet.IsSelected = false;
                    }
                }
            });
        }
    }
}
