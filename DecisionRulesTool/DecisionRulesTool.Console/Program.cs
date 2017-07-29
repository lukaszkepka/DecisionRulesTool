using DecisionRulesTool.Model;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string _4EmkaRulesFilePath = "/Examples/RSES/Rules/male.rul";
            IFileParser<RulesSet> r = new RsesRulesParser();

            r.ParseFile(Globals.TestFilesDirectory + _4EmkaRulesFilePath);
        }
    }
}
