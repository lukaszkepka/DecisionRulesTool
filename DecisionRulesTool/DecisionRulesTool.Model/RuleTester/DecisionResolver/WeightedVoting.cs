using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.RuleTester
{
    using DecisionResolver;
    using Model;
    using Utils;

    public class WeightedVoting : MajorityVoting
    {
        public override ConflictResolvingMethod ResolvingMethod { get; } = ConflictResolvingMethod.WeightedVoting;

        public WeightedVoting(DataSet testSet, Attribute decisionAttribute) : base(testSet, decisionAttribute)
        {
        }

        public override void AddDecision(Object dataObject, Rule rule)
        {
            foreach (Decision decision in rule.Decisions)
            {
                int index = GetDecisionValueIndex(decision);
                decisionMatrix[dataObject][index] += decision.Support;
            }
        }
    }
}

