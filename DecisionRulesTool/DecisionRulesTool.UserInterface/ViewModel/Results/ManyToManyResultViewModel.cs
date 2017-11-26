using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel.Results
{
    [AddINotifyPropertyChangedInterface]
    public class ManyToManyResultViewModel : BaseWindowViewModel
    {
        private TestRequestGroup aggregatedTestResult;

        public ICollection<RuleSetSubset> RuleSets { get; private set; }
        public List<GroupedRuleSetResult> GroupedRuleSetResults { get; private set; }
        public DataTable GropedTestResult { get; private set; }

        public ManyToManyResultViewModel(TestRequestGroup aggregatedTestResult, ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.RuleSets = new List<RuleSetSubset>();
            this.GroupedRuleSetResults = new List<GroupedRuleSetResult>();
            this.aggregatedTestResult = aggregatedTestResult;
            A();
        }

        private void A()
        {
            GropedTestResult = new DataTable();

            GropedTestResult.Columns.Add(new DataColumn("Rule Sets", typeof(string)));

            foreach (var testRequest in applicationCache.TestRequests)
            {
                var column = GetDataColumn(testRequest.TestSet.Name);
                var precisionRow = GetDataRow(testRequest, "Total Accuary");
                precisionRow[column] = testRequest.TestResult.RelativePrecision;

                var accuaryRow = GetDataRow(testRequest, "Accuary");
                accuaryRow[column] = testRequest.TestResult.GlobalPrecision;

                var coverageRow = GetDataRow(testRequest, "Coverage");
                coverageRow[column] = testRequest.TestResult.Coverage;
            }
        }

        private DataRow GetDataRow(TestRequest testRequest, string name)
        {
            DataRow dataRow = null;
            int i = 0;
            while (i < GroupedRuleSetResults.Count)
            {
                var t = GroupedRuleSetResults[i++];
                if (t.RuleSet == testRequest.RuleSet && t.ConflictResolvingMethod == testRequest.ResolvingMethod)
                {
                    int offset = 0;
                    switch (name)
                    {
                        case "Total Accuary":
                            offset = 0;
                            break;
                        case "Accuary":
                            offset = 1;
                            break;
                        case "Coverage":
                            offset = 2;
                            break;
                        default:
                            break;
                    }

                    int effectiveIndex = (i - 1) * 3 + offset;
                    if (effectiveIndex < GropedTestResult.Rows.Count)
                    {
                        dataRow = GropedTestResult.Rows[effectiveIndex];
                        if (!dataRow[0].Equals(name))
                        {
                            dataRow = null;
                        }
                    }
                    else
                    {
                        object[] values = new object[GropedTestResult.Columns.Count];
                        values[0] = name;
                        dataRow = GropedTestResult.Rows.Add(values);
                    }
                    break;
                }
            }

            if (dataRow == null)
            {
                object[] values = new object[GropedTestResult.Columns.Count];
                values[0] = name;
                dataRow = GropedTestResult.Rows.Add(values);
                GroupedRuleSetResults.Add(new GroupedRuleSetResult(testRequest.RuleSet, testRequest.ResolvingMethod));
            }

            return dataRow;
        }

        private DataColumn GetDataColumn(string name)
        {
            DataColumn dataColumn;
            if (!GropedTestResult.Columns.Contains(name))
            {
                dataColumn = new DataColumn(name, typeof(string));
                GropedTestResult.Columns.Add(dataColumn);
            }
            else
            {
                dataColumn = GropedTestResult.Columns[name];
            }

            return dataColumn;
        }


    }
}
