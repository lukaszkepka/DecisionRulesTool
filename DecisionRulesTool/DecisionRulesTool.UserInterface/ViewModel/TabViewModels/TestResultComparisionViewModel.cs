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
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Globalization;
using System.Windows.Controls;
using DecisionRulesTool.UserInterface.Services.Dialog;
using ClosedXML.Excel;

namespace DecisionRulesTool.UserInterface.ViewModel.Results
{
    [AddINotifyPropertyChangedInterface]
    public class TestResultComparisionViewModel : ApplicationViewModel
    {
        private DataTable resultTable;

        public ICollectionView ResultView { get; private set; }

        public ICommand CalculateResultTable { get; private set; }
        public ICommand SaveToFile { get; private set; }

        public TestResultComparisionViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            CalculateResultTable = new RelayCommand(OnCalculateResultTable);
            SaveToFile = new RelayCommand(OnSaveToFile);
        }

        public void OnSaveToFile()
        {
            try
            {
                if (resultTable != null)
                {
                    SaveFileDialogSettings settings = new SaveFileDialogSettings()
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        ExtensionFilter = $"Excel(*.xlsx)|*.xlsx"
                    };

                    string filePath = servicesRepository.DialogService.SaveFileDialog(settings);

                    //TODO : Remove to test result saver class
                    XLWorkbook excelWorkBook = new XLWorkbook();
                    excelWorkBook.Worksheets.Add(resultTable, "Algorithms comparison");
                    excelWorkBook.SaveAs(filePath);

                    servicesRepository.DialogService.ShowWarningMessage($"Saving to file completed sucessfully");
                }
                else
                {
                    servicesRepository.DialogService.ShowWarningMessage($"Result table is empty");
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during saving to file : {ex.Message})");
            }

        }

        public void OnCalculateResultTable()
        {
            resultTable = new DataTable();
            resultTable.Columns.Add(new DataColumn("Rule Set", typeof(string)));
            resultTable.Columns.Add(new DataColumn("Filters", typeof(string)));
            resultTable.Columns.Add(new DataColumn("Conflict Resolving Method", typeof(string)));
            resultTable.Columns.Add(new DataColumn("Parameter Name", typeof(string)));

            foreach (var testSet in applicationCache.TestSets)
            {
                resultTable.Columns.Add(new DataColumn(testSet.Name, typeof(string)));
            }

            var testRequestGroups = applicationCache.TestRequests.OrderBy(x => x.TestSet.Name)
                .GroupBy(x => new GroupedRuleSetResult((RuleSetSubsetViewItem)x.RuleSet, x.ResolvingMethod), new GroupedRuleSetResultComparer());

            foreach (var testRequestGroup in testRequestGroups)
            {
                DataRow coverageRow = CreateDataRow(testRequestGroup, resultTable, "Coverage");
                DataRow accuaryRow = CreateDataRow(testRequestGroup, resultTable, "Accuracy");
                DataRow totalAccuaryRow = CreateDataRow(testRequestGroup, resultTable, "Total Accuracy");

                foreach (var testRequest in testRequestGroup)
                {
                    accuaryRow[testRequest.TestSet.Name] = string.Format("{0:P2}", testRequest?.TestResult?.Accuracy);
                    coverageRow[testRequest.TestSet.Name] = string.Format("{0:P2}", testRequest?.TestResult?.Coverage);
                    totalAccuaryRow[testRequest.TestSet.Name] = string.Format("{0:P2}", testRequest?.TestResult?.TotalAccuracy);
                }
            }

            ResultView = CollectionViewSource.GetDefaultView(resultTable);
            ResultView.GroupDescriptions.Add(new ManyPropertiesGroupDescription("Rule Set", "Filters", "Conflict Resolving Method"));
        }

        public DataRow CreateDataRow(IGrouping<GroupedRuleSetResult, TestRequest> testRequestGroup, DataTable groupedTestResult, string parameter)
        {
            object[] values = new object[groupedTestResult.Columns.Count];
            values[0] = testRequestGroup.Key.RuleSet.Name;
            values[1] = testRequestGroup.Key.RuleSet.FiltersInfo;
            values[2] = testRequestGroup.Key.ConflictResolvingMethod;
            values[3] = parameter;
            return groupedTestResult.Rows.Add(values);
        }
    }

    public class GroupedRuleSetResultComparer : IEqualityComparer<GroupedRuleSetResult>
    {
        public bool Equals(GroupedRuleSetResult x, GroupedRuleSetResult y)
        {
            return x.ConflictResolvingMethod == y.ConflictResolvingMethod && x.RuleSet.GetShortenName().Equals(y.RuleSet.GetShortenName()) && x.RuleSet.FiltersShortInfo.Equals(y.RuleSet.FiltersShortInfo);
        }

        public int GetHashCode(GroupedRuleSetResult obj)
        {
            return base.GetHashCode();
        }
    }

    public class ManyPropertiesGroupDescription : GroupDescription
    {
        private string[] columnNamesToGroup;

        public ManyPropertiesGroupDescription(params string[] columnNames)
        {
            columnNamesToGroup = columnNames;
        }

        public override object GroupNameFromItem(object item, int level, CultureInfo culture)
        {
            if (item is DataRowView dataRow)
            {
                StringBuilder groupNameBuilder = new StringBuilder();
                groupNameBuilder.Append($"{dataRow["Rule Set"].ToString()}, ");

                if (!string.IsNullOrEmpty(dataRow["Filters"].ToString()))
                {
                    groupNameBuilder.Append($"({dataRow["Filters"].ToString()}), ");
                }

                groupNameBuilder.Append($"{dataRow["Conflict Resolving Method"].ToString()} ");

                return groupNameBuilder.ToString();
            }
            else
            {
                return null;
            }
        }
    }

}
