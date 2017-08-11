using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO
{
    internal static class _4eMkaFileFormat
    {
        internal readonly static string AttributesSectionHeader = "**ATTRIBUTES";
        internal readonly static string PreferencesSectionHeader = "**PREFERENCES ";
        internal readonly static string ExamplesSectionHeader = "**EXAMPLES";
        internal readonly static string RulesSectionHeader = "**RULES";
        internal readonly static string[] ConditionRelationChars = new[] { "<=", ">=" };
        internal readonly static string[] DecisionRelationChars = new[] { "at most", "at least" };
        internal readonly static string[] DecisionStringStartChars = new[] { "=>" };
        //internal readonly static string[] MissingValueChars = new[] { "MISSING", "?" };
        internal readonly static char ConditionSeparatorChar = '&';
    }
}
