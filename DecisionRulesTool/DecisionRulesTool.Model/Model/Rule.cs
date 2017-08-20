﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class Rule : ICloneable
    {
        public Rule(RuleSet ruleSet)
        {
            RuleSet = ruleSet;
            Conditions = new HashSet<Condition>();
            Decisions = new HashSet<Decision>();
        }

        public Rule(RuleSet ruleSet, ICollection<Condition> conditions, ICollection<Decision> decisions) : this(ruleSet)
        {
            Conditions = conditions;
            Decisions = decisions;
        }

        public ICollection<Condition> Conditions { get; }
        public ICollection<Decision> Decisions { get; }
        public RuleSet RuleSet { get; set; }
        public virtual int SupportValue => Decisions.Sum(x => (int)x.Support);

        public virtual object Clone()
        {
            return new Rule(RuleSet, Conditions, Decisions);
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            Rule rule = obj as Rule;
            if (rule != null)
            {
                result = SupportValue.Equals(rule.SupportValue) &&
                         Conditions.SequenceEqual(rule.Conditions) &&
                         Decisions.SequenceEqual(rule.Decisions);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
