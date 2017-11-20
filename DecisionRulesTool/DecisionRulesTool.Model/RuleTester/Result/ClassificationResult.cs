﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester
{
    [DebuggerDisplay("ToString()")]
    public struct ClassificationResult
    {
        public const string NoCoverage = "No Coverage";
        public const string Ambigious = "Ambigious";
        public const string PositiveClassification = "Positive";
        public const string NegativeClassification = "Negative";

        public string DecisionValue { get; set; }
        public string Result { get; set; }

        public override string ToString()
        {
            return $"{DecisionValue}, {Result}";
        }
    }
}
