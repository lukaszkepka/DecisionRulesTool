﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class RsesRule : Rule
    {
        public RsesRule(RuleSet ruleSet) : base(ruleSet)
        {

        }

        public RsesRule(RuleSet ruleSet, ICollection<Condition> conditions, ICollection<Decision> decisions) : base(ruleSet, conditions, decisions)
        {

        }
    }
}
