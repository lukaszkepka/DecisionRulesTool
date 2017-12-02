using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester.Result.Interfaces
{
    public interface ITestResultLoader
    {
        TestResult LoadTestResult(string path);
    }
}
