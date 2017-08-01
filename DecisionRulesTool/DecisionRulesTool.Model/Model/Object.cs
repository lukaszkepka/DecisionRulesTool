using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Object
    {
        public IEnumerable<Attribute> Attributes { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
