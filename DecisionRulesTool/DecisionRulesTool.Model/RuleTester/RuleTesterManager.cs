﻿using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DecisionRulesTool.Model.RuleTester
{
    /// <summary>
    /// Class responsible for creating test request and running rule testing
    /// </summary>
    public class RuleTesterManager
    {
        private IList<TestRequest> testRequests;
        public virtual IEnumerable<TestRequest> TestRequests
        {
            get
            {
                return testRequests;
            }
        }

        public RuleTesterManager()
        {
            testRequests = new List<TestRequest>();
        }

        public virtual IEnumerable<TestRequest> GenerateTests(DataSet[] testSets, RuleSet ruleSet, ConflictResolvingMethod conflictResolvingMethod = ConflictResolvingMethod.MajorityVoting)
        {
            IList<TestRequest> testRequests = new List<TestRequest>();
            foreach (DataSet testSet in testSets)
            {
                testRequests.Add(new TestRequest(ruleSet, testSet, conflictResolvingMethod));
            }
            return testRequests;
        }

        public virtual IEnumerable<TestRequest> GenerateTestSeries(RuleFilterAggregator filterAggregator, DataSet[] testSets)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TestResult> RunTesting(IRuleTester testingStrategy)
        {
            return testingStrategy.RunTesting(testRequests);
        }

        public virtual void AddTestRequest(TestRequest testRequest)
        {
            testRequests.Add(testRequest);
        }

        public virtual void DeleteTestRequest(int index)
        {
            testRequests.RemoveAt(index);
        }

        public virtual void Clear()
        {
            testRequests.Clear();
        }

    }
}

