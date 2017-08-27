using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Test.DataProvider
{
    interface ITestDataProvider<T> 
    {
        T GetData();
    }
}
