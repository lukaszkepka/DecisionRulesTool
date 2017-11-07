using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    /// <summary>
    /// Represents collection of test requests and their results 
    /// aggregated to test rule set
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class AggregatedTestRequestViewModel
    {
        public DataSet TestSet { get; set; }

        public ICollection<TestRequest> TestRequests { get; set; }

        public AggregatedTestRequestViewModel(DataSet testSet, ICollection<TestRequest> testRequests)
        {
            TestSet = testSet;
            TestRequests = new ObservableCollection<TestRequest>(testRequests);
        }
    }
}
