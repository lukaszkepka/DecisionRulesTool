using DecisionRulesTool.Model;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.Parsers.RSES;
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
            string _4EmkaRulesFilePath = "/Examples/RSES/Sets/Min.tab";
            IFileParser<DataSet> r = new RsesDataSetParser();

            r.ParseFile(Globals.TestFilesDirectory + _4EmkaRulesFilePath);
        }
    }
}
