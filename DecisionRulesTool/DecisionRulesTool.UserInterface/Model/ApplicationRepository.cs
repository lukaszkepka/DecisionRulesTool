using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model
{
    public class ApplicationRepository
    {
        public ObservableCollection<RuleSetSubset> RuleSets { get; set; }
        public ObservableCollection<DataSet> TestSets { get; set; }
        public ObservableCollection<TestObject> TestRequests { get; set; }
    }
}
