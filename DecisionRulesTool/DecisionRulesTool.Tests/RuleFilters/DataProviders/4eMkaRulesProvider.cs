using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Test.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Test.RuleFilters
{
    using Model.Model;
    class _4eMkaRulesProvider : ITestDataProvider<RuleSet>
    {
        public RuleSet GetData()
        {
            var attributes = new[]
            {
                new Attribute(AttributeType.Numeric, "MaxSpeed"),
                new Attribute(AttributeType.Numeric, "ComprPressure"),
                new Attribute(AttributeType.Numeric, "Blacking"),
                new Attribute(AttributeType.Numeric, "Torque"),
                new Attribute(AttributeType.Numeric, "SummerCons"),
                new Attribute(AttributeType.Numeric, "WinterCons"),
                new Attribute(AttributeType.Numeric, "OilCons"),
                new Attribute(AttributeType.Numeric, "HorsePower"),
                new Attribute(AttributeType.Symbolic, "State", new[] {"1", "2", "3"})
            };

            var ruleSet = new RuleSet(string.Empty, attributes, new List<Rule>(), attributes.Last());
            var rules = new[]
            {
                new _4eMkaRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.LessOrEqual, attributes[0], 430.000000),
                             new Condition(Relation.GreatherOrEqual, attributes[1], 2.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.AtMost, null, "1")
                         },
                         supportValue : 23,
                         relativeStrength : 92.0M),
                new _4eMkaRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.GreatherOrEqual, attributes[0], 75.000000),
                             new Condition(Relation.LessOrEqual, attributes[2], 64.000000),
                             new Condition(Relation.LessOrEqual, attributes[1], 25.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.AtLeast, null, "2"),
                         },
                         supportValue : 36,
                         relativeStrength : 75.0M),
                new _4eMkaRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.LessOrEqual, attributes[0], 80.000000),
                             new Condition(Relation.GreatherOrEqual, attributes[5], 120.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "1"),
                             new Decision(DecisionType.Equality, null, "2")
                         },
                         supportValue : 3,
                         relativeStrength : 100.0M)
            };

            foreach (var rule in rules)
            {
                foreach (var decision in rule.Decisions)
                {
                    decision.Rule = rule;
                }
                ruleSet.Rules.Add(rule);
            }

            return ruleSet;
        }
    }
}
