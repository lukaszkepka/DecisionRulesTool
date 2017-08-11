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
            Attributes = new List<Attribute>();
            Rules = new List<Rule>();
        }
        public RuleSet(string name) : this()
        {
            Name = name;
        }
        public RuleSet(string name, IList<Attribute> attributes, IList<Rule> rules, Attribute decisionAttribute) : this(name)
        {
            Attributes = attributes;
            Rules = rules;
            DecisionAttribute = decisionAttribute;
        }

        public string Name { get; set; }
        public Attribute DecisionAttribute { get; set; }
        public IList<Attribute> Attributes { get; }
        public IList<Rule> Rules { get; }
    }
}
