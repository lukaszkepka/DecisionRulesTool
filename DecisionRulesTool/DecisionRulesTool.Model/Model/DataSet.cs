using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class DataSet
    {
        public string Name { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
        public IEnumerable<Object> Objects { get; set; }
        public Attribute DecisionAttribute { get; set; }
    }
}
