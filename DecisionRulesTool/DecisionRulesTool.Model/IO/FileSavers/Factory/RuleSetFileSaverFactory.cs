using DecisionRulesTool.Model.Exceptions;
using DecisionRulesTool.Model.FileSavers;
using DecisionRulesTool.Model.FileSavers.RSES;
using DecisionRulesTool.Model.IO.FileSavers._4eMka;
using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO.FileSavers.Factory
{
    public class RuleSetFileSaverFactory : IFileSaverFactory<RuleSet>
    {
        public IFileSaver<RuleSet> Create(string fileExtension)
        {
            IFileSaver<RuleSet> ruleSetParser = default(IFileSaver<RuleSet>);
            switch (fileExtension)
            {
                case BaseFileFormat.FileExtensions.RSESRuleSet:
                    ruleSetParser = new RsesRulesSaver();
                    break;
                case BaseFileFormat.FileExtensions._4emkaRuleSet:
                    ruleSetParser = new _4eMkaRulesSaver();
                    break;
                default:
                    throw new FileFormatNotSupportedException(fileExtension);
            }
            return ruleSetParser;
        }
    }
}
