using DecisionRulesTool.Model.RuleTester;
using System.Collections.Generic;
using System.Data;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;
    using PropertyChanged;
    using System.Linq;

    [AddINotifyPropertyChangedInterface]
    public class TestRequestsAggregate
    {
        public DataSet TestSet { get; }

        public ICollection<TestRequest> TestRequests { get; }

        public int Progress
        {
            get
            {
                int progressSum = TestRequests.Sum(x => x.Progress);
                if (progressSum == TestRequests.Count * TestRequest.MaxProgress)
                {
                    return TestRequest.MaxProgress;
                }
                else
                {
                    return (int)((double)TestRequests.Sum(x => x.Progress) / TestRequests.Count);
                }
            }
        }

        public TestRequestsAggregate(DataSet testSet, ICollection<TestRequest> testRequests)
        {
            this.TestSet = testSet;
            this.TestRequests = testRequests;

            //foreach (var item in testRequests)
            //{
            //    dynamic t = item;
            //    t.PropertyChanged += 
            //}
        }
    }
}