using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers._4eMka;

namespace DecisionRulesTool.Model.Parsers
{
    public class _4EmkaRulesParser : _4EmkaFileParser<RuleSet>
    {
        public override string[] SupportedFormats => new[] { "rls" };

        public override RuleSet ParseFile(StreamReader fileStream)
        {
            throw new NotImplementedException();
        }

        public RuleSet ParseFile(FileStream file)
        {
            throw new NotImplementedException();
        }
    }
}
