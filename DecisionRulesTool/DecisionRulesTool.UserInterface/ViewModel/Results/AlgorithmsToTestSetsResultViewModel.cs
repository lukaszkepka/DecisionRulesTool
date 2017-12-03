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
        private TestRequestGroup testRequestGroup;

        public DataTable GroupedTestResultTable { get; private set; }

        public ICommand SaveToFile { get; private set; }

        public AlgorithmsToTestSetsResultViewModel(TestRequestGroup testRequestGroup, ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.testRequestGroup = testRequestGroup;
            InitializeCommands();
            FillResultDataTable();
        }

        private void InitializeCommands()
        {
            SaveToFile = new RelayCommand(OnSaveToFile);
        }

        private void OnSaveToFile()
        {
            if (GroupedTestResultTable.Rows.Count > 0)
            {
                SaveFileDialogSettings settings = new SaveFileDialogSettings()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    ExtensionFilter = $"Excel(*.xlsx)|*.xlsx"
                };

                string filePath = servicesRepository.DialogService.SaveFileDialog(settings);
                if (filePath != null)
                {
                    //TODO : Remove to test result saver class
                    XLWorkbook excelWorkbook = new XLWorkbook();
                    excelWorkbook.Worksheets.Add(GroupedTestResultTable, $"Classification results for {testRequestGroup.TestSet.Name}");
                    excelWorkbook.SaveAs(filePath);
                }
            }
        }

        public void FillResultDataTable()
        {
            IEnumerable<TestRequest> completedTests = testRequestGroup.TestRequests.Where(x => x.IsCompleted);

            GroupedTestResultTable = new DataTable();
            if (completedTests.Any())
            {
                TestRequest exampleTest = completedTests.First();
                int rowsCount = exampleTest.TestSet.Objects.Count;

                foreach (var attribute in exampleTest.TestSet.Attributes)
                {
                    GroupedTestResultTable.Columns.Add(new DataColumn(attribute.Name, typeof(object)));
                }

                foreach (var completedTest in completedTests)
                {
                    GroupedTestResultTable.Columns.Add(new DataColumn($"{completedTest.GetShortenName()}", typeof(string)));
                }

                GroupedTestResultTable.Columns.Add(new DataColumn($"Result", typeof(string)));

                for (int i = 0; i < rowsCount; i++)
                {
                    List<object> partlyResults = new List<object>();
                    Object dataObject = exampleTest.TestSet.Objects.ElementAt(i);
                    MajorityVoting majorityVoting = new MajorityVoting(exampleTest.TestSet, exampleTest.RuleSet.DecisionAttribute);

                    foreach (var completedTest in completedTests)
                    {
                        string partlyResult = completedTest.TestResult.ClassificationResults[i];
                        string partlyDecisionValue = completedTest.TestResult.DecisionValues[i];

                        majorityVoting.AddDecision(dataObject, new Decision(DecisionType.Undefined, null, partlyDecisionValue));
                        partlyResults.Add(partlyResult);
                    }

                    ClassificationResult[] finalClassificationResult = majorityVoting.RunClassification();
                    partlyResults.Add(finalClassificationResult[i].Result);

                    object[] finalRow = dataObject.Values.Concat(partlyResults).ToArray();
                    GroupedTestResultTable.Rows.Add(finalRow);
                }
            }
        }
    }
}
