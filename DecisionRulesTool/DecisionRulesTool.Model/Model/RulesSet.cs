﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class RuleSet
    {
        public string Name { get; set; }
        public Attribute DecisionAttribute => Attributes.LastOrDefault();
        public IEnumerable<string> DecisionValues { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
        public IEnumerable<Rule> Rules { get; set; }
    }
}
