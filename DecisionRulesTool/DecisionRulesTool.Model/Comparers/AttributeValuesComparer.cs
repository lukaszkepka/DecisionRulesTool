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

        public static IAttributeValuesComparer Instantiate(Attribute attribute)
        {
            IAttributeValuesComparer result = null;
            switch (attribute.Type)
            {
                case AttributeType.Numeric:
                    result = new NumericAttributeComparer();
                    break;
                case AttributeType.Integer:
                    result = new IntegerAttributeComparer();
                    break;
                case AttributeType.Symbolic:
                    result = new SymbolicAttributeComparer();
                    break;
                case AttributeType.Specific:
                    result = new SpecificAttibuteComparer(attribute.AvailableValues);
                    break;
                default:
                    break;
            }
            return result;
        }

    }
}
