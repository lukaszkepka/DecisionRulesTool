using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Attribute
    {
        public static readonly Attribute MissingValue;
        public AttributeType Type { get; }
        public string Name { get; }
        public string[] AvailableValues { get; set; }

        public Attribute(AttributeType type, string name, params string[] availableValues)
        {
            Type = type;
            Name = name;
            AvailableValues = availableValues;
        }

        public Attribute()
        {

        }

        public override bool Equals(object obj)
        {
            bool result = false;
            Attribute attribute = obj as Attribute;
            if (attribute != null)
            {
                bool availableValuesEqual = true; //false;
                //if (attribute.AvailableValues == null || AvailableValues == null)
                //{
                //    availableValuesEqual = attribute.AvailableValues == AvailableValues;
                //}
                //else
                //{
                //    availableValuesEqual = attribute.AvailableValues.SequenceEqual(AvailableValues);
                //}
                result = attribute.Name.Equals(Name) &&
                         availableValuesEqual &&
                         attribute.Type == Type;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}