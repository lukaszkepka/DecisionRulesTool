using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class responsible for creating test request and running rule testing
/// </summary>
public class RuleTesterManager
{
    public virtual IEnumerable<TestRequest> testRequests
    {
        get;
        set;
    }

    public virtual IRuleTester IRuleTester
    {
        get;
        set;
    }

    public virtual IEnumerable<TestRequest> GenerateTests(DataSet[] testSets, RuleSet ruleSet)
    {
        throw new System.NotImplementedException();
    }

    public virtual IEnumerable<TestRequest> GenerateTestSeries(RuleFilterAggregator filterAggregator, DataSet[] testSets)
    {
        throw new System.NotImplementedException();
    }

    public virtual IEnumerable<TestResult> RunTesting()
    {
        throw new System.NotImplementedException();
    }

    public virtual void AddTestRequest(TestRequest testRequest)
    {
        throw new System.NotImplementedException();
    }

    public virtual void DeleteTestRequest(int index)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Clear()
    {
        throw new System.NotImplementedException();
    }

}

