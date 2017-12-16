using DecisionRulesTool.Model.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Tests.DataProviders;
using NSubstitute;

namespace DecisionRulesTool.Tests.RuleTester
{
    using DecisionRulesTool.Model.Utils;
    using Model.Comparers;
    using Model.Model;
    using Model.RuleTester;
    using Model.RuleTester.DecisionResolver;

    [TestFixture]
    public class MajorityVotingTests : ITestDataProvider<RuleSet>
    {
        private IProgressNotifier progressNotifier = Substitute.For<IProgressNotifier>();

        [Test]
        public void RunClassification_AllExamplesPositive()
        {
            DataSet testSet = DataProvider.GetDataset(new[] { AttributeType.Numeric, AttributeType.Numeric, AttributeType.Symbolic }, new[] { "C1", "C2", "D1" }, null);
            testSet.Objects.Add(DataProvider.GetObject(testSet, 1.0, -2.0, "T"));
            testSet.Objects.Add(DataProvider.GetObject(testSet, 5.0, -3.0, "N"));

            RuleSet r1 = GetData();

            RuleTesterManager ruleTesterManager = new RuleTesterManager();
            var testRequests = ruleTesterManager.GenerateTests( testSet , new[] { r1 }, new[] { ConflictResolvingMethod.MajorityVoting });

            foreach (var item in testRequests)
            {
                ruleTesterManager.AddTestRequest(item);
            }

            var testResults = ruleTesterManager.RunTesting(new RuleTester(new ConditionChecker(), progressNotifier));

            Assert.IsTrue(testResults.All(x => x.ClassificationResults.All(y => y.Equals(ClassificationResult.PositiveClassification))));
        }

        [Test]
        public void RunClassification_AllExamplesNegative()
        {
            DataSet testSet = DataProvider.GetDataset(new[] { AttributeType.Numeric, AttributeType.Numeric, AttributeType.Symbolic }, new[] { "C1", "C2", "D1" }, null);
            testSet.Objects.Add(DataProvider.GetObject(testSet, 1.0, -2.0, "N"));
            testSet.Objects.Add(DataProvider.GetObject(testSet, 5.0, -3.0, "T"));

            RuleSet r1 = GetData();

            RuleTesterManager ruleTesterManager = new RuleTesterManager();
            var testRequests = ruleTesterManager.GenerateTests(testSet, new[] { r1 }, new[] { ConflictResolvingMethod.MajorityVoting });

            foreach (var item in testRequests)
            {
                ruleTesterManager.AddTestRequest(item);
            }

            var testResults = ruleTesterManager.RunTesting(new RuleTester(new ConditionChecker(), progressNotifier));

            Assert.IsTrue(testResults.All(x => x.ClassificationResults.All(y => y.Equals(ClassificationResult.NegativeClassification))));
        }

        [Test]
        public void RunClassification_AllExamplesInvalid()
        {
            DataSet testSet = DataProvider.GetDataset(new[] { AttributeType.Numeric, AttributeType.Numeric, AttributeType.Symbolic }, new[] { "C1", "C2", "D1" }, null);
            testSet.Objects.Add(DataProvider.GetObject(testSet, 4.0, -2.0, "N"));
            testSet.Objects.Add(DataProvider.GetObject(testSet, 4.0, -2.0, "T"));

            RuleSet r1 = GetData();

            RuleTesterManager ruleTesterManager = new RuleTesterManager();
            var testRequests = ruleTesterManager.GenerateTests(testSet, new[] { r1 }, new[] { ConflictResolvingMethod.MajorityVoting });

            foreach (var item in testRequests)
            {
                ruleTesterManager.AddTestRequest(item);
            }

            var testResults = ruleTesterManager.RunTesting(new RuleTester(new ConditionChecker(), progressNotifier));

            Assert.IsTrue(testResults.All(x => x.ClassificationResults.All(y => y.Equals(ClassificationResult.Ambigious))));
        }

        [Test]
        public void RunClassification_AllExamplesNoCoverage()
        {
            DataSet testSet = DataProvider.GetDataset(new[] { AttributeType.Numeric, AttributeType.Numeric, AttributeType.Symbolic }, new[] { "C1", "C2", "D1" }, null);
            testSet.Objects.Add(DataProvider.GetObject(testSet, 5.0, 5.0, "T"));
            testSet.Objects.Add(DataProvider.GetObject(testSet, 5.0, 5.0, "N"));

            RuleSet r1 = GetData();

            RuleTesterManager ruleTesterManager = new RuleTesterManager();
            var testRequests = ruleTesterManager.GenerateTests(testSet, new[] { r1 }, new[] { ConflictResolvingMethod.MajorityVoting });

            foreach (var item in testRequests)
            {
                ruleTesterManager.AddTestRequest(item);
            }

            var testResults = ruleTesterManager.RunTesting(new RuleTester(new ConditionChecker(), progressNotifier));

            Assert.IsTrue(testResults.All(x => x.ClassificationResults.All(y => y.Equals(ClassificationResult.NoCoverage))));
        }

        public RuleSet GetData()
        {
            var attributes = new[]
{
                new Attribute(AttributeType.Numeric, "C1"),
                new Attribute(AttributeType.Numeric, "C2"),
                new Attribute(AttributeType.Symbolic, "D1", "T", "N")
            };

            var ruleSet = new RuleSet(string.Empty, string.Empty, attributes, new List<Rule>(), attributes.Last());
            var rules = new[]
            {
                new _4eMkaRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.LessOrEqual, attributes[0], 4.0),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "T")
                         },
                         supportValue : 1,
                         relativeStrength : 92.0M),
                new _4eMkaRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.GreatherOrEqual, attributes[0], 1.0),
                             new Condition(Relation.LessOrEqual, attributes[0], 3.0),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "T"),
                         },
                         supportValue : 1,
                         relativeStrength : 75.0M),
                new _4eMkaRule(ruleSet : ruleSet,
                         conditions : new []
                         {
                             new Condition(Relation.Less, attributes[1], 0.0),
                         },
                         decisions : new []
                         {
                             new Decision(DecisionType.Equality, null, "N"),
                         },
                         supportValue : 1,
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
