using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Te.RuleFilters
{
    using DataProvider;
    using Model.Model;
    using Model.RuleFilters;
    using RuleFilters;

    [TestFixture("Rses")]
    [TestFixture("4eMka")]
    public class AttributePresenceFilterTests
    {
        private RuleSet ruleSet;

        public AttributePresenceFilterTests(string rulesFormat)
        {
            ITestDataProvider<RuleSet> dataProvider = default(ITestDataProvider<RuleSet>);
            switch (rulesFormat)
            {
                case "Rses":
                    dataProvider = new RsesRulesProvider();
                    break;
                case "4eMka":
                    dataProvider = new _4eMkaRulesProvider();
                    break;
                default:
                    break;
            }
            ruleSet = dataProvider.GetData();
        }

        [Test]
        public void FilterRules_DifferentAttributes_FilteredRulesHaveReferenceToNewRuleSet()
        {
            #region Given
            IRuleFilter ruleFilter = new AttributePresenceFilter("HorsePower", "MaxSpeed");
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
        public void FilterRules_DifferentAttributes_FilteredProperly()
        {
            #region Given
            IRuleFilter ruleFilter = new AttributePresenceFilter("MaxSpeed", "ComprPressure");
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = new[] { ruleSet.Rules[0], ruleSet.Rules[1] };
            #endregion When
            #region Then
            Assert.IsTrue(filteredRuleSet.Rules.SequenceEqual(expectedResult));
            #endregion Then
        }

        [Test]
        public void FilterRules_EachRuleSatisfyCondition_FilteredRuleSetDidntChange()
        {
            #region Given
            IRuleFilter ruleFilter = new AttributePresenceFilter("MaxSpeed");
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
            IRuleFilter ruleFilter = new AttributePresenceFilter("HorsePower");
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = new Rule[0];
            #endregion When
            #region Then
            Assert.IsTrue(filteredRuleSet.Rules.SequenceEqual(expectedResult));
            #endregion Then
        }
    }
}
