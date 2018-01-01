using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests.IO
{
    using Model.Model;
    using System.IO;

    [TestFixture]
    [Category("Unit test")]
    public class _4EmkaRulesParserTests
    {
        [Test]
        public void ParseAttributes_DifferentTypes_ParsedProperly()
        {
            IFileParser<RuleSet> _4emkaFileParser = new _4eMkaRulesParser();

            string fileContent =
            "#\n\n" +
            "**ATTRIBUTES\n" +
            "+ but:	(continuous)\n" +
            "+ author:	[edith, jane]\n" +
            "+ and1:	(continuous)\n" +
            "- State1:  [1, 2]\n" +
            "+ not:	(continuous)\n" +
            "decision:	author\n" +
            "**RULES\n" +
            "**END\n";

            var expectedResult = new[] {
                new Attribute(AttributeType.Numeric, "but"),
                new Attribute(AttributeType.Numeric, "and1"),
                new Attribute(AttributeType.Numeric, "not"),
                new Attribute(AttributeType.Symbolic, "author", new string[] {"edith", "jane" }),
            };

            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet ruleSet = _4emkaFileParser.ParseFile(streamReader); 
            Assert.IsTrue(ruleSet.Attributes.SequenceEqual(expectedResult));
        }

        [Test]
        public void ParseRules_RulesParsedProperly()
        {
            IFileParser<RuleSet> _4emkaFileParser = new _4eMkaRulesParser();

            string fileContent =
            "#\n\n" +
            "**ATTRIBUTES\n" +
            "+ comma:	(continuous)\n" +
            "+ author:	[curwood, london]\n" +
            "+ on:	(continuous)\n" +
            "+ what:	(continuous)\n" +
            "decision:	author\n" +
            "**RULES\n" +
            "Rule 1. (comma<=0.000962) & (what<=0.000000) & (on<=0.000479) => (author at most curwood);   [2, 2.00%]\n" +
            "**END\n";

            RuleSet expectedResult = new RuleSet(
            name: string.Empty,
            extension: string.Empty,
            attributes: new[]
            {
                new Attribute(AttributeType.Numeric, "comma"),
                new Attribute(AttributeType.Numeric, "on"),
                new Attribute(AttributeType.Numeric, "what"),
                new Attribute(AttributeType.Symbolic, "author", new string[] {"curwood", "london" }),
            },
            rules: new List<Rule>(),
            decisionAttribute: null);

            expectedResult.DecisionAttribute = expectedResult.Attributes.Last();

            Rule rule = new _4eMkaRule(
                ruleSet: expectedResult,
                conditions: new[]
                {
                    new Condition(Relation.LessOrEqual, expectedResult.Attributes[0], 0.000962),
                    new Condition(Relation.LessOrEqual, expectedResult.Attributes[2], 0.000000),
                    new Condition(Relation.LessOrEqual, expectedResult.Attributes[1], 0.000479)
                },
                decisions: new[]
                {
                    new Decision(DecisionType.AtMost, null, "curwood"),
                },
                supportValue: 2,
                relativeStrength: 2.0M);

            rule.Decisions.First().Rule = rule;
            expectedResult.Rules.Add(rule);

            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet ruleSet = _4emkaFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.Rules.SequenceEqual(expectedResult.Rules));
        }
    }
}
