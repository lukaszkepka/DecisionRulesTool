﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DecisionRulesTool.Model.RuleTester
{
    public interface IRuleTester
    {
        bool DumpResults { get; set; }
        IEnumerable<TestResult> RunTesting(IEnumerable<TestObject> testRequests);
    }
}

