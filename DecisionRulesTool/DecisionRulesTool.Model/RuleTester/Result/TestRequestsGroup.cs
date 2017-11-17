using DecisionRulesTool.Model.RuleTester;
using System.Collections.Generic;
using PropertyChanged;
using System.ComponentModel;
using System.Linq;
using System.Data;

namespace DecisionRulesTool.Model.RuleTester
{
    using DecisionRulesTool.Model.Model;


    [AddINotifyPropertyChangedInterface]
    public class TestRequestsGroup
    {
        public DataSet TestSet { get; }

        public ICollection<TestRequest> TestRequests { get; }

        public int Progress { get; private set; }

        public TestRequestsGroup(DataSet testSet, ICollection<TestRequest> testRequests)
        {
            this.TestSet = testSet;
            this.TestRequests = testRequests;
            BindProgressNotification();
        }

        private void BindProgressNotification()
        {
            foreach (var testRequest in TestRequests)
            {
                testRequest.PropertyChanged += OnTestRequestProgressChanged;
            }
        }

        private void OnTestRequestProgressChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if(eventArgs.PropertyName.Equals("Progress"))
            {
                int progressSum = TestRequests.Sum(x => x.Progress);
                if (progressSum == TestRequests.Count * TestRequest.MaxProgress)
                {
                    Progress = TestRequest.MaxProgress;
                }
                else
                {
                    Progress = (int)((double)TestRequests.Sum(x => x.Progress) / TestRequests.Count);
                }
            }
        }
    }
}