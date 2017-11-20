using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.Model.RuleTester
{
    using DecisionResolver;
    using Model;
    using Utils;

    public class MajorityVoting : BaseDecisionResolverStrategy
    {
        public override ConflictResolvingMethod ResolvingMethod { get; } = ConflictResolvingMethod.MajorityVoting;

        public MajorityVoting(DataSet testSet, Attribute decisionAttribute) : base(testSet, decisionAttribute)
        {
        }

        protected override string ResolveConflict(KeyValuePair<Object, int[]> decisionItem)
        {
            string decision = string.Empty;
            int maxValue = decisionItem.Value.Max();
            bool isDraw = decisionItem.Value.Count(x => x == maxValue) > 1 ? true : false;

            if (isDraw)
            {
                decision = ClassificationResult.Ambigious;
            }
            else
            {
                int maxIndex = decisionItem.Value.GetIndexOfMax();
                decision = decisionValues[maxIndex];
            }
            return decision;
        }
    }
}