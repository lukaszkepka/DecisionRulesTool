using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Test.RuleFilters
{
    using DataProvider;
    using Model.Model;
    using Model.RuleFilters;

    [TestFixture("4eMka")]
    public class SupportValueFilterTests
    {
        private RuleSet ruleSet;

        public SupportValueFilterTests(string rulesFormat)
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
        public void FilterRules_DifferentSupportValues_FilteredRulesHaveReferenceToNewRuleSet()
        {
            #region Given
            IRuleFilter ruleFilter = new SupportValueFilter(Relation.Greather, 3);
            #endregion Given
            #region When
            RuleSet filteredRuleSet = ruleFilter.FilterRules(ruleSet);
            Rule[] expectedResult = new[] { ruleSet.Rules[0], ruleSet.Rules[1] };
            #endregion When
            #region Then
            bool haveInvalidReference = false;
            foreach (Rule rule in filteredRuleSet.Rules)
            {
                if (rule.RuleSet != filteredRuleSet)
                {
                    haveInvalidReference = true;
                    break;
                }
            }
            Assert.IsFalse(haveInvalidReference);
            #endregion Then
        }

        [Test]
        public void FilterRules_DifferentSupportValues_FilteredProperly()
        {
            #region Given
            IRuleFilter ruleFilter = new SupportValueFilter(Relation.Greather, 3);
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
            IRuleFilter ruleFilter = new SupportValueFilter(Relation.LessOrEqual, 60);
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
            IRuleFilter ruleFilter = new SupportValueFilter(Relation.Less, 3);
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
