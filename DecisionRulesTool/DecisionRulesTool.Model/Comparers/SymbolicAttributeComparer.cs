using DecisionRulesTool.Model.Model;
using System;

namespace DecisionRulesTool.Model.Comparers
{
    public class SymbolicAttributeComparer : AttributeValuesComparer
    {
        public override AttributeType SupportedType => AttributeType.Symbolic;

        public override bool AreInRelation(Relation relation, object value1, object value2)
        {
            bool result = false;
            switch (relation)
            {
                case Relation.Equality:
                    result = ((string)value1).Equals((string)value2);
                    break;
                case Relation.Greather:
                case Relation.GreatherOrEqual:
                case Relation.Less:
                case Relation.LessOrEqual:
                    throw new NotImplementedException();
                default:
                    break;
            }
            return result;
        }
    }
}
