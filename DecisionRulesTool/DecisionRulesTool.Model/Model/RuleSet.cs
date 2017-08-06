using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class RuleSet
    {
        public RuleSet()
        {
            this.Attributes = new HashSet<Attribute>();
            this.Rules = new HashSet<Rule>();
        }

        public string Name { get; set; }

        public virtual ICollection<Attribute> Attributes { get; set; }
        public virtual ICollection<Rule> Rules { get; set; }
    }
}
