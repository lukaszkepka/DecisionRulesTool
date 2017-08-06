using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Rule
    {
        public Rule()
        {
            this.Conditions = new HashSet<Condition>();
            this.Decisions = new HashSet<Decision>();
        }

        public virtual ICollection<Condition> Conditions { get; set; }
        public virtual ICollection<Decision> Decisions { get; set; }
        public virtual RuleSet RuleSet { get; set; }
    }
}
