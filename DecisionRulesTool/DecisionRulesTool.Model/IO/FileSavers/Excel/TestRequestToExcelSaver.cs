using ClosedXML.Excel;
using DecisionRulesTool.Model.FileSavers;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DecisionRulesTool.Model.RuleTester.Result.Interfaces;

namespace DecisionRulesTool.Model.RuleTester.Result
{
    public class TestRequestToExcelSaver : BaseFileSaver<TestRequest>
    {
        private _FiltersToStringConverter filtersToStringConverter;
        private ITestResultConverter<DataTable> testResultConverterToDataTableConverter;

        public TestRequestToExcelSaver()
        {
            testResultConverterToDataTableConverter = new TestResultToDataTableConverter();
            filtersToStringConverter = new _FiltersToStringConverter();
        }

        public override void Save(TestRequest testRequest, StreamWriter fileStream)
        {
            DataTable testResultDataTable = testResultConverterToDataTableConverter.ConvertClassificationTable(testRequest);
            DataTable confusionMatrixDataTable = testResultConverterToDataTableConverter.ConvertConfusionMatrix(testRequest);
            DataTable attributesMetaDataTable = CreateAttributtesMetaDataTable(testRequest);
            DataTable metaDataTable = CreateMetaDataTable(testRequest);
            DataTable summaryDataTable = CreateSummaryDataTable(testRequest);

            XLWorkbook excelFile = CreateExcelWorkBook(testResultDataTable, confusionMatrixDataTable, attributesMetaDataTable, metaDataTable, summaryDataTable);
            excelFile.SaveAs(fileStream.BaseStream);
        }

        private DataTable CreateAttributtesMetaDataTable(TestRequest testRequest)
        {
            DataTable attributesMetaDataTable = new DataTable();
            attributesMetaDataTable.Columns.Add(new DataColumn("Column Name", typeof(string)));
            attributesMetaDataTable.Columns.Add(new DataColumn("Type", typeof(string)));
            attributesMetaDataTable.Columns.Add(new DataColumn("Available Values", typeof(string)));

            foreach (var attribute in testRequest.TestSet.Attributes)
            {
                object o1 = attribute.Name;
                object o2 = attribute.Type;
                object o3 = attribute.AvailableValues.Length > 0 ? attribute.AvailableValues.Aggregate((x, y) => x + "," + y) : string.Empty;
                attributesMetaDataTable.Rows.Add(new object[] { o1, o2, o3 });
            }

            return attributesMetaDataTable;
        }

        private DataTable CreateSummaryDataTable(TestRequest testRequest)
        {
            DataTable summaryTable = new DataTable();
            summaryTable.Columns.Add(new DataColumn("Coverage", typeof(decimal)));
            summaryTable.Columns.Add(new DataColumn("Accuracy", typeof(decimal)));
            summaryTable.Columns.Add(new DataColumn("Total Accuracy", typeof(decimal)));
            summaryTable.Rows.Add(new object[] { testRequest.TestResult.Coverage, testRequest.TestResult.Accuracy, testRequest.TestResult.TotalAccuracy });
            return summaryTable;
        }

        private DataTable CreateMetaDataTable(TestRequest testRequest)
        {
            DataTable metaData = new DataTable();
            metaData.Columns.Add(new DataColumn("Test Set", typeof(string)));
            metaData.Columns.Add(new DataColumn("Rule Set", typeof(string)));
            metaData.Columns.Add(new DataColumn("Decision Attribute Index", typeof(string)));
            metaData.Columns.Add(new DataColumn("Filters", typeof(string)));
            metaData.Columns.Add(new DataColumn("Filters short", typeof(string)));
            metaData.Columns.Add(new DataColumn("Conflict Resolving Method", typeof(string)));

            int decisionAttributeIndex = testRequest.TestSet.Attributes.Select((x, i) => new Tuple<int, string>(i, x.Name)).FirstOrDefault(x => x.Item2.Equals(testRequest.RuleSet.DecisionAttribute.Name)).Item1;

            metaData.Rows.Add(new object[] 
            {
                testRequest.TestSet.Name,
                testRequest.RuleSet.Name,
                decisionAttributeIndex,
                filtersToStringConverter.Convert(((RuleSetSubset)testRequest.RuleSet).Filters, typeof(string), null, CultureInfo.CurrentCulture),
                ((RuleSetSubset)testRequest.RuleSet).FiltersShortInfo, testRequest.ResolvingMethod
            });

            return metaData;
        }

        private XLWorkbook CreateExcelWorkBook(DataTable testResultDataTable, DataTable confusionMatrixDataTable, DataTable attributesMetaDataTable, DataTable metaDataTable, DataTable summaryDataTable)
        {
            XLWorkbook excelWorkBook = new XLWorkbook();
            excelWorkBook.Worksheets.Add(testResultDataTable, "Labels");
            excelWorkBook.Worksheets.Add(confusionMatrixDataTable, "Confusion Matrix");
            excelWorkBook.Worksheets.Add(summaryDataTable, "Summary");
            excelWorkBook.Worksheets.Add(metaDataTable, "MetaData");
            excelWorkBook.Worksheets.Add(attributesMetaDataTable, "AttributesMetaData");
            return excelWorkBook;
        }

        public void SaveResultToFile(string filePath, DataTable ConfusionMatrix, DataTable TestResultDataTable, TestRequest testRequest)
        {
            DataTable testResultDataTable = TestResultDataTable;
            DataTable confusionMatrixDataTable = ConfusionMatrix;
            DataTable attributesMetaDataTable = CreateAttributtesMetaDataTable(testRequest);
            DataTable metaDataTable = CreateMetaDataTable(testRequest);
            DataTable summaryDataTable = CreateSummaryDataTable(testRequest);

            XLWorkbook excelFile = CreateExcelWorkBook(testResultDataTable, confusionMatrixDataTable, attributesMetaDataTable, metaDataTable, summaryDataTable);
            excelFile.SaveAs(filePath);
        }
    }
}
