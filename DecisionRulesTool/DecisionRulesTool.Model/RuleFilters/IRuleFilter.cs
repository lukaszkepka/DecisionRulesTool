﻿using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleFilters
{
    interface IRuleFilter
    {
        RuleSet FilterRules(RuleSet ruleSet);
    }
}
