using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester
{
    public class DecisionResolverFactory
    {
        public virtual IDecisionResolverStrategy Instantiate(TestObject testRequest)
        {
            switch (testRequest.ResolvingMethod)
            {
                case ConflictResolvingMethod.WeightedVoting:
                    return new WeightedVoting(testRequest.TestSet, testRequest.RuleSet.DecisionAttribute);
                case ConflictResolvingMethod.MajorityVoting:
                    return new MajorityVoting(testRequest.TestSet, testRequest.RuleSet.DecisionAttribute);
                case ConflictResolvingMethod.RefuseConflicts:
                    return new RefuseConflict(testRequest.TestSet, testRequest.RuleSet.DecisionAttribute);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
