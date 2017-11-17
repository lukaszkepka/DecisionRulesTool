﻿
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

        private ConfusionMatrix ComputeConfusionMatrix(ClassificationResult[] decisionValues, DataSet testSet, Attribute decisionAttribute)
        {
            int i = 0;
            ConfusionMatrix confusionMatrix = new ConfusionMatrix(decisionAttribute);

            foreach (var testSetObject in testSet.Objects)
            {
                string realClass = testSetObject[decisionAttribute].ToString();
                string predictedClass = decisionValues[i++].DecisionValue;
                confusionMatrix.IncrementPredictionCount(realClass, predictedClass);
            }

            return confusionMatrix;
        }

        private ClassificationResult[] HandleTestRequest(TestRequest testRequest)
        {
            IDecisionResolverStrategy decisionResolver = decisionResolverFactory.Instantiate(testRequest);
            for (int i = 0; i < testRequest.RuleSet.Rules.Count; i++)
            {
                Rule rule = testRequest.RuleSet.Rules[i];
                TestSingleRule(rule, testRequest.TestSet, decisionResolver);

                testRequest.Progress = GetTesterProgress(i, testRequest.RuleSet.Rules.Count);
            }

            testRequest.Progress = TestRequest.MaxProgress;
            return decisionResolver.RunClassification();
        }

        public virtual IEnumerable<TestResult> RunTesting(IEnumerable<TestRequest> testRequests)
        {
            int completedTestRequestCount = 0;
            int testRequestsCount = testRequests.Count();
            //Send notification that process has started
            progressNotifier.OnStart();

            IList<TestResult> testResults = new List<TestResult>();
            foreach (TestRequest testRequest in testRequests)
            {
                if (testRequest.Progress < 100)
                {
                    ClassificationResult[] classificationResults = HandleTestRequest(testRequest);
                    ConfusionMatrix confusionMatrix = ComputeConfusionMatrix(classificationResults, testRequest.TestSet, testRequest.RuleSet.DecisionAttribute);
                    TestResult testResult = new TestResult()
                    {
                        DecisionValues = classificationResults.Select(x => x.DecisionValue).ToArray(),
                        ClassificationResults = classificationResults.Select(x => x.Result).ToArray(),
                        ConfusionMatrix = confusionMatrix,
                    };

                    testRequest.TestResult = testResult;
                    testResults.Add(testResult);
                }
                
                //Send notification that process has completed testing another test request
                progressNotifier.OnProgressChanged(GetTesterProgress(++completedTestRequestCount, testRequestsCount));
            }

            //Send notification that process has been completed
            progressNotifier.OnCompleted();
            return testResults;
        }

        private int GetTesterProgress(int actualIteration, int maxIterations) => (int)((actualIteration / (double)maxIterations) * 100);

    }
}

