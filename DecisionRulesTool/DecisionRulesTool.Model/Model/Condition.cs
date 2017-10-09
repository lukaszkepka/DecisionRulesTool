using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Condition
    {
        public Condition(Relation relationType, Attribute attribute, object value, Rule rule = null)
        {
            RelationType = relationType;
            Attribute = attribute;
            Value = value;
            Rule = rule;
        }

        public Relation RelationType { get; }
        public Rule Rule { get; set; }
        public Attribute Attribute { get; }
        public object Value { get; }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Condition condition)
            {
                result = Value == null || condition.Value == null ? Value == condition.Value : Value.Equals(condition.Value) &&
                         RelationType == condition.RelationType &&
                         Attribute.Equals(condition.Attribute);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
