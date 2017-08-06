using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Attribute
    {
        public Attribute()
        {
        }

        public AttributeType Type { get; set; }
        public string Name { get; set; }
        public string[] AvailableValues { get; set; }

        public DataSet DataSet { get; set; }
        public RuleSet RuleSet { get; set; }
    }
}