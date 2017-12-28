using ClosedXML.Excel;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel.Results
{
    using DecisionRulesTool.Model.Model;

    [AddINotifyPropertyChangedInterface]
    public class AlgorithmsToTestSetsResultViewModel : BaseDialogViewModel
    {
        private ExtendedTestResult testResult;
        private TestRequestGroup testRequestGroup;

        public string TestSetName => testRequestGroup.TestSet.Name;
        public decimal Coverage { get; private set; }
        public decimal Accuracy { get; private set; }
        public decimal TotalAccuracy { get; private set; }
        public DataTable GroupedTestResultTable { get; private set; }
        public DataTable ConfusionMatrix { get; private set; }

        public ICommand SaveToFile { get; private set; }

        public AlgorithmsToTestSetsResultViewModel(TestRequestGroup testRequestGroup, ApplicationRepository applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.testResult = new ExtendedTestResult();
            this.testRequestGroup = testRequestGroup;
            InitializeCommands();
            CalculateResultTables();
        }

        private void InitializeCommands()
        {
            SaveToFile = new RelayCommand(OnSaveToFile);
        }

        private DataTable GetSummaryDataTable()
        {
            DataTable summaryTable = new DataTable();
            summaryTable.Columns.Add(new DataColumn("Coverage", typeof(decimal)));
            summaryTable.Columns.Add(new DataColumn("Accuracy", typeof(decimal)));
            summaryTable.Columns.Add(new DataColumn("Total Accuracy", typeof(decimal)));
            summaryTable.Rows.Add(new object[] { testResult.Coverage, testResult.Accuracy, testResult.TotalAccuracy });
            return summaryTable;
        }

        private void OnSaveToFile()
        {
            try
            {
                if (GroupedTestResultTable != null)
                {
                    SaveFileDialogSettings settings = new SaveFileDialogSettings()
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        ExtensionFilter = $"Excel(*.xlsx)|*.xlsx"
                    };

                    string filePath = servicesRepository.DialogService.SaveFileDialog(settings);
                    if (filePath != null)
                    {
                        XLWorkbook excelWorkBook = new XLWorkbook();
                        excelWorkBook.Worksheets.Add(GroupedTestResultTable, "Labels");
                        excelWorkBook.Worksheets.Add(ConfusionMatrix, "Confusion Matrix");
                        excelWorkBook.Worksheets.Add(GetSummaryDataTable(), "Summary");
                        excelWorkBook.SaveAs(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during saving results {ex.Message}");
            }
        }

        private void AddColumnsToLabelsTable(IEnumerable<TestObject> completedTests)
        {
            TestObject exampleTest = completedTests.First();

            GroupedTestResultTable.Columns.Add(new ExtendedDataColumn("Rule Set", typeof(string)));
            GroupedTestResultTable.Columns.Add(new ExtendedDataColumn("Filters", typeof(string)));
            GroupedTestResultTable.Columns.Add(new ExtendedDataColumn("Conflict Resolving Method", typeof(string)));

            int objectIndex = 0;
            foreach (var objects in exampleTest.TestSet.Objects)
            {
                GroupedTestResultTable.Columns.Add(new DataColumn($"Object {objectIndex++}"));
            }
        }

        private void AddRowsToLabelsTable(IEnumerable<TestObject> completedTests, TestObject exampleTest)
        {
            Object[] objects = exampleTest.TestSet.Objects.ToArray();

            MajorityVoting majorityVoting = new MajorityVoting(exampleTest.TestSet, exampleTest.RuleSet.DecisionAttribute);
            foreach (var completedTest in completedTests)
            {
                List<object> rowValues = new List<object>
                {
                    completedTest.RuleSet.Name,
                    ((RuleSetSubset)completedTest.RuleSet).FiltersInfo,
                    completedTest.ResolvingMethod
                };
                rowValues.AddRange(completedTest.TestResult.ClassificationResults);

                for (int i = 0; i < objects.Length; i++)
                {
                    majorityVoting.AddDecision(objects[i], new Decision(DecisionType.Undefined, null, completedTest.TestResult.DecisionValues[i]));
                }

                object[] finalRow = rowValues.ToArray();
                GroupedTestResultTable.Rows.Add(finalRow);
            }

            testResult._ClassificationResults = majorityVoting.RunClassification();

            List<object> finalResultRowValues = new List<object>
            {
                "",
                "",
                "Final Result : "
            };

            finalResultRowValues.AddRange(testResult.ClassificationResults);
            object[] finalResultRow = finalResultRowValues.ToArray();
            GroupedTestResultTable.Rows.Add(finalResultRow);
        }

        public void FillConfusionMatrixTable(TestObject exampleTestRequest)
        {
            testResult.ConfusionMatrix = RuleTester.ComputeConfusionMatrix(testResult._ClassificationResults, exampleTestRequest.TestSet, exampleTestRequest.RuleSet.DecisionAttribute);
            ConfusionMatrix = servicesRepository.TestResultConverter.ConvertConfusionMatrix(testResult.ConfusionMatrix);
        }

        public void FillSummary()
        {
            Coverage = testResult.Coverage;
            Accuracy = testResult.Accuracy;
            TotalAccuracy = testResult.TotalAccuracy;
        }

        public void FillLabelsTable(IEnumerable<TestObject> completedTests, TestObject exampleTest)
        {
            GroupedTestResultTable = new DataTable();
            AddColumnsToLabelsTable(completedTests);
            AddRowsToLabelsTable(completedTests, exampleTest);
        }

        public void CalculateResultTables()
        {
            IEnumerable<TestObject> completedTests = testRequestGroup.TestRequests.Where(x => x.IsCompleted);
            if (completedTests.Any())
            {
                TestObject exampleTest = completedTests.First();
                FillLabelsTable(completedTests, exampleTest);
                FillConfusionMatrixTable(exampleTest);
                FillSummary();
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class ExtendedDataColumn : DataColumn
    {
        public decimal Width { get; set; }

        public ExtendedDataColumn(string columnName, Type dataType) : base(columnName, dataType)
        {
            ColumnName = columnName;
            Width = 30;
        }
    }

    public class ExtendedTestResult : TestResult
    {
        private ClassificationResult[] _classificationResults;
        private ConfusionMatrix _confusionMatrix;

        public ClassificationResult[] _ClassificationResults
        {
            get
            {
                return _classificationResults;
            }
            set
            {
                _classificationResults = value;
                this.DecisionValues = _classificationResults.Select(x => x.DecisionValue).ToArray();
                this.ClassificationResults = _classificationResults.Select(x => x.Result).ToArray();
            }
        }

        public override ConfusionMatrix ConfusionMatrix
        {
            get
            {
                return _confusionMatrix;
            }
            set
            {
                _confusionMatrix = value;
                Coverage = _confusionMatrix.Coverage;
                Accuracy = _confusionMatrix.Accuary;
                TotalAccuracy = _confusionMatrix.Coverage * _confusionMatrix.Accuary;
            }
        }
    }
}