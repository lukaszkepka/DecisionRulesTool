using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Decision
    {
        public Decision(DecisionType type, Rule rule, object value, int support = 0)
        {
            Type = type;
            Rule = rule;
            Value = value;
            Support = support;
        }

        public DecisionType Type { get; }
        public Rule Rule { get; set; }
        public Attribute DecisionAttribute => Rule.RuleSet.DecisionAttribute;
        public object Value { get; }
        public int Support { get; set; }

        public override bool Equals(object obj)
        {
            bool result = false;
            Decision decision = obj as Decision;
            if (decision != null)
            {
                result = Value.Equals(decision.Value) &&
                         Support == decision.Support &&
                         Type == decision.Type &&
                         DecisionAttribute.Equals(decision.DecisionAttribute);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
