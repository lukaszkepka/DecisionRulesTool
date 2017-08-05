﻿using DecisionRulesTool.Model.Model;
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

    [TestFixture]
    public class RsesRuleParserTests
    {

        public static Stream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

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
                new Attribute() {Name = "but", Type = Attribute.Category.NUMERIC, Accuary = 1},
                new Attribute() {Name = "and", Type = Attribute.Category.SYMBOLIC, Accuary = null},
            };

            StreamReader streamReader = new StreamReader(GenerateStreamFromString(fileContent));
            RuleSet ruleSet = rsesFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.Attributes.SequenceEqual(expectedResult));
        }

        [Test]
        public void ParseDecisionValues_ParsedProperly()
        {
            RsesRulesParser rsesFileParser = new RsesRulesParser();

            string fileContent =
            "RULE_SET test\n" +
            "ATTRIBUTES 0\n" +
            "DECISION_VALUES 2\n" +
            "hardy\n" +
            "james\n" +
            "RULES 0\n";

            var expectedResult = new RuleSet()
            {
                DecisionValues = new[] { "hardy", "james" }
            };

            StreamReader streamReader = new StreamReader(GenerateStreamFromString(fileContent));
            RuleSet ruleSet = rsesFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.DecisionValues.SequenceEqual(expectedResult.DecisionValues));
        }

        [Test]
        public void ParseRules_ParsedProperly()
        {
            RsesRulesParser rsesFileParser = new RsesRulesParser();

            string fileContent =
            "RULE_SET test\n" +
            "ATTRIBUTES 0\n" +
            "DECISION_VALUES 2\n" +
            "hardy\n" +
            "james\n" +
            "RULES 0\n";

            var expectedResult = new RuleSet()
            {
                DecisionValues = new[] { "hardy", "james" }
            };

            StreamReader streamReader = new StreamReader(GenerateStreamFromString(fileContent));
            RuleSet ruleSet = rsesFileParser.ParseFile(streamReader);
            Assert.IsTrue(ruleSet.DecisionValues.SequenceEqual(expectedResult.DecisionValues));
        }
    }
}
