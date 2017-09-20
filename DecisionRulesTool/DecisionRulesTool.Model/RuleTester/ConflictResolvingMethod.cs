using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionRulesTool.Model.RuleTester
{
    public enum ConflictResolvingMethod
    {
        WeightedVoting,
        MajorityVoting,
        RefuseConflicts,
    }
}