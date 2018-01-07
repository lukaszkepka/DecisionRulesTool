using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Tests.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Test.RuleFilters
{
    using DecisionRulesTool.Model.Model;

    class RsesRulesProvider : ITestDataProvider<RuleSet>
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

            var ruleSet = new RuleSet(string.Empty, string.Empty, attributes, new List<Rule>(), attributes.Last());
            var rules = new[]
            {
                new RsesRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Equality, attributes[0], 430.000000),
                             new Condition(Relation.Equality, attributes[1], 2.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.AtMost, null, "1", 1)
                         }),
                new RsesRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Equality, attributes[0], 75.000000),
                             new Condition(Relation.Equality, attributes[2], 64.000000),
                             new Condition(Relation.Equality, attributes[1], 25.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.AtLeast, null, "2", 1),
                         }),
                new RsesRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Equality, attributes[0], 80.000000),
                             new Condition(Relation.Equality, attributes[5], 120.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "1", 1),
                             new Decision(DecisionType.Equality, null, "2", 1)
                         })
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
