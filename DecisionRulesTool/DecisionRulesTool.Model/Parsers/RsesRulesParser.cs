using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using System.Diagnostics;
using DecisionRulesTool.Model.Parsers;

namespace DecisionRulesTool.Model.Parsers
{
    public class RsesRulesParser : BaseFileParser<RulesSet>
    {
        public override string[] SupportedFormats => new[] { "rul" };

        public RsesRulesParser() : base()
        {

        }

        public override RulesSet ParseFile(StreamReader fileStream)
        {
            RulesSet rules = default(RulesSet);
            using (fileStream)
            {
                while(!fileStream.EndOfStream)
                {
                    Debug.WriteLine(fileStream.ReadLine());
                }
            }
            return rules;
        }
    }
}
