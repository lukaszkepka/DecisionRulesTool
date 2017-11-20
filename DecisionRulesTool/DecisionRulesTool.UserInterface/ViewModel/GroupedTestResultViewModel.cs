using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class GroupedTestResultViewModel : BaseDialogViewModel
    {
        public TestRequestsAggregate TestRequestsAggregate { get; private set; }
        public DataTable GropedTestResult { get; private set; }

        public GroupedTestResultViewModel(TestRequestsAggregate aggregatedTestResult, ApplicationCache applicationCache,  ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            TestRequestsAggregate = aggregatedTestResult;
            A();
        }

        public void A()
        {
            var testRequest = TestRequestsAggregate.TestRequests.FirstOrDefault();

            GropedTestResult = new DataTable();
            if (testRequest.TestResult != null)
            {
                int rowsCount = testRequest.TestSet.Objects.Count;

                foreach (var attribute in testRequest.TestSet.Attributes)
                {
                    GropedTestResult.Columns.Add(new DataColumn(attribute.Name, typeof(object)));
                }

                int j = 0;
                foreach (var item in TestRequestsAggregate.TestRequests)
                {

                    GropedTestResult.Columns.Add(new DataColumn($"Result_{j}", typeof(string)));
                    j++;
                }

                for (int i = 0; i < rowsCount; i++)
                {
                    var row = (testRequest.TestSet.Objects.ElementAt(i).Values);

                    foreach (var item in TestRequestsAggregate.TestRequests)
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

