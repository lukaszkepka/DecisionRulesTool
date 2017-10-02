using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO
{
    using Model;
    using System.Globalization;

    public abstract class BaseFileFormat
    {
        public static class FileExtensions
        {
            public const string RSESRuleSet = ".rul";
            public const string RSESDataset = ".tab";
            public const string _4emkaRuleSet = ".rls";
            public const string _4emkaDataset = ".isf";
        }

        public abstract string RuleFileHeader { get; }
        public abstract string DatasetFileHeader { get; }
        public abstract string AttributesSectionHeader { get; }
        public abstract string ObjectsSectionHeader { get; }
        public abstract string RulesSectionHeader { get; }
        public abstract string EndFileHeader { get; }
        public abstract string[] ConditionRelationChars { get; }
        public abstract string[] DecisionRelationChars { get; }
        public abstract string[] DecisionStringStartChars { get; }
        public abstract string[] MissingValueChars { get; }
        public abstract char ConditionSeparatorChar { get; }

        public abstract Relation GetConditionRelation(string conditionString);
        public abstract DecisionType GetDecisionType(string rawDecision);
        public virtual AttributeType GetAttributeType(string typeString)
        {
            return (AttributeType)Enum.Parse(typeof(AttributeType), typeString, true);
        }
        public object GetAttributeValue(Attribute attribute, string value)
        {
            object attributeValue = value;
            if (MissingValueChars.Contains(attributeValue))
            {
                attributeValue = Attribute.MissingValue;
            }
            else
            {
                switch (attribute.Type)
                {
                    case AttributeType.Numeric:
                        attributeValue = Convert.ToDouble(attributeValue, CultureInfo.InvariantCulture);
                        break;
                    case AttributeType.Integer:
                        attributeValue = Convert.ToInt32(attributeValue, CultureInfo.InvariantCulture);
                        break;
                }
            }
            return attributeValue;
        }
    }
}
