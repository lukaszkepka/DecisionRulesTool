using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Specialized;
using DecisionRulesTool.UserInterface.Utils;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TestResultViewerViewModel : ApplicationViewModel
    {
        private TestRequest selectedTestRequest;
        private TestRequestsAggregate selectedTestRequestAggregate;
        private IProgressNotifier ruleTesterProgressNotifier;
        private IRuleTester ruleTester;

        #region Properties
        public ICollection<TestRequestsAggregate> AggregatedTestRequests { get; private set; }
        public IEnumerable<TestRequest> FilteredTestRequests
        {
            get
            {
                return SelectedTestRequestAggregate.TestRequests;
            }
        }
        public RuleTesterManager RuleTesterManager { get; private set; }
        public DataTable TestResultDataTable { get; private set; }
        public DataTable ConfusionMatrix { get; private set; }
        public TestRequest SelectedTestRequest
        {
            get
            {
                return selectedTestRequest;
            }
            set
            {
                selectedTestRequest = value;
                FillTestResultDataTable(selectedTestRequest);
                FillConfusionMatrixDataTable(selectedTestRequest);
            }
        }
        public TestRequestsAggregate SelectedTestRequestAggregate
        {
            get
            {
                return selectedTestRequestAggregate;
            }
            set
            {
                selectedTestRequestAggregate = value;

            }
        }
        public int Progress { get; private set; }
        #endregion

        #region Commands
        public ICommand Run { get; private set; }
        #endregion

        public TestResultViewerViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            this.ruleTesterProgressNotifier = new ProgressNotifier();
            this.ruleTester = new RuleTester(new ConditionChecker(), ruleTesterProgressNotifier);
            this.RuleTesterManager = new RuleTesterManager(applicationCache.TestRequests);

            InitializeCommands();
            InitializeTestResultDataTable();
            InitializeTestRequestAggregate();

            applicationCache.TestRequests.CollectionChanged += OnTestRequestCollectionChanged;
        }

        private void OnTestRequestCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            InitializeTestRequestAggregate();
        }

        private void InitializeTestResultDataTable()
        {
            TestResultDataTable = new DataTable();

            DataColumn resultColumn = new DataColumn("Result", typeof(string));
            DataColumn decisionValueColumn = new DataColumn("Decision", typeof(string));

            TestResultDataTable.Columns.Add(resultColumn);
            TestResultDataTable.Columns.Add(decisionValueColumn);
        }

        private void InitializeCommands()
        {
            Run = new RelayCommand(OnRunTesting);
        }

        private void InitializeTestRequestAggregate()
        {
            AggregatedTestRequests = new ObservableCollection<TestRequestsAggregate>();

            foreach (var groupedTestRequest in applicationCache.TestRequests.GroupBy(x => x.TestSet, y => y))
            {
                AggregatedTestRequests.Add(new TestRequestsAggregate(groupedTestRequest.Key, groupedTestRequest));
            }
        }

        private void FillTestResultDataTable(TestRequest selectedTestRequest)
        {
            int rowsCount = selectedTestRequest.TestSet.Objects.Count;
            var t = TestResultDataTable;

            TestResultDataTable.Columns.Clear();
            TestResultDataTable.Rows.Clear();

            DataColumn resultColumn = new DataColumn("Result", typeof(string));
            DataColumn decisionValueColumn = new DataColumn("Decision", typeof(string));

            TestResultDataTable.Columns.Add(resultColumn);
            TestResultDataTable.Columns.Add(decisionValueColumn);

            foreach (var attribute in selectedTestRequest.TestSet.Attributes)
            {
                DataColumn attributeColumn = new DataColumn(attribute.Name, typeof(object));
                TestResultDataTable.Columns.Add(attributeColumn);
            }

            for (int i = 0; i < rowsCount; i++)
            {
                var a = new[]
                {
                    selectedTestRequest.TestResult?.ClassificationResults[i],
                    selectedTestRequest.TestResult?.DecisionValues[i],
                }.Concat(selectedTestRequest.TestSet.Objects.ElementAt(i).Values).ToArray();
                TestResultDataTable.Rows.Add(a);
            }


            TestResultDataTable = null;
            TestResultDataTable = t;
        }

        private void FillConfusionMatrixDataTable(TestRequest selectedTestRequest)
        {
            DataTable confusionMatrixTable = new DataTable();

            var confusionMatrix = SelectedTestRequest.TestResult.ConfusionMatrix;

            string[] decisionClasses = SelectedTestRequest.RuleSet.DecisionAttribute.AvailableValues;


            confusionMatrixTable.Columns.Add(new DataColumn("\\"));
            decisionClasses.ForEach(x => confusionMatrixTable.Columns.Add(new DataColumn(x)));

            foreach (var realDecision in decisionClasses)
            {
                List<object> values = new List<object>()
                {
                    realDecision
                };

                foreach (var predictedDecision in decisionClasses)
                {
                    values.Add(confusionMatrix.GetConfusionValue(realDecision, predictedDecision));
                }

                confusionMatrixTable.Rows.Add(values.ToArray());
            }


            ConfusionMatrix = confusionMatrixTable;
        }


        public void OnRunTesting()
        {
            if (RuleTesterManager.TestRequests.Any())
            {
                RunTestsAsync(RuleTesterManager);
            }
        }

        private async void RunTestsAsync(RuleTesterManager ruleTesterManager)
        {
            ruleTesterProgressNotifier.ProgressChanged += (s, progress) => { Progress = progress; };
            await Task.Factory.StartNew(() => ruleTesterManager.RunTesting(ruleTester));
            ruleTesterProgressNotifier.ProgressChanged -= (s, progress) => { Progress = progress; }; ;
        }
    }
}
