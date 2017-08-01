using DecisionRulesTool.Model.FileLoaders.RSES;
using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecisionRulesTool.Model.FileSavers.RSES
{
    public class RsesRulesSaver : RsesFileSaver<RuleSet>
    {
        public override void Save(RuleSet content, FileStream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
