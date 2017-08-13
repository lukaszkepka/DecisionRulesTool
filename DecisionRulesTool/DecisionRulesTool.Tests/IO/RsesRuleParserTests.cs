using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests
{
    using DecisionRulesTool.Model.Model;
    using Tests;

    [TestFixture]
    [Category("Unit test")]
    public class RsesRuleParserTests
    {
        [Test]
        public void ParseAttributes_DifferentTypes_ParsedProperly()
        {
            RsesRulesParser rsesFileParser = new RsesRulesParser();

            string fileContent =
            "RULE_SET test\n" +
            "ATTRIBUTES 2\n" +
            " but numeric 1\n" +
            " and symbolic\n" +
            "DECISION_VALUES 0\n" +
            "RULES 0\n";

            var expectedResult = new[] {
                new Attribute(AttributeType.Numeric, "but"),
                new Attribute(AttributeType.Symbolic, "and")
            };

            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet ruleSet = rsesFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.Attributes.SequenceEqual(expectedResult));
        }

        [Test]
        public void ParseDecisionValues_DecisionAttributeParsedProperly()
        {
            RsesRulesParser rsesFileParser = new RsesRulesParser();

            string fileContent =
            "RULE_SET test\n" +
            "ATTRIBUTES 1\n" +
            " name symbolic\n" +
            "DECISION_VALUES 2\n" +
            "hardy\n" +
            "james\n" +
            "RULES 0\n";

            var expectedResult = new RuleSet("test");
            expectedResult.Attributes.Add(new Attribute(AttributeType.Symbolic, "name", new[] { "hardy", "james" }));
            expectedResult.DecisionAttribute = expectedResult.Attributes.Last();


            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet ruleSet = rsesFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.DecisionAttribute.Equals(expectedResult.DecisionAttribute));
        }

        [Test]
        public void ParseRules_RulesParsedProperly()
        {
            RsesRulesParser rsesFileParser = new RsesRulesParser();

            string fileContent =
            "RULE_SET Demo\n" +
            "ATTRIBUTES 3\n" +
            "temperature numeric 1\n" +
            "headache numeric 0\n" +
            "disease symbolic\n" +
            "DECISION_VALUES 1\n" +
            "angina\n" +
            "RULES 1\n" +
            "(temperature = 38.7) & (headache = 7)=>(disease = angina[1]) 1\n";

            var expectedResult = new RuleSet(
                name: "Demo",
                attributes: new[]
                {
                    new Attribute(AttributeType.Numeric, "temperature"),
                    new Attribute(AttributeType.Numeric, "headache"),
                    new Attribute(AttributeType.Symbolic, "disease", new[] {"angina"})
                },
                rules: new List<Rule>(),
                decisionAttribute: null);

            expectedResult.DecisionAttribute = expectedResult.Attributes.Last();

            Rule rule = new Rule(ruleSet: expectedResult,
                conditions: new[]
                {
                    new Condition(Relation.Equality, expectedResult.Attributes[0], 38.7),
                    new Condition(Relation.Equality, expectedResult.Attributes[1], 7.0),
                },
                decisions: new[]
                {
                    new Decision(DecisionType.Equality, null, "angina", 1)
                });

            rule.Decisions.First().Rule = rule;
            expectedResult.Rules.Add(rule);

            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet ruleSet = rsesFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.Rules.SequenceEqual(expectedResult.Rules));
        }
    }
}
