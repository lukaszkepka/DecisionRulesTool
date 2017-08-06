using DecisionRulesTool.Model.Model;
using System;


namespace DecisionRulesTool.Model.Comparers
{
    using Model;
    public class ConditionChecker : IConditionChecker
    {
        public virtual bool IsConditionSatisfied(Condition condition, Object dataObject)
        {
            bool isSatisfied = false;
            Attribute attribute = condition.Attribute;
            var objectValue = dataObject[attribute];

            IAttributeValuesComparer comparingStrategy = GetComparingStrategy(attribute);
            isSatisfied = comparingStrategy.AreInRelation(condition.RelationType, objectValue, condition.Value);

            return isSatisfied;
        }

        public IAttributeValuesComparer GetComparingStrategy(Attribute attribute)
        {
            IAttributeValuesComparer comparingStrategy = null;
            switch (attribute.Type)
            {
                case AttributeType.Numeric:
                    comparingStrategy = new NumericAttributeComparer();
                    break;
                case AttributeType.Integer:
                    comparingStrategy = new IntegerAttributeComparer();
                    break;
                case AttributeType.Symbolic:
                    comparingStrategy = new SymbolicAttributeComparer();
                    break;
                case AttributeType.Specific:
                    comparingStrategy = new SpecificAttibuteComparer(attribute.AvailableValues);
                    break;
                default:
                    break;
            }
            return comparingStrategy;
        }
    }
}
