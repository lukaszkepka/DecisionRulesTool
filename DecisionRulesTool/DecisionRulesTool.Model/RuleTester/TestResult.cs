﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DecisionRulesTool.Model.RuleTester
{
    public class TestResult
    {
        public virtual string[] ClassificationResults { get; set; }
        public virtual string[] DecisionValues { get; set; }
        public virtual ConfusionMatrix ConfusionMatrix { get; set; }
        public virtual TestRequest TestRequest { get; set; }

    }
}
