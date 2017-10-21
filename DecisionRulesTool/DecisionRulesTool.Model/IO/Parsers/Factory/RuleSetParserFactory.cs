using DecisionRulesTool.Model.Exceptions;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO.Parsers.Factory
{
    public class RuleSetParserFactory : IFileParserFactory<RuleSet>
    {
        public IFileParser<RuleSet> Create(string fileExtension)
        {
            IFileParser<RuleSet> ruleSetParser = default(IFileParser<RuleSet>);
            switch (fileExtension)
            {
                case BaseFileFormat.FileExtensions.RSESRuleSet:
                    ruleSetParser = new RsesRulesParser();
                    break;
                case BaseFileFormat.FileExtensions._4emkaRuleSet:
                    ruleSetParser = new _4eMkaRulesParser();
                    break;
                default:
                    throw new FileFormatNotSupportedException(fileExtension);
            }
            return ruleSetParser;
        }
    }
}
