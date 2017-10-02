﻿using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Model
{
    public class RuleSet : ICloneable
    {

        public string Name { get; set; }
        public Attribute DecisionAttribute { get; set; }
        public IList<Attribute> Attributes { get; protected set; }
        public IList<Rule> Rules { get; protected set; }

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

        public override bool Equals(object obj)
        {
            bool result = false;
            RuleSet ruleSet = obj as RuleSet;
            if(ruleSet != null)
            {
                result = Attributes.SequenceEqual(ruleSet.Attributes) &&
                         Rules.SequenceEqual(ruleSet.Rules) &&
                         DecisionAttribute.Equals(ruleSet.DecisionAttribute);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            int decisionAttributeIndex = Attributes.IndexOf(DecisionAttribute);
            IList<Attribute> newAttributes = new List<Attribute>(Attributes);
            return new RuleSet(Name, newAttributes, new List<Rule>(Rules), newAttributes[decisionAttributeIndex]);
        }
    }
}
