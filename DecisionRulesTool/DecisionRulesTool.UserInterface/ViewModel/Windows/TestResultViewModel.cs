using ClosedXML.Excel;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Model.Converters;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel.Windows
{
    public class TestResultViewModel : BaseWindowViewModel
    {
        #region Fields
        private TestRequest testRequest;
        #endregion

        #region Properties
        public DataTable TestResultDataTable { get; private set; }
        public DataTable ConfusionMatrix { get; private set; }
        #endregion

        #region Commands
        public ICommand SaveToFile { get; private set; }
        #endregion

        #region Constructor
        public TestResultViewModel(TestRequest testRequest, ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.testRequest = testRequest;
            InitializeCommands();
            FillTestResultDataTable();
            FillConfusionMatrixDataTable();
        }
        #endregion

        #region Methods
        private void InitializeCommands()
        {
            SaveToFile = new RelayCommand(OnSaveToFile);
        }

        private void FillTestResultDataTable()
        {
            try
            {
                TestResultDataTable = servicesRepository.TestResultConverter.ConvertClassificationTable(testRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillConfusionMatrixDataTable()
        {
            try
            {
                ConfusionMatrix = servicesRepository.TestResultConverter.ConvertConfusionMatrix(testRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveResultToFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                FiltersToStringConverter filtersToStringConverter = new FiltersToStringConverter();

                //TODO : Remove to test result saver class
                XLWorkbook wb = new XLWorkbook();
                DataTable dt = TestResultDataTable;
                wb.Worksheets.Add(dt, "Labels");

                DataTable cm = ConfusionMatrix;
                wb.Worksheets.Add(cm, "Confusion Matrix");

                DataTable sm = new DataTable();
                sm.Columns.Add(new DataColumn("Coverage", typeof(decimal)));
                sm.Columns.Add(new DataColumn("Accuary", typeof(decimal)));
                sm.Columns.Add(new DataColumn("Total accuary", typeof(decimal)));
                sm.Rows.Add(new object[] { testRequest.TestResult.Coverage, testRequest.TestResult.Accuracy, testRequest.TestResult.TotalAccuracy });
                wb.Worksheets.Add(sm, "Summary");

                DataTable metaData = new DataTable();
                metaData.Columns.Add(new DataColumn("Test Set", typeof(string)));
                metaData.Columns.Add(new DataColumn("Rule Set", typeof(string)));
                metaData.Columns.Add(new DataColumn("Filters", typeof(string)));
                metaData.Columns.Add(new DataColumn("Conflict Resolving Method", typeof(string)));
                metaData.Rows.Add(new object[] { testRequest.TestSet.Name, testRequest.RuleSet.Name, filtersToStringConverter.Convert(((RuleSetSubsetViewItem)testRequest.RuleSet).Filters, typeof(string), null, CultureInfo.CurrentCulture), testRequest.ResolvingMethod });

                DataTable testSetMetaData = new DataTable();
                testSetMetaData.Columns.Add(new DataColumn("Column Name", typeof(string)));
                testSetMetaData.Columns.Add(new DataColumn("Type", typeof(string)));
                testSetMetaData.Columns.Add(new DataColumn("Available Values", typeof(string)));

                foreach (var attribute in testRequest.TestSet.Attributes)
                {
                    object o1 = attribute.Name;
                    object o2 = attribute.Type;
                    object o3 = attribute.AvailableValues.Length > 0 ? attribute.AvailableValues.Aggregate((x, y) => x + "," + y) : string.Empty;
                    testSetMetaData.Rows.Add(new object[] { o1, o2, o3 });
                }

                wb.Worksheets.Add(metaData, "MetaData");
                wb.Worksheets.Add(testSetMetaData, "AttributesMetaData");

                wb.SaveAs(filePath);
            }
        }

        private void OnSaveToFile()
        {
            if (TestResultDataTable.Rows.Count > 0)
            {
                SaveFileDialogSettings settings = new SaveFileDialogSettings()
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    ExtensionFilter = $"Excel(*.xlsx)|*.xlsx"
                };

                string filePath = servicesRepository.DialogService.SaveFileDialog(settings);
                SaveResultToFile(filePath);
            }
        }
        #endregion
    }
}
