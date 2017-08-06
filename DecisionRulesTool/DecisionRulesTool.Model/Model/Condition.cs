using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Condition
    {
        public Relation RelationType { get; set; }

        public Rule Rule { get; set; }
        public Attribute Attribute { get; set; }
        public object Value { get; set; }
    }
}
