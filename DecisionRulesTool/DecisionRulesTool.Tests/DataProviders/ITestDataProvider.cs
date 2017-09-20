using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests.DataProviders
{
    interface ITestDataProvider<T> 
    {
        T GetData();
    }
}
