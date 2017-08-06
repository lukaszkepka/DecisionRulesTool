using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Comparers
{
    public interface IAttributeValuesComparer
    {
        bool AreInRelation(Relation relation, object value1, object value2);
    }
}
