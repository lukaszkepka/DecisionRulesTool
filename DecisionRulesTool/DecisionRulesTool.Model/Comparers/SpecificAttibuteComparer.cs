using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.Comparers
{
    public class SpecificAttibuteComparer : AttributeValuesComparer
    {
        private string[] availableValues;

        public SpecificAttibuteComparer(string[] availableValues)
        {
            this.availableValues = availableValues;
        }

        public override AttributeType SupportedType => AttributeType.Specific;

        public override bool AreInRelation(Relation relation, object value1, object value2)
        {
            bool result = false;
            int value1Index = GetValueIndex(value1);
            int value2Index = GetValueIndex(value2);

            if(value1Index > -1 && value2Index > -1)
            {
                switch (relation)
                {
                    case Relation.Equality:
                        result = value1Index == value2Index;
                        break;
                    case Relation.Greather:
                        result = value1Index > value2Index;
                        break;
                    case Relation.GreatherOrEqual:
                        result = value1Index >= value2Index;
                        break;
                    case Relation.Less:
                        result = value1Index < value2Index;
                        break;
                    case Relation.LessOrEqual:
                        result = value1Index <= value2Index;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private int GetValueIndex(object value)
        {
            string valueString = (string)value;
            int index = availableValues.TakeWhile(x => !x.Equals(valueString)).Count();
            if(index >= availableValues.Length)
            {
                index = -1;
            }
            return index;
        }
    }
}
