using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO
{
    internal static class RsesFileFormat
    {
        public const string RULES_FILE_HEADER = "RULE_SET";
        public const string DATASET_FILE_HEADER = "TABLE";
        public const string ATTRIBUTES_SECTION_HEADER = "ATTRIBUTES";
        public const string DECISION_VALUES_SECTION_HEADER = "DECISION_VALUES";
        public const string RULES_SECTION_HEADER = "RULES";
        public const string OBJECTS_SECTION_HEADER = "OBJECTS";
    }
}
