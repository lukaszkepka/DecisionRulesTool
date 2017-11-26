using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel.Results
{
    public class GroupedRuleSetResult
    {
        public RuleSet RuleSet { get; set; }
        public ConflictResolvingMethod ConflictResolvingMethod { get; set; }

        public GroupedRuleSetResult(RuleSet RuleSet, ConflictResolvingMethod ConflictResolvingMethod)
        {
            this.RuleSet = RuleSet;
            this.ConflictResolvingMethod = ConflictResolvingMethod;
        }
    }
}
