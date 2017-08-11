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
            RuleSet ruleSet = default(RuleSet);
            using (fileStream)
            {
                ruleSet = new RuleSet();
                ParseHeader(fileStream, ruleSet);
                ParseAttributes(fileStream, ruleSet);
                ParseDecisionValues(fileStream, ruleSet);
                ParseRules(fileStream, ruleSet);
            }
            return ruleSet;
        }

        private void ParseRules(StreamReader fileStream, RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        private void ParseDecisionValues(StreamReader fileStream, RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        private void ParseAttributes(StreamReader fileStream, RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }

        private void ParseHeader(StreamReader fileStream, RuleSet ruleSet)
        {
            throw new NotImplementedException();
        }
    }
}
