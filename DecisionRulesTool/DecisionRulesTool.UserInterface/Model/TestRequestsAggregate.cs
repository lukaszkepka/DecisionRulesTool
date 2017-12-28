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
    public class TestRequestGroup
    {
        public bool IsSelected { get; set; }
        public DataSet TestSet { get; }

        public ICollection<TestObject> TestRequests { get; }

        public int Progress { get; set; }



        public TestRequestGroup(DataSet testSet, ICollection<TestObject> testRequests)
        {
            this.TestSet = testSet;
            this.TestRequests = testRequests;

            foreach (var item in testRequests)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        public void AddTestRequest(TestObject testRequest)
        {
            TestRequests.Add(testRequest);
            testRequest.PropertyChanged += Item_PropertyChanged;
        }

        public void RecalculateProgress()
        {
            int progressSum = TestRequests.Sum(x => x.Progress);
            if (progressSum == TestRequests.Count() * TestObject.MaxProgress)
            {
                Progress = TestObject.MaxProgress;
            }
            else
            {
                Progress = (int)((double)TestRequests.Sum(x => x.Progress) / TestRequests.Count());
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Progress"))
            {
                RecalculateProgress();
            }
        }
    }
}