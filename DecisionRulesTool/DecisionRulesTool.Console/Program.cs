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
using System.Data.Entity;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.RuleFilters;

namespace DecisionRulesTool.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileParser<RuleSet> rulesParser = new RsesRulesParser();
            IFileParser<DataSet> dataSetParser = new RsesDataSetParser();
            RuleSet ruleSet = rulesParser.ParseFile(Globals.RsesFilesDirectory + "/Rules/female.rul");
            DataSet dataSet = dataSetParser.ParseFile(Globals.RsesFilesDirectory + "/Sets/Fin.tab");

            RuleFilterAggregator a = new RuleFilterAggregator(ruleSet);
            a.AddFilter(new LengthFilter(Relation.Equality, 2));
            a.AddFilter(new SupportValueFilter(Relation.LessOrEqual, 20));

            ruleSet = a.RunFiltering();

            RuleTesterManager ruleTesterManager = new RuleTesterManager();
            var x = ruleTesterManager.GenerateTests(new[] { dataSet }, ruleSet, ConflictResolvingMethod.RefuseConflicts);

            foreach (var item in x)
            {
                ruleTesterManager.AddTestRequest(item);
            }

            var y1 = ruleTesterManager.RunTesting(new RuleTester(new ConditionChecker()));

        }
    }
}
