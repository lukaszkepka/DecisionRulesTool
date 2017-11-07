using DecisionRulesTool.Model.Comparers;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TestResultViewerViewModel : BaseWindowViewModel
    {
        private TestRequest selectedTestRequest;
        private IProgressNotifier ruleTesterProgressNotifier;
        private IRuleTester ruleTester;

        #region Properties
        public RuleTesterManager RuleTesterManager { get; private set; }
        public CustomDataTable TestResultDataTable { get; private set; }
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
            }
        }
        public int Progress { get; private set; }
        #endregion

        #region Commands
        public ICommand Run { get; private set; }
        #endregion

        public TestResultViewerViewModel(ICollection<TestRequest> testRequests, IUnityContainer container) : base(container)
        {
            this.ruleTesterProgressNotifier = new ProgressNotifier();
            this.ruleTester = new RuleTester(new ConditionChecker(), ruleTesterProgressNotifier);
            this.RuleTesterManager = new RuleTesterManager(testRequests);

            InitializeCommands();
            InitializeTestResultDataTable();
        }

        private void InitializeTestResultDataTable()
        {
            TestResultDataTable = new CustomDataTable();

            DataColumn resultColumn = new DataColumn("Result", typeof(string));
            DataColumn decisionValueColumn = new DataColumn("Decision", typeof(string));

            TestResultDataTable.Columns.Add(resultColumn);
            TestResultDataTable.Columns.Add(decisionValueColumn);
        }

        private void InitializeCommands()
        {
            Run = new RelayCommand(OnRunTesting);
        }

        private void FillTestResultDataTable(TestRequest selectedTestRequest)
        {
            //TestResultDataTable.Rows.Clear();
            //TestResultDataTable.Columns.Clear();

            //InitializeTestResultDataTable();
            //int rowsCount = selectedTestRequest.TestSet.Objects.Count;

            //foreach (var attribute in selectedTestRequest.TestSet.Attributes)
            //{
            //    DataColumn attributeColumn = new DataColumn(attribute.Name, typeof(object));
            //    TestResultDataTable.Columns.Add(attributeColumn);
            //}

            //for (int i = 0; i < rowsCount; i++)
            //{
            //    TestResultDataTable.Rows.Add(
            //        selectedTestRequest.TestResult?.ClassificationResults[i],
            //        selectedTestRequest.TestResult?.DecisionValues[i],
            //        selectedTestRequest.TestSet.Objects.ElementAt(i).Values);
            //}

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
