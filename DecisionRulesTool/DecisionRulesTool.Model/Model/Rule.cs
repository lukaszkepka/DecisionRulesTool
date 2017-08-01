using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Rule
    {
        public RuleSet RulesSet { get; set; }
        public IEnumerable<Relation> Relations { get; set; }
        public IEnumerable<Decision> Decision { get; set; }
    }
}
