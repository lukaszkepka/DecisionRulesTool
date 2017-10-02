using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO
{
    public class _4eMkaFileFormat : BaseFileFormat
    {
        public override string RuleFileHeader => string.Empty;
        public override string DatasetFileHeader => string.Empty;
        public override string AttributesSectionHeader => "**ATTRIBUTES";
        public override string ObjectsSectionHeader => "**EXAMPLES";
        public override string RulesSectionHeader => "**RULES";
        public override string EndFileHeader => "**END";
        public override string[] ConditionRelationChars => new[] { "<=", ">=", "=", ">", "<" };
        public override string[] DecisionRelationChars => new[] { "at most", "at least" };
        public override string[] DecisionStringStartChars => new[] { "=>" };
        public override string[] MissingValueChars => new[] { "?" };
        public override char ConditionSeparatorChar => '&';
        public string PreferencesSectionHeader => "**PREFERENCES";

        public override AttributeType GetAttributeType(string typeString)
        {
            AttributeType type = AttributeType.Undefined;

            switch (typeString)
            {
                case "(continuous)":
                    type = AttributeType.Numeric;
                    break;
            }

            if (typeString.StartsWith("["))
            {
                type = AttributeType.Symbolic;
            }
            return type;
        }

        public override Relation GetConditionRelation(string conditionString)
        {
            Relation relation = Relation.Undefined;
            if (conditionString.Contains(">="))
            {
                relation = Relation.GreatherOrEqual;
            }
            else if (conditionString.Contains("<="))
            {
                relation = Relation.LessOrEqual;
            }
            else if (conditionString.Contains(">"))
            {
                relation = Relation.Greather;
            }
            else if (conditionString.Contains("<"))
            {
                relation = Relation.Less;
            }
            else if (conditionString.Contains("="))
            {
                relation = Relation.Equality;
            }
            return relation;
        }

        public override DecisionType GetDecisionType(string rawDecision)
        {
            DecisionType decisionType = DecisionType.Undefined;
            if (rawDecision.Contains("at least"))
            {
                decisionType = DecisionType.AtLeast;
            }
            else if (rawDecision.Contains("at most"))
            {
                decisionType = DecisionType.AtMost;
            }
            else if (rawDecision.Contains("="))
            {
                decisionType = DecisionType.Equality;
            }
            return decisionType;
        }
    }
}
