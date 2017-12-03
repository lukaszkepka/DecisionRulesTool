using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DecisionRulesTool.UserInterface.ViewModel.Results
{
    [AddINotifyPropertyChangedInterface]
    public class TestResultComparisionViewModel : ApplicationViewModel
    {
        public ICollection<RuleSetSubset> RuleSets { get; private set; }
        public ObservableCollection<GroupedRuleSetResult> GroupedRuleSetResults { get; private set; }
        public DataTable GropedTestResult { get; private set; }

        public ICommand CalculateResultTable { get; private set; }
        public ICommand SaveToFile { get; private set; }

        [PreferredConstructor]
        public TestResultComparisionViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.RuleSets = new List<RuleSetSubset>();
            this.GroupedRuleSetResults = new ObservableCollection<GroupedRuleSetResult>();

            CalculateResultTable = new RelayCommand(OnCalculateResultTable);
            SaveToFile = new RelayCommand(OnSaveToFile);
        }

        public void OnSaveToFile()
        {

        }

        public void OnCalculateResultTable()
        {
            var groupedTestResult = new DataTable();

            groupedTestResult.Columns.Add(new DataColumn("Rule Sets", typeof(string)));

            foreach (var testRequest in applicationCache.TestRequests)              
            {
                var column = GetDataColumn(groupedTestResult, testRequest.TestSet.Name);
                var precisionRow = GetDataRow(groupedTestResult, testRequest, "Total Accuary");
                precisionRow[column] = string.Format("{0:P2}", testRequest.TestResult.TotalAccuracy);

                var accuaryRow = GetDataRow(groupedTestResult, testRequest, "Accuary");
                accuaryRow[column] = string.Format("{0:P2}", testRequest.TestResult.Accuracy);

                var coverageRow = GetDataRow(groupedTestResult, testRequest, "Coverage");
                coverageRow[column] = string.Format("{0:P2}", testRequest.TestResult.Coverage);
            }

            GropedTestResult = groupedTestResult;
        }

        private DataRow GetDataRow(DataTable groupedTestResult, TestRequest testRequest, string name)
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
                    if (effectiveIndex < groupedTestResult.Rows.Count)
                    {
                        dataRow = groupedTestResult.Rows[effectiveIndex];
                        if (!dataRow[0].Equals(name))
                        {
                            dataRow = null;
                        }
                    }
                    else
                    {
                        object[] values = new object[groupedTestResult.Columns.Count];
                        values[0] = name;
                        dataRow = groupedTestResult.Rows.Add(values);
                    }
                    break;
                }
            }

            if (dataRow == null)
            {
                object[] values = new object[groupedTestResult.Columns.Count];
                values[0] = name;
                dataRow = groupedTestResult.Rows.Add(values);
                GroupedRuleSetResults.Add(new GroupedRuleSetResult(testRequest.RuleSet, testRequest.ResolvingMethod));
            }

            return dataRow;
        }

        private DataColumn GetDataColumn(DataTable groupedTestResult, string name)
        {
            DataColumn dataColumn;
            if (!groupedTestResult.Columns.Contains(name))
            {
                dataColumn = new DataColumn(name, typeof(string));
                groupedTestResult.Columns.Add(dataColumn);
            }
            else
            {
                dataColumn = groupedTestResult.Columns[name];
            }

            return dataColumn;
        }
    }
}
