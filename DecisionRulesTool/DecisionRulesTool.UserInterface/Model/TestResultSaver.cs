using ClosedXML.Excel;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model.Converters;
using DecisionRulesTool.UserInterface.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model
{
    public class TestResultSaver
    {
        private ServicesRepository servicesRepository;

        public TestResultSaver(ServicesRepository servicesRepository)
        {
            this.servicesRepository = servicesRepository;
        }

        public void SaveResultToFile(string filePath, TestRequest testRequest)
        {
            try
            {
                var TestResultDataTable = servicesRepository.TestResultConverter.ConvertClassificationTable(testRequest);
                var ConfusionMatrix = servicesRepository.TestResultConverter.ConvertConfusionMatrix(testRequest);

                SaveResultToFile(filePath, ConfusionMatrix, TestResultDataTable, testRequest);
            }
            catch (Exception ex)
            {
               
            }
        }

        public void SaveResultToFile(string filePath, DataTable ConfusionMatrix, DataTable TestResultDataTable, TestRequest testRequest)
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
                sm.Columns.Add(new DataColumn("Accuracy", typeof(decimal)));
                sm.Columns.Add(new DataColumn("Total Accuracy", typeof(decimal)));
                sm.Rows.Add(new object[] { testRequest.TestResult.Coverage, testRequest.TestResult.Accuracy, testRequest.TestResult.TotalAccuracy });
                wb.Worksheets.Add(sm, "Summary");

                DataTable metaData = new DataTable();
                metaData.Columns.Add(new DataColumn("Test Set", typeof(string)));
                metaData.Columns.Add(new DataColumn("Rule Set", typeof(string)));
                metaData.Columns.Add(new DataColumn("Decision Attribute Index", typeof(string)));
                metaData.Columns.Add(new DataColumn("Filters", typeof(string)));
                metaData.Columns.Add(new DataColumn("Conflict Resolving Method", typeof(string)));

                int decisionAttributeIndex = testRequest.TestSet.Attributes.Select((x, i) => new Tuple<int, string>(i, x.Name)).FirstOrDefault(x => x.Item2.Equals(testRequest.RuleSet.DecisionAttribute.Name)).Item1;


                metaData.Rows.Add(new object[] { testRequest.TestSet.Name, testRequest.RuleSet.Name, decisionAttributeIndex, filtersToStringConverter.Convert(((RuleSetSubsetViewItem)testRequest.RuleSet).Filters, typeof(string), null, CultureInfo.CurrentCulture), testRequest.ResolvingMethod });

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
    }
}
