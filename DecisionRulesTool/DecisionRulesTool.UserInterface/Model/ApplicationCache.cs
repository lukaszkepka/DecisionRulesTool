using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model
{
    public class ApplicationCache
    {
        public ICollection<RuleSetSubset> RuleSets { get; set; }
        public ICollection<DataSet> TestSets { get; set; }
        public ICollection<TestRequest> TestRequests { get; set; }
    }
}
