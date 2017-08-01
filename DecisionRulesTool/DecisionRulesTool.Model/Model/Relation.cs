using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Relation
    {
        public Attribute Attribute { get; set; }
        public string Value { get; set; }
        public Category Type { get; set; }

        public enum Category
        {
            Equality,
            GreatherThan,
            LessThan,
            Less,
            Greather
        }
    }
}
