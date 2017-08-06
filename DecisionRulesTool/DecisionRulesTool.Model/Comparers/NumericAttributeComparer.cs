using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Comparers
{
    public class NumericAttributeComparer : AttributeValuesComparer
    {
        public override AttributeType SupportedType => AttributeType.Numeric;

        public override bool AreInRelation(Relation relation, object value1, object value2)
        {
            bool result = false;
            switch (relation)
            {
                case Relation.Equality:
                    result = (double)value1 == (double)value2;
                    break;
                case Relation.Greather:
                    result = (double)value1 > (double)value2;
                    break;
                case Relation.GreatherOrEqual:
                    result = (double)value1 >= (double)value2;
                    break;
                case Relation.Less:
                    result = (double)value1 < (double)value2;
                    break;
                case Relation.LessOrEqual:
                    result = (double)value1 <= (double)value2;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
