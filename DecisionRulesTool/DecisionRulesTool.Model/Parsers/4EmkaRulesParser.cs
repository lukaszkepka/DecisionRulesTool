using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.Model.Parsers
{
    public class _4EmkaRulesParser : BaseFileParser<RulesSet>
    {
        public override string[] SupportedFormats
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override RulesSet ParseFile(StreamReader fileStream)
        {
            throw new NotImplementedException();
        }

        public RulesSet ParseFile(FileStream file)
        {
            throw new NotImplementedException();
        }
    }
}
