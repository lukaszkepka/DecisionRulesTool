using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO
{
    internal static class RsesFileFormat
    {
        internal readonly static string RulesFileHeader = "RULE_SET";
        internal readonly static string DatasetFileHeader = "TABLE";
        internal readonly static string AttributesSectionHeader = "ATTRIBUTES";
        internal readonly static string DecisionValuesSectionHeader = "DECISION_VALUES";
        internal readonly static string RulesSectionHeader = "RULES";
        internal readonly static string ObjectsSectionHeader = "OBJECTS";
        internal readonly static string[] ConditionRelationChars = new[] { "=" };
        internal readonly static string[] DecisionRelationChars = new[] { "=" };
        internal readonly static string[] DecisionStringStartChars = new[] { "=>" };
        internal readonly static string[] MissingValueChars = new[] { "MISSING", "?" };
        internal readonly static char ConditionSeparatorChar = '&';
    }
}
