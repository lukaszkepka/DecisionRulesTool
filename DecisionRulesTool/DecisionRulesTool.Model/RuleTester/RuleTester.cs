
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
    using System.Threading;
    using Utils;

    public class RuleTester : IRuleTester
    {
        private readonly IProgressNotifier progressNotifier;
        private readonly IConditionChecker conditionChecker;
        private readonly DecisionResolverFactory decisionResolverFactory;

        public RuleTester(IConditionChecker conditionChecker, IProgressNotifier progressNotifier)
        {
            this.progressNotifier = progressNotifier;
            this.conditionChecker = conditionChecker;
            decisionResolverFactory = new DecisionResolverFactory();
        }

        public RuleTester(IConditionChecker conditionChecker, IProgressNotifier progressNotifier, DecisionResolverFactory decisionResolverFactory) : this(conditionChecker, progressNotifier)
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

        private ClassificationResult[] HandleTestRequest(TestRequest testRequest)
        {
            IDecisionResolverStrategy decisionResolver = decisionResolverFactory.Instantiate(testRequest);
            for (int i = 0; i < testRequest.RuleSet.Rules.Count; i++)
            {
                Rule rule = testRequest.RuleSet.Rules[i];
                TestSingleRule(rule, testRequest.TestSet, decisionResolver);

                testRequest.Progress = (int)((i / (double)testRequest.RuleSet.Rules.Count) * 100);
                Thread.Sleep(1);
            }

            testRequest.Progress = 100;
            return decisionResolver.RunClassification();
        }

        public virtual IEnumerable<TestResult> RunTesting(IEnumerable<TestRequest> testRequests)
        {
            IList<TestResult> testResults = new List<TestResult>();
            foreach (TestRequest testRequest in testRequests)
            {
                ClassificationResult[] classificationResults = HandleTestRequest(testRequest);
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

