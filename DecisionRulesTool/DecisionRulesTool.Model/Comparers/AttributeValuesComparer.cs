using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.Comparers
{
    using Model;
    public abstract class AttributeValuesComparer : IAttributeValuesComparer
    {
        public abstract AttributeType SupportedType { get; }
        public abstract bool AreInRelation(Relation relation, object value1, object value2);
    }
}
