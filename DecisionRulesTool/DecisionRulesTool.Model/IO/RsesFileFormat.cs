using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.IO
{
    public class RsesFileFormat : BaseFileFormat
    {
        public override string RuleFileHeader => "RULE_SET";
        public override string DatasetFileHeader => "TABLE";
        public override string AttributesSectionHeader => "ATTRIBUTES";
        public override string RulesSectionHeader => "RULES";
        public override string ObjectsSectionHeader => "OBJECTS";
        public override string[] ConditionRelationChars => new[] { "=" };
        public override string[] DecisionRelationChars => new[] { "=" };
        public override string[] DecisionStringStartChars => new[] { "=>" };
        public override string[] MissingValueChars => new[] { "MISSING", "?" };
        public override char ConditionSeparatorChar => '&';
        public override string EndFileHeader => string.Empty;

        public string DecisionValuesSectionHeader => "DECISION_VALUES";

        public override Relation GetConditionRelation(string conditionString)
        {
            return Relation.Equality;
        }

        public override DecisionType GetDecisionType(string rawDecision)
        {
            return DecisionType.Equality;
        }
    }
}
