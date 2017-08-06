using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.Comparers
{
    public abstract class AttributeValuesComparer : IAttributeValuesComparer
    {
        public abstract AttributeType SupportedType { get; }
        public abstract bool AreInRelation(Relation relation, object value1, object value2);

        public static IAttributeValuesComparer Instantiate(AttributeType type)
        {
            IAttributeValuesComparer result = null;
            switch (type)
            {
                case AttributeType.Numeric:
                    result = null;
                    break;
                case AttributeType.Integer:
                    result = null;
                    break;
                case AttributeType.Symbolic:
                    result = null;
                    break;
                case AttributeType.Specific:
                    result = null;
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
