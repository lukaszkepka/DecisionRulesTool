using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Te.Acceptance
{
    using Model;
    using Model.Model;
    using Model.Parsers;
    using Model.Parsers.RSES;
    using System.IO;

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

            var expectedRuleSet = new RuleSet("Demo", attributes, new List<Rule>(), attributes.Last());
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

            foreach(var rule in rules)
            {
                foreach(var decision in rule.Decisions)
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
            Assert.Fail();
        }

        [Test]
        public void _4eMkaFileParser_DataSetFile_ParsedProperly()
        {
            Assert.Fail();
        }
    }
}
