using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester.Result
{
    public class TestResultAggregator
    {
        private TestRequestsGroup testRequestGroup;

        public TestResultAggregator(TestRequestsGroup testRequestGroup)
        {
            this.testRequestGroup = testRequestGroup;
        }

    }
}
