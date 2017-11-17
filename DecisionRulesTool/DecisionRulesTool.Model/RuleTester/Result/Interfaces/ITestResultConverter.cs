﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester.Result.Interfaces
{
    public interface ITestResultConverter<T>
    {
        T ConvertConfusionMatrix(TestRequest testResult);

        T ConvertClassificationTable(TestRequest testResult);
    }
}
