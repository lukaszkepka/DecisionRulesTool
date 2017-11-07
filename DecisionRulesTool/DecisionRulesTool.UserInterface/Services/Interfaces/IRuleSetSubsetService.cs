using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Services.Interfaces
{
    public interface IRuleSetSubsetService
    {
        void WalkSubsetTree(ICollection<RuleSetSubset> ruleSets, Action<RuleSetSubset> action);
        void SelectAllSubsets(ICollection<RuleSetSubset> ruleSets);
        void UnselectEmptySubsets(ICollection<RuleSetSubset> ruleSets);
    }
}
