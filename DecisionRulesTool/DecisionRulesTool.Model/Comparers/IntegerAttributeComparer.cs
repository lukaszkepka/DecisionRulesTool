using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Comparers
{
    public class IntegerAttributeComparer : AttributeValuesComparer
    {
        public override AttributeType SupportedType => AttributeType.Integer;

        public override bool AreInRelation(Relation relation, object value1, object value2)
        {
            bool result = false;
            switch (relation)
            {
                case Relation.Equality:
                    result = (int)value1 == (int)value2;
                    break;
                case Relation.Greather:
                    result = (int)value1 > (int)value2;
                    break;
                case Relation.GreatherOrEqual:
                    result = (int)value1 >= (int)value2;
                    break;
                case Relation.Less:
                    result = (int)value1 < (int)value2;
                    break;
                case Relation.LessOrEqual:
                    result = (int)value1 <= (int)value2;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
