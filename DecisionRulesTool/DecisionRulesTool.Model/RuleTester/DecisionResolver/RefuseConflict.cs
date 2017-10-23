using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleTester
{
    using DecisionResolver;
    using Model;
    public class RefuseConflict : BaseDecisionResolverStrategy
    {
        public override ConflictResolvingMethod ResolvingMethod { get; } = ConflictResolvingMethod.RefuseConflicts;

        public RefuseConflict(DataSet testSet, Attribute decisionAttribute) : base(testSet, decisionAttribute)
        {
        }

        protected override string ResolveConflict(KeyValuePair<Object, int[]> decisionItem)
        {
            return BadClassification;
        }
    }
}

