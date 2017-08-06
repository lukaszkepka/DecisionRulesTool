using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Decision
    {
        public enum Type
        {
            Equality,
            AtMost,
            AtLeast
        }

        public Rule Rule { get; set; }
        public Attribute Attribute { get; set; }
        public object Value { get; set; }
    }
}
