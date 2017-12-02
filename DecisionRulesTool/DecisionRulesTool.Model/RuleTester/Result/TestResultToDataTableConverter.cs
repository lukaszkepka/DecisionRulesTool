using DecisionRulesTool.Model.RuleTester.Result.Interfaces;
using DecisionRulesTool.Model.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.RuleTester.Result
{
    public class TestResultToDataTableConverter : ITestResultConverter<DataTable>
    {
        public DataTable ConvertClassificationTable(TestRequest testRequest)
        {
            DataTable classificationTable = new DataTable();
            if (testRequest.TestResult != null)
            {
                int rowsCount = testRequest.TestSet.Objects.Count;

                classificationTable.Columns.Add(new DataColumn("lp", typeof(string)));
                classificationTable.Columns.Add(new DataColumn("Predicted Value", typeof(string)));
                classificationTable.Columns.Add(new DataColumn("Result", typeof(string)));

                foreach (var attribute in testRequest.TestSet.Attributes)
                {
                    classificationTable.Columns.Add(new DataColumn(attribute.Name, typeof(object)));
                }

                for (int i = 0; i < rowsCount; i++)
                {
                    var resultRow = new object[]
                    {
                       i.ToString(),
                       testRequest.TestResult.DecisionValues[i],
                       testRequest.TestResult.ClassificationResults[i],
                    };

                    var row = resultRow.Concat(testRequest.TestSet.Objects.ElementAt(i).Values).ToArray();

                    classificationTable.Rows.Add(row);
                }
            }

            return classificationTable;
        }

        public DataTable ConvertConfusionMatrix(TestRequest testRequest)
        {
            DataTable confusionMatrixTable = new DataTable();
            if (testRequest.TestResult != null)
            {
                var confusionMatrix = testRequest.TestResult.ConfusionMatrix;

                string[] decisionClasses = ClassificationResult.GetDecisionClasses(testRequest.RuleSet.DecisionAttribute);

                confusionMatrixTable.Columns.Add(new DataColumn("\\"));
                decisionClasses.ForEach(x => confusionMatrixTable.Columns.Add(new DataColumn(x, typeof(int))));

                foreach (var predictedDecision in decisionClasses)
                {
                    List<object> values = new List<object>()
                    {
                        predictedDecision
                    };

                    foreach (var realDecision in decisionClasses)
                    {
                        values.Add(confusionMatrix.GetConfusionValue(realDecision, predictedDecision));
                    }

                    confusionMatrixTable.Rows.Add(values.ToArray());
                }
            }

            return confusionMatrixTable;
        }
    }
}
