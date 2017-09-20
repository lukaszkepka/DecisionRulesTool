
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionRulesTool.Model.RuleTester
{
    using Model;
    using Utils;

    public class RuleTester : IRuleTester
    {
        private readonly IConditionChecker conditionChecker;
        private readonly DecisionResolverFactory decisionResolverFactory;

        public RuleTester(IConditionChecker conditionChecker)
        {
            this.conditionChecker = conditionChecker;
            decisionResolverFactory = new DecisionResolverFactory();
        }

        public RuleTester(IConditionChecker conditionChecker, DecisionResolverFactory decisionResolverFactory) : this(conditionChecker)
        {
            this.conditionChecker = conditionChecker;
            this.decisionResolverFactory = decisionResolverFactory;
        }

        private void TestSingleRule(Rule rule, DataSet testSet, IDecisionResolverStrategy decisionResolver)
        {
            foreach (Object dataObject in testSet.Objects)
            {
                bool allConditionsSatisfiedSoFar = true;
                IEnumerator conditionEnumerator = rule.Conditions.GetEnumerator();
                while (conditionEnumerator.MoveNext() && allConditionsSatisfiedSoFar)
                {
                    Condition condition = (Condition)conditionEnumerator.Current;
                    allConditionsSatisfiedSoFar = conditionChecker.IsConditionSatisfied(condition, dataObject);
                }

                if (allConditionsSatisfiedSoFar)
                {
                    decisionResolver.AddDecision(dataObject, rule);
                }
            }
        }

        private ConfusionMatrix ComputeConfusionMatrix(ClassificationResult[] decisionValues, DataSet testSet)
        {
            return new ConfusionMatrix();
        }

        private ClassificationResult[] GetDecisions(TestRequest testRequest)
        {
            IDecisionResolverStrategy decisionResolver = decisionResolverFactory.Instantiate(testRequest);
            foreach (Rule rule in testRequest.RuleSet.Rules)
            {
                TestSingleRule(rule, testRequest.TestSet, decisionResolver);
            }

            return decisionResolver.RunClassification();
        }

        public virtual IEnumerable<TestResult> RunTesting(IEnumerable<TestRequest> testRequests)
        {
            IList<TestResult> testResults = new List<TestResult>();
            foreach (TestRequest testRequest in testRequests)
            {
                ClassificationResult[] classificationResults = GetDecisions(testRequest);
                ConfusionMatrix confusionMatrix = ComputeConfusionMatrix(classificationResults, testRequest.TestSet);
                TestResult testResult = new TestResult()
                {
                    DecisionValues = classificationResults.Select(x => x.DecisionValue).ToArray(),
                    ClassificationResults = classificationResults.Select(x => x.Result).ToArray(),
                    ConfusionMatrix = confusionMatrix,
                    TestRequest = testRequest,
                };

                testResults.Add(testResult);
            }
            return testResults;
        }

    }
}

