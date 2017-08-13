using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class _4eMkaRule : Rule
    {
        private int supportValue;
        public override int SupportValue => supportValue;
        public decimal RelativeStrength { get; set; }

        public _4eMkaRule(RuleSet ruleSet, int supportValue = 0, decimal relativeStrength = 0.0M) : base(ruleSet)
        {
            this.supportValue = supportValue;
            RelativeStrength = relativeStrength;
        }

        public _4eMkaRule(RuleSet ruleSet, ICollection<Condition> conditions, ICollection<Decision> decisions, int supportValue = 0, decimal relativeStrength = 0.0M) : base(ruleSet, conditions, decisions)
        {
            this.supportValue = supportValue;
            RelativeStrength = relativeStrength;
        }

        public void SetSupportValue(int supportValue)
        {
            this.supportValue = supportValue;
        }
    }
}
