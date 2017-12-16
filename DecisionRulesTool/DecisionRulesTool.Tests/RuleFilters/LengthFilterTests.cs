using DecisionRulesTool.Model.RuleFilters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Test.RuleFilters
{
    using Model.Model;

    [TestFixture]
    [Category("Unit test")]
    public class LengthFilterTests
    {
        [Test]
        public void FilterRules_DifferentRuleLengths_FilteredRulesHaveReferenceToNewRuleSet()
        {
            #region Given
            IRuleFilter ruleFilter = new LengthFilter(Relation.GreatherOrEqual, 3);
            RuleSet ruleSet = GetRuleSet();
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = new[] { ruleSet.Rules[1] };
            #endregion When
            #region Then
            bool haveInvalidReference = false;
            foreach (Rule rule in filteredRuleSet.Rules)
            {
                if (rule.RuleSet != filteredRuleSet)
                {
                    haveInvalidReference = true;
                }
            }
            Assert.IsFalse(haveInvalidReference);
            #endregion Then
        }

        [Test]
        public void FilterRules_DifferentRuleLengths_FilteredProperly()
        {
            #region Given
            IRuleFilter ruleFilter = new LengthFilter(Relation.GreatherOrEqual, 3);
            RuleSet ruleSet = GetRuleSet();
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = new[] { ruleSet.Rules[1] };
            #endregion When
            #region Then
            Assert.IsTrue(filteredRuleSet.Rules.SequenceEqual(expectedResult));
            #endregion Then
        }

        [Test]
        public void FilterRules_EachRuleSatisfyCondition_FilteredRuleSetDidntChange()
        {
            #region Given
            IRuleFilter ruleFilter = new LengthFilter(Relation.LessOrEqual, 3);
            RuleSet ruleSet = GetRuleSet();
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = ruleSet.Rules.ToArray();
            #endregion When
            #region Then
            Assert.IsTrue(filteredRuleSet.Rules.SequenceEqual(expectedResult));
            #endregion Then
        }

        [Test]
        public void FilterRules_NoRuleSatisfyCondition_EmptyRuleSetReturned()
        {
            #region Given
            IRuleFilter ruleFilter = new LengthFilter(Relation.Greather, 3);
            RuleSet ruleSet = GetRuleSet();
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = new Rule[0];
            #endregion When
            #region Then
            Assert.IsTrue(filteredRuleSet.Rules.SequenceEqual(expectedResult));
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

            var ruleSet = new RuleSet(string.Empty, string.Empty, attributes, new List<Rule>(), attributes.Last());
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

