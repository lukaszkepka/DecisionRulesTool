using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionRulesTool.Model.RuleTester
{
    using Model;
    public interface IDecisionResolverStrategy
    {
        ConflictResolvingMethod ResolvingMethod { get; }

        void AddDecision(Object dataObject, Rule rule);
        ClassificationResult[] RunClassification();
    }
}
