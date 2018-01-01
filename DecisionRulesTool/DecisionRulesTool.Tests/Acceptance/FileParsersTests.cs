using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests.Acceptance
{
    using Model;
    using Model.IO.Parsers._4eMka;
    using Model.Model;
    using Model.Parsers;
    using Model.Parsers.RSES;
    using System.IO;
    using Tests;

    [TestFixture]
    [Category("Acceptance test")]
    public class FileParsersTests
    {
        [Test]
        public void RsesFileParser_RuleSetFile_ParsedProperly()
        {
            #region Given
            IFileParser<RuleSet> rsesFileParser = new RsesRulesParser();

            string fileContent =
            "RULE_SET Demo\n" +
            "ATTRIBUTES 4\n" +
            "temperature numeric 1\n" +
            "headache numeric 0\n" +
            "catarrh symbolic\n" +
            "disease symbolic\n" +
            "DECISION_VALUES 4\n" +
            "angina\n" +
            "influenza\n" +
            "cold\n" +
            "healthy\n" +
            "RULES 3\n" +
            "(temperature = 38.7) & (headache = 7)=>(disease = angina[1]) 1\n" +
            "(catarrh = no)=>(disease ={ angina[1],cold[1],healthy[1]}) 3\n" +
            "(temperature = MISSING) & (headache = 3)=>(disease = cold[1]) 1\n";

            var attributes = new[]
            {
                new Attribute(AttributeType.Numeric, "temperature"),
                new Attribute(AttributeType.Numeric, "headache"),
                new Attribute(AttributeType.Symbolic, "catarrh"),
                new Attribute(AttributeType.Symbolic, "disease", new[] {"angina", "influenza", "cold", "healthy"})
            };

            var expectedRuleSet = new RuleSet("Demo", string.Empty, attributes, new List<Rule>(), attributes.Last());
            var rules = new[]
            {
                new Rule(ruleSet : expectedRuleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Equality, attributes[0], 38.7),
                             new Condition(Relation.Equality, attributes[1], 7.0),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "angina", 1)
                         }),
                new Rule(ruleSet : expectedRuleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Equality, attributes[2], "no"),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "angina", 1),
                             new Decision(DecisionType.Equality, null, "cold", 1),
                             new Decision(DecisionType.Equality, null, "healthy", 1)
                         }),
                new Rule(ruleSet : expectedRuleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Equality, attributes[0], Attribute.MissingValue),
                             new Condition(Relation.Equality, attributes[1], 3.0),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "cold", 1)
                         })
            };

            foreach (var rule in rules)
            {
                foreach (var decision in rule.Decisions)
                {
                    decision.Rule = rule;
                }
                expectedRuleSet.Rules.Add(rule);
            }

            #endregion Given
            #region When
            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet actualRuleSet = rsesFileParser.ParseFile(streamReader);
            bool attributesEqual = actualRuleSet.Attributes.SequenceEqual(expectedRuleSet.Attributes);
            bool rulesEqual = actualRuleSet.Rules.SequenceEqual(expectedRuleSet.Rules);
            #endregion When
            #region Then
            Assert.IsTrue(attributesEqual && rulesEqual);
            #endregion Then
        }

        [Test]
        public void RsesFileParser_DataSetFile_ParsedProperly()
        {
            #region Given
            IFileParser<DataSet> rsesFileParser = new RsesDataSetParser();

            string fileContent =
            "TABLE therapy\n" +
            "ATTRIBUTES 5\n" +
            "temperature numeric 1\n" +
            "headache numeric 0\n" +
            "cough symbolic\n" +
            "catarrh symbolic\n" +
            "disease symbolic\n" +
            "OBJECTS 4\n" +
            "38.7,7,no,no,angina\n" +
            "38.3,MISSING,yes,yes,influenza\n" +
            "MISSING,3,no,no,cold\n" +
            "36.7,1,no,no,healthy\n";

            var attributes = new[]
            {
                new Attribute(AttributeType.Numeric, "temperature"),
                new Attribute(AttributeType.Numeric, "headache"),
                new Attribute(AttributeType.Symbolic, "cough"),
                new Attribute(AttributeType.Symbolic, "catarrh"),
                new Attribute(AttributeType.Symbolic, "disease")
            };

            var expectedDataSet = new DataSet("therapy", attributes, new List<Object>());
            var objects = new[]
            {
                new Object(expectedDataSet, 38.7, 7.0, "no", "no", "angina"),
                new Object(expectedDataSet, 38.3, Attribute.MissingValue, "yes", "yes", "influenza"),
                new Object(expectedDataSet, Attribute.MissingValue, 3.0, "no", "no", "cold"),
                new Object(expectedDataSet, 36.7, 1.0, "no", "no", "healthy"),
            };

            foreach (var dataObject in objects)
            {
                expectedDataSet.Objects.Add(dataObject);
            }
            #endregion Given
            #region When
            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            DataSet actualDataSet = rsesFileParser.ParseFile(streamReader);
            bool attributesEqual = actualDataSet.Attributes.SequenceEqual(expectedDataSet.Attributes);
            bool objectsEqual = actualDataSet.Objects.SequenceEqual(expectedDataSet.Objects);
            #endregion When
            #region Then
            Assert.IsTrue(attributesEqual && objectsEqual);
            #endregion Then
        }

        [Test]
        public void _4eMkaFileParser_RuleSetFile_ParsedProperly()
        {
            #region Given
            IFileParser<RuleSet> _4eMkaFileParser = new _4eMkaRulesParser();

            string fileContent =
            "#\n\n" +
            "**ATTRIBUTES\n" +
            "+ MaxSpeed: (continuous)\n" +
            "+ ComprPressure: (continuous)\n" +
            "+ Blacking: (continuous)\n" +
            "+ Torque: (continuous)\n" +
            "+ SummerCons: (continuous)\n" +
            "+ WinterCons: (continuous)\n" +
            "+ OilCons: (continuous)\n" +
            "+ HorsePower: (continuous)\n" +
            "- State1: [1, 2]\n" +
            "+ State: [1, 2, 3]\n" +
            "decision: State\n" +
            "** PREFERENCES\n" +
            "MaxSpeed: gain\n" +
            "ComprPressure: gain\n" +
            "Blacking: cost\n" +
            "Torque: gain\n" +
            "SummerCons: cost\n" +
            "WinterCons: cost\n" +
            "OilCons: cost\n" +
            "HorsePower: gain\n" +
            "State1: gain\n" +
            "State: gain\n" +
            "**RULES\n" +
            "Rule 1. (Torque<=430.000000) & (OilCons>=2.000000) => (State at most 1);   [23, 92.00%]\n" +
            "Rule 2. (MaxSpeed >= 75.000000) & (Blacking <= 64.000000) & (WinterCons <= 25.000000) => (State at least 2);   [36, 75.00%]\n" +
            "Rule 3. (MaxSpeed<=80.000000) & (HorsePower>=120.000000) => (State = 1 OR 2);   [3, 100.00%]\n" +
            "**END";

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

            var expectedRuleSet = new RuleSet(string.Empty, string.Empty, attributes, new List<Rule>(), attributes.Last());
            var rules = new[]
            {
                new _4eMkaRule(ruleSet : expectedRuleSet,
                         conditions : new []
                         {
                             new Condition(Relation.LessOrEqual, attributes[3], 430.000000),
                             new Condition(Relation.GreatherOrEqual, attributes[6], 2.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.AtMost, null, "1")
                         },
                         supportValue : 23, 
                         relativeStrength : 92.0M),
                new _4eMkaRule(ruleSet : expectedRuleSet,
                         conditions : new []
                         {
                             new Condition(Relation.GreatherOrEqual, attributes[0], 75.000000),
                             new Condition(Relation.LessOrEqual, attributes[2], 64.000000),
                             new Condition(Relation.LessOrEqual, attributes[5], 25.000000),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.AtLeast, null, "2"),
                         },
                         supportValue : 36,
                         relativeStrength : 75.0M),
                new _4eMkaRule(ruleSet : expectedRuleSet,
                         conditions : new []
                         {
                             new Condition(Relation.LessOrEqual, attributes[0], 80.000000),
                             new Condition(Relation.GreatherOrEqual, attributes[7], 120.000000),
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
                expectedRuleSet.Rules.Add(rule);
            }

            #endregion Given
            #region When
            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            RuleSet actualRuleSet = _4eMkaFileParser.ParseFile(streamReader);
            bool attributesEqual = actualRuleSet.Attributes.SequenceEqual(expectedRuleSet.Attributes);
            bool rulesEqual = actualRuleSet.Rules.SequenceEqual(expectedRuleSet.Rules);
            #endregion When
            #region Then
            Assert.IsTrue(attributesEqual && rulesEqual);
            #endregion Then
        }

        [Test]
        public void _4eMkaFileParser_DataSetFile_ParsedProperly()
        {
            #region Given
            IFileParser<DataSet> _4eMkaFileParser = new _4eMkaDataSetParser();

            string fileContent =
            "**ATTRIBUTES\n" +
            "+ but:	(continuous)\n" +
            "+ and1:	(continuous)\n" +
            "+ not:	(continuous)\n" +
            "+ author:	[edith, jane]\n" +
            "decision:	author\n" +
            "** PREFERENCES\n" +
            "but:	cost\n" +
            "and1:	cost\n" +
            "not:	cost\n" +
            "author:	gain\n" +
            "**EXAMPLES\n" +
            "0.005076,   ?,   0.003285, edith\n" +
            "0.003298,   0.038381,   0.003298, edith\n" +
            "0.005678,   0.039151,   0.003288, jane\n" +
            "0.005939,   0.038055,   0.005059, jane\n" +
            "\n" +
            "**END\n";

            var attributes = new[]
{
                new Attribute(AttributeType.Numeric, "but"),
                new Attribute(AttributeType.Numeric, "and1"),
                new Attribute(AttributeType.Numeric, "not"),
                new Attribute(AttributeType.Symbolic, "author", new[] {"edith", "jane" })
            };

            var expectedDataSet = new DataSet(string.Empty, attributes, new List<Object>());
            var objects = new[]
            {
                new Object(expectedDataSet, 0.005076,   Attribute.MissingValue,   0.003285, "edith"),
                new Object(expectedDataSet, 0.003298,   0.038381,   0.003298, "edith"),
                new Object(expectedDataSet, 0.005678,   0.039151,   0.003288, "jane"),
                new Object(expectedDataSet, 0.005939,   0.038055,   0.005059, "jane"),
            };

            foreach (var dataObject in objects)
            {
                expectedDataSet.Objects.Add(dataObject);
            }
            #endregion Given
            #region When
            StreamReader streamReader = new StreamReader(Utils.GenerateStreamFromString(fileContent));
            DataSet actualDataSet = _4eMkaFileParser.ParseFile(streamReader);
            bool attributesEqual = actualDataSet.Attributes.SequenceEqual(expectedDataSet.Attributes);
            bool objectsEqual = actualDataSet.Objects.SequenceEqual(expectedDataSet.Objects);
            #endregion When
            #region Then
            Assert.IsTrue(attributesEqual && objectsEqual);
            #endregion Then
        }
    }
}
