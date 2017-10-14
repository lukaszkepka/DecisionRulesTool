using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests.RuleFilters
{
    using Model.Model;
    using Model.RuleFilters;

    [TestFixture]
    public class RuleFilterAggreagtorTests
    {
        [Test]
        public void RunFiltering_SimpleFilters_InitialSetIsntChanged()
        {
            #region Given
            RuleSet ruleSet = GetRuleSet();
            RuleSet initialRuleSet = (RuleSet)ruleSet.Clone();
            IEnumerable<IRuleFilter> filters = new[]
            {
                new LengthFilter(Relation.Equality, 3)
            };
            RuleFilterAggregator filterAggregator = new RuleFilterAggregator(ruleSet, filters);
            #endregion Given
            #region When
            RuleSet filteredRuleSet = filterAggregator.RunFiltering();
            #endregion When
            #region Then
            Assert.AreEqual(initialRuleSet, ruleSet);
            #endregion Then
        }

        [Test]
        public void RunFiltering_NoFilters_ResultSetEqualToInitial()
        {
            #region Given
            RuleSet ruleSet = GetRuleSet();
            RuleFilterAggregator filterAggregator = new RuleFilterAggregator(ruleSet, Enumerable.Empty<IRuleFilter>());
            #endregion Given
            #region When
            RuleSet filteredRuleSet = filterAggregator.RunFiltering();
            #endregion When
            #region Then
            Assert.AreEqual(filteredRuleSet, ruleSet);
            #endregion Then
        }

        [Test]
        public void RunFiltering_NoFilters_ResultSetIsNewInstance()
        {
            #region Given
            RuleSet ruleSet = GetRuleSet();
            RuleFilterAggregator filterAggregator = new RuleFilterAggregator(ruleSet, Enumerable.Empty<IRuleFilter>());
            #endregion Given
            #region When
            RuleSet filteredRuleSet = filterAggregator.RunFiltering();
            #endregion When
            #region Then
            Assert.AreNotSame(filteredRuleSet, ruleSet);
            #endregion Then
        }

        [Test]
        public void RunFiltering_ManyFilters_FilteredProperly()
        {
            #region Given
            RuleSet ruleSet = GetRuleSet();

            IEnumerable<IRuleFilter> filters = new IRuleFilter[]
            {
                new AttributePresenceFilter(AttributePresenceFilter.Mode.All,"MaxSpeed"),
                new LengthFilter(Relation.Equality, 2)
            };

            RuleFilterAggregator filterAggregator = new RuleFilterAggregator(ruleSet, filters);
            #endregion Given
            #region When
            RuleSet filteredRuleSet = filterAggregator.RunFiltering();
            IEnumerable<Rule> expectedRules = new[] { ruleSet.Rules[2] };
            #endregion When
            #region Then
            Assert.AreEqual(filteredRuleSet.Rules, expectedRules);
            #endregion Then
        }

        private RuleSet GetRuleSet()
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
                             new Condition(Relation.LessOrEqual, attributes[3], 430.000000),
                             new Condition(Relation.GreatherOrEqual, attributes[6], 2.000000),
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
                             new Condition(Relation.LessOrEqual, attributes[5], 25.000000),
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
                ruleSet.Rules.Add(rule);
            }

            return ruleSet;
        }
    }
}
