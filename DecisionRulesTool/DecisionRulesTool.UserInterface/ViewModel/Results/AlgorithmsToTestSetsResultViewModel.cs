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
    public class AlgorithmsToTestSetsResultViewModel : BaseWindowViewModel
    {
        private TestRequestGroup aggregatedTestResult;

        public DataTable GropedTestResult { get; private set; }

        public AlgorithmsToTestSetsResultViewModel(TestRequestGroup aggregatedTestResult, ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.aggregatedTestResult = aggregatedTestResult;
            A();
        }

        public void A()
        {
            var testRequest = aggregatedTestResult.TestRequests.FirstOrDefault();

            GropedTestResult = new DataTable();
            if (testRequest.TestResult != null)
            {
                int rowsCount = testRequest.TestSet.Objects.Count;

                foreach (var attribute in testRequest.TestSet.Attributes)
                {
                    GropedTestResult.Columns.Add(new DataColumn(attribute.Name, typeof(object)));
                }

                int j = 0;
                foreach (var item in aggregatedTestResult.TestRequests)
                {

                    GropedTestResult.Columns.Add(new DataColumn($"Result_{j}", typeof(string)));
                    j++;
                }

                for (int i = 0; i < rowsCount; i++)
                {
                    var row = (testRequest.TestSet.Objects.ElementAt(i).Values);

                    foreach (var item in aggregatedTestResult.TestRequests)
                    {
                        row = row.Concat(
                        new object[]
                        {
                            item.TestResult.ClassificationResults[i],
                        }).ToArray();
                    }
                    GropedTestResult.Rows.Add(row);
                }

            }
        }
    }
}
