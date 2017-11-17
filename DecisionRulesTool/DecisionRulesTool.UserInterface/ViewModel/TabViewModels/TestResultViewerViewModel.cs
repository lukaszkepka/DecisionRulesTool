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
using System;
using ClosedXML.Excel;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.RuleTester.Result;
    using DecisionRulesTool.Model.RuleTester.Result.Interfaces;
    using DecisionRulesTool.UserInterface.Services.Dialog;
    using DecisionRulesTool.UserInterface.Utils;
    using System.Windows;

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
        public ICommand SaveToFile { get; private set; }
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
            DataColumn decisionValueColumn = new DataColumn("Prediction", typeof(string));

            TestResultDataTable.Columns.Add(resultColumn);
            TestResultDataTable.Columns.Add(decisionValueColumn);
        }

        private void InitializeCommands()
        {
            Run = new RelayCommand(OnRunTesting);
            SaveToFile = new RelayCommand(OnSaveToFile);
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
            try
            {
                TestResultDataTable = servicesRepository.TestResultConverter.ConvertClassificationTable(selectedTestRequest);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void FillConfusionMatrixDataTable(TestRequest selectedTestRequest)
        {
            try
            {
                ConfusionMatrix = servicesRepository.TestResultConverter.ConvertConfusionMatrix(selectedTestRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSaveToFile()
        {
            if(TestResultDataTable.Rows.Count > 0)
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
                    XLWorkbook wb = new XLWorkbook();
                    DataTable dt = TestResultDataTable;
                    wb.Worksheets.Add(dt, "Labels");

                    DataTable cm = ConfusionMatrix;
                    wb.Worksheets.Add(cm, "Confusion Matrix");

                    wb.SaveAs(filePath);
                }

            }
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
