using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Comparers
{
    using Model;
    public interface IConditionChecker
    {
        bool IsConditionSatisfied(Condition condition, Object dataObject);
    }
}
