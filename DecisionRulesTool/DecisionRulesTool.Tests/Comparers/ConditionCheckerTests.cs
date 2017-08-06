using DecisionRulesTool.Model;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Tests
{
    using Model.Model;

    [TestFixture]
    public class ConditionCheckerTests
    {
        [Test]
        public void IsConditionSatisfied_NumericAttributeAndEqualityRelationValuesEqual_IsSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Numeric }, new[] { "TEST" }, null);
            Condition condition = GetCondition(12.0, Relation.Equality, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, 12.0);

            Assert.IsTrue(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_NumericAttributeAndEqualityRelationValuesNotEqual_IsNotSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Numeric }, new[] { "TEST" }, null);
            Condition condition = GetCondition(10.0, Relation.Equality, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, 12.0);

            Assert.IsFalse(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_SymbolicAttributeAndEqualityRelationValuesEqual_IsSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Symbolic }, new[] { "TEST" }, null);
            Condition condition = GetCondition("1", Relation.Equality, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, "1");

            Assert.IsTrue(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_SymbolicAttributeAndEqualityRelationValuesNotEqual_IsNotSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Symbolic }, new[] { "TEST" }, null);
            Condition condition = GetCondition("1", Relation.Equality, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, "2");

            Assert.IsFalse(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_SpecificAttributeAndEqualityRelationValuesNotEqual_IsNotSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Specific }, new[] { "TEST" }, new string[][] { new[] { "y", "n" } });
            Condition condition = GetCondition("y", Relation.Equality, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, "n");

            Assert.IsFalse(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_SpecificAttributeAndEqualityRelationValuesEqual_IsSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Specific }, new[] { "TEST" }, new string[][] { new[] { "y", "n" } });
            Condition condition = GetCondition("y", Relation.Equality, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, "y");

            Assert.IsTrue(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_SpecificAttributeAndGreatherRelationObjectValueGreather_IsSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Specific }, new[] { "TEST" }, new string[][] { new[] { "n", "y" } });
            Condition condition = GetCondition("n", Relation.Greather, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, "y");

            Assert.IsTrue(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_SpecificAttributeAndLessRelationObjectValueLess_IsSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Specific }, new[] { "TEST" }, new string[][] { new[] { "n", "y" } });
            Condition condition = GetCondition("y", Relation.Less, dataSet.Attributes.First());
            Object dataObject = GetObject(dataSet, "n");

            Assert.IsTrue(conditionChecker.IsConditionSatisfied(condition, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_DifferentAttributesAllValuesEqual_IsSatisfied()
        {
            IConditionChecker conditionChecker = new ConditionChecker();
            DataSet dataSet = GetDataset(new[] { AttributeType.Specific, AttributeType.Numeric }, new[] { "A1", "A2" }, new string[][] { new[] { "y", "n" }, null });
            Condition condition1 = GetCondition("y", Relation.Equality, dataSet.Attributes.First());
            Condition condition2 = GetCondition(10, Relation.GreatherOrEqual, dataSet.Attributes.Last());
            Object dataObject = GetObject(dataSet, "y", 15);

            Assert.IsTrue(conditionChecker.IsConditionSatisfied(condition1, dataObject) && conditionChecker.IsConditionSatisfied(condition2, dataObject));
        }

        [Test]
        public void IsConditionSatisfied_DifferentAttributesNotAllValuesEqual_IsNotSatisfied()
        {

        }


        private DataSet GetDataset(AttributeType[] attributeTypes, string[] names, string[][] avaiableValues)
        {
            DataSet dataSet = new DataSet();

            for (int i = 0; i < attributeTypes.Length; i++)
            {
                Attribute attribute = new Attribute()
                {
                    Type = attributeTypes[i],
                    Name = names[i],
                    AvailableValues = avaiableValues == null ? null : avaiableValues[i]
                };

                dataSet.Attributes.Add(attribute);
            }

            return dataSet;
        }

        private Object GetObject(DataSet dataSet, params object[] objectValues)
        {
            return new Object()
            {
                DataSet = dataSet,
                Values = objectValues
            };
        }

        private Condition GetCondition(object attributeValue, Relation relation, Model.Model.Attribute attribute)
        {
            return new Condition()
            {
                RelationType = relation,
                Attribute = attribute,
                Value = attributeValue
            };
        }
    }
}
