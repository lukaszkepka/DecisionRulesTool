using DecisionRulesTool.Model.RuleTester;
using System.Collections.Generic;
using System.Data;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;
    using PropertyChanged;
    using System.ComponentModel;
    using System.Linq;

    [AddINotifyPropertyChangedInterface]
    public class TestRequestsAggregate
    {
        public DataSet TestSet { get; }

        public IEnumerable<TestRequest> TestRequests { get; }

        public int Progress { get; set; }

        public TestRequestsAggregate(DataSet testSet, IEnumerable<TestRequest> testRequests)
        {
            this.TestSet = testSet;
            this.TestRequests = testRequests;

            foreach (var item in testRequests)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Progress"))
            {
                int progressSum = TestRequests.Sum(x => x.Progress);
                if (progressSum == TestRequests.Count() * TestRequest.MaxProgress)
                {
                    Progress = TestRequest.MaxProgress;
                }
                else
                {
                    Progress = (int)((double)TestRequests.Sum(x => x.Progress) / TestRequests.Count());
                }
            }
        }
    }
}