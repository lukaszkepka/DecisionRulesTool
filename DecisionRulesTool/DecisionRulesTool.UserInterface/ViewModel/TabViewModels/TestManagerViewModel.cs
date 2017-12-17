using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using DecisionRulesTool.Model.RuleTester;
using PropertyChanged;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight.Command;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
using GalaSoft.MvvmLight.Ioc;
using DecisionRulesTool.UserInterface.Model.Exceptions;
using System.Data;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.Model.Comparers;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;
    using DecisionRulesTool.UserInterface.ViewModel.Dialog;
    using DecisionRulesTool.UserInterface.ViewModel.Results;
    using DecisionRulesTool.UserInterface.ViewModel.Windows;

    [AddINotifyPropertyChangedInterface]
    public class TestManagerViewModel : ApplicationViewModel
    {
        private IProgressNotifier ruleTesterProgressNotifier;
        private IRuleTester ruleTester;

        public enum TestRequestFilter
        {
            All,
            ForSelectedTestSet,
            ForSelcetedRuleSet
        }

        #region Commands
        public ICommand Run { get; private set; }
        public ICommand ShowGroupedTestResults { get; private set; }
        public ICommand LoadTestSets { get; private set; }
        public ICommand ViewTestSet { get; private set; }
        public ICommand FilterTestRequests { get; private set; }
        public ICommand GenerateTestRequests { get; private set; }
        public ICommand DeleteTestRequest { get; private set; }
        public ICommand LoadTestResult { get; private set; }
        public ICommand UndoLastLoadedTestRequests { get; private set; }
        public ICommand ShowTestResults { get; private set; }
        public ICommand SaveAllResults { get; private set; }


        public ICommand DeleteTestRequestGroup { get; private set; }

        #endregion

        #region Properties
        /// <summary>
        /// Wrapper property for application's cache data set collection
        /// </summary>
        public IEnumerable<DataSet> TestSets
        {
            get
            {
                return applicationCache.TestSets;
            }
        }

        /// <summary>
        /// Wrapper property for application's cache test request collection
        /// </summary>
        public IEnumerable<TestRequest> TestRequests
        {
            get
            {
                return applicationCache.TestRequests;
            }
        }

        public IEnumerable<TestRequest> FilteredTestRequests { get; private set; }
        public TestRequest SelectedTestRequest { get; set; }
        public RuleTesterManager RuleTesterManager { get; private set; }
        public ICollection<TestRequestGroup> TestRequestGroups { get; private set; }
        public TestRequestGroup SelectedTestRequestGroup
        {
            get; set;
        }
        public int Progress { get; private set; }
        public bool IsThinking { get; private set; }
        public bool DumpResults
        {
            get
            {
                return ruleTester.DumpResults;
            }
            set
            {
                ruleTester.DumpResults = value;
            }
        }
        #endregion

        #region Constructors
        public TestManagerViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            this.ruleTesterProgressNotifier = new ProgressNotifier();
            this.ruleTester = new RuleTester(new ConditionChecker(), ruleTesterProgressNotifier);
            this.RuleTesterManager = new RuleTesterManager();

            InitializeCommands();
            InitializeTestRequestGroups();
            OnFilterTestRequests(TestRequestFilter.All);
        }
        #endregion

        private void InitializeTestRequestGroups()
        {
            TestRequestGroups = new ObservableCollection<TestRequestGroup>();
            foreach (DataSet testSet in applicationCache.TestSets)
            {
                ICollection<TestRequest> testRequests = applicationCache.TestRequests.Where(x => x.TestSet == testSet).ToList();
                TestRequestGroups.Add(new TestRequestGroup(testSet, testRequests));
            }
        }

        private void InitializeCommands()
        {
            Run = new RelayCommand(OnRunTesting);
            //SaveToFile = new RelayCommand(OnSaveToFile);
            ShowGroupedTestResults = new RelayCommand<TestRequestGroup>(OnShowResultsForTestSet);
            ShowTestResults = new RelayCommand<TestRequest>(OnShowSingleTestResult);

            UndoLastLoadedTestRequests = new RelayCommand(OnUndoLastLoadedTestRequests);
            SaveAllResults = new RelayCommand(OnSaveAllResults);
            ViewTestSet = new RelayCommand<TestRequestGroup>(OnViewTestSet);
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            LoadTestResult = new RelayCommand(OnLoadTestResult);
            FilterTestRequests = new RelayCommand<TestRequestFilter>(OnFilterTestRequests);
            GenerateTestRequests = new RelayCommand(OnGenerateTestRequests);
            DeleteTestRequest = new RelayCommand(OnDeleteTestRequest);
            DeleteTestRequestGroup = new RelayCommand<TestRequestGroup>(OnDeleteTestRequestGroup);
        }

        private void OnUndoLastLoadedTestRequests()
        {
            try
            {
                if (applicationCache.TestRequests.Any())
                {
                    int lastSerieNumber = applicationCache.TestRequests.Max(x => x.SeriesNumber);
                    var testRequestsToDelete = applicationCache.TestRequests.Where(x => x.SeriesNumber == lastSerieNumber);
                    if (testRequestsToDelete.Any())
                    {
                        DeleteTestRequests(testRequestsToDelete);
                    }
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during undoing last operation : {ex.Message}");
            }

        }

        private void OnDeleteTestRequestGroup(TestRequestGroup obj)
        {
            try
            {
                TestRequestGroups.Remove(obj);
                applicationCache.TestSets.Remove(obj.TestSet);

                foreach (var item in obj.TestRequests)
                {
                    applicationCache.TestRequests.Remove(item);
                }
                UpdateNumericComparisonResultTable();
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during deleting test results : {ex.Message}");
            }
        }

        private void OnSaveAllResults()
        {
            try
            {
                if (applicationCache.TestRequests.Any())
                {
                    int serieNumber = GetTestRequestSerieNumber();
                    TestResultSaverViewModel testResultSaverViewModel = new TestResultSaverViewModel(applicationCache, servicesRepository);
                    testResultSaverViewModel.RunSaving();
                    servicesRepository.DialogService.ShowDialog(testResultSaverViewModel);
                }
                else
                {
                    //TODO
                }

            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during saving test results : {ex.Message}");
            }
        }

        private void OnLoadTestResult()
        {
            try
            {
                int serieNumber = GetTestRequestSerieNumber();

                TestResultLoaderViewModel progressViewModel = new TestResultLoaderViewModel(AddTestRequest, applicationCache, servicesRepository);
                progressViewModel.RunLoading(serieNumber);
                servicesRepository.DialogService.ShowDialog(progressViewModel);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during loading test results : {ex.Message}");
            }
            finally
            {
                UpdateNumericComparisonResultTable();
            }
        }

        private void UpdateNumericComparisonResultTable()
        {
            //Refactor:
            SimpleIoc.Default.GetInstance<TestResultComparisionViewModel>().OnCalculateResultTable();
            //
        }

        private async void RunTestsAsync(RuleTesterManager ruleTesterManager)
        {
            try
            {
                ruleTesterProgressNotifier.ProgressChanged += (s, progress) => { Progress = progress; };
                await Task.Factory.StartNew(() => ruleTesterManager.RunTesting(ruleTester));
                UpdateNumericComparisonResultTable();
                ruleTesterProgressNotifier.ProgressChanged -= (s, progress) => { Progress = progress; }; ;
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during testing error : {ex.Message}");
            }
        }

        private TestRequestGeneratorViewModel InstantiateTestRequestGeneratorViewModel()
        {
            TestRequestGeneratorViewModel viewModel = SimpleIoc.Default.GetInstance<TestRequestGeneratorViewModel>();
            viewModel.TestSets = GetSelectedTestSets();
            return viewModel;
        }

        private void AddTestSet(DataSet testSet)
        {
            applicationCache.TestSets.Add(testSet);
            TestRequestGroups.Add(new TestRequestGroup(testSet, new ObservableCollection<TestRequest>()));
        }

        private void AddTestRequest(TestRequest testRequest, int serieNumber)
        {
            testRequest.SeriesNumber = serieNumber;

            var testRequestGroup = TestRequestGroups.FirstOrDefault(x => x.TestSet.Name.Equals(testRequest.TestSet.Name));
            if (testRequestGroup != null)
            {
                testRequest.TestSet = testRequestGroup.TestSet;

            }
            else
            {
                AddTestSet(testRequest.TestSet);
                testRequestGroup = TestRequestGroups.FirstOrDefault(x => x.TestSet.Name.Equals(testRequest.TestSet.Name));
            }

            applicationCache.TestRequests.Add(testRequest);
            testRequestGroup.AddTestRequest(testRequest);
            testRequestGroup.RecalculateProgress();
        }



        public void OnLoadTestSets()
        {
            foreach (var testSet in servicesRepository.DataSetLoaderService.LoadDataSets())
            {
                if (applicationCache.TestSets.Any(x => x.Name.Equals(testSet.Name)))
                {
                    servicesRepository.DialogService.ShowInformationMessage($"Test Set with name {testSet.Name} is already loaded");
                }
                else
                {
                    AddTestSet(testSet);
                }
            }
        }

        private void OnViewTestSet(TestRequestGroup testRequestGroup)
        {
            try
            {
                TestSetViewModel testSetDialogViewModel = new TestSetViewModel(testRequestGroup.TestSet, applicationCache, servicesRepository);
                servicesRepository.DialogService.ShowDialog(testSetDialogViewModel);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void OnGenerateTestRequests()
        {
            try
            {
                TestRequestGeneratorViewModel testRequestGeneratorViewModel = InstantiateTestRequestGeneratorViewModel();

                if (testRequestGeneratorViewModel != null && servicesRepository.DialogService.ShowDialog(testRequestGeneratorViewModel) == true)
                {
                    int serieNumber = GetTestRequestSerieNumber();
                    foreach (var testRequest in testRequestGeneratorViewModel.GenerateTestRequests())
                    {
                        AddTestRequest(testRequest, serieNumber);
                    }
                    OnFilterTestRequests(TestRequestFilter.All);
                }
            }
            catch (IncompatibleTestSetsException ex)
            {
                servicesRepository.DialogService.ShowWarningMessage(ex.Message);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void DeleteTestRequests(IEnumerable<TestRequest> testRequestsToDelete)
        {
            foreach (var testRequest in testRequestsToDelete.ToList())
            {
                if (testRequest != null)
                {
                    var testRequestGroup = TestRequestGroups.FirstOrDefault(x => x.TestSet.Name.Equals(testRequest.TestSet.Name));
                    if (testRequestGroup != null)
                    {
                        if (testRequestGroup.TestRequests.Contains(testRequest))
                        {
                            testRequestGroup.TestRequests.Remove(testRequest);
                            applicationCache.TestRequests.Remove(testRequest);
                        }
                    }
                }
            }

            UpdateNumericComparisonResultTable();
        }

        private void OnDeleteTestRequest()
        {
            try
            {
                var testRequestsToDelete = TestRequests.Where(x => x.IsSelected);
                DeleteTestRequests(testRequestsToDelete);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Fatal error during deleting test requests : {ex.Message}");
            }

        }

        public int GetTestRequestSerieNumber()
        {
            if (TestRequests.Any())
            {
                return TestRequests.Max(x => x.SeriesNumber) + 1;
            }
            else
            {
                return 0;
            }
        }


        public void OnRunTesting()
        {
            IEnumerable<TestRequest> notCompletedTestRequests = GetNotCompletedTestRequests();
            RuleTesterManager.Clear();
            RuleTesterManager.AddTestRequests(notCompletedTestRequests);

            if (RuleTesterManager.TestRequests.Any())
            {
                RunTestsAsync(RuleTesterManager);
            }
        }

        /// <summary>
        /// Opens window with classification results grouped for single test set
        /// </summary>
        private void OnShowResultsForTestSet(TestRequestGroup testRequestGroup)
        {
            try
            {
                if (testRequestGroup == null)
                {
                    servicesRepository.DialogService.ShowInformationMessage($"Select test set for which you want to see test results");
                }
                else
                {
                    servicesRepository.DialogService.ShowDialog(new AlgorithmsToTestSetsResultViewModel(testRequestGroup, applicationCache, servicesRepository));
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Error during grouping results for \"{SelectedTestRequestGroup?.TestSet?.Name}\" test set");
            }
        }

        /// <summary>
        /// Opens window with classification results for single test request
        /// </summary>
        private void OnShowSingleTestResult(TestRequest testRequest)
        {
            try
            {
                TestResultViewModel testResultViewModel = new TestResultViewModel(testRequest, applicationCache, servicesRepository);
                servicesRepository.WindowNavigatorService.ShowWindow(testResultViewModel);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void OnFilterTestRequests(TestRequestFilter filterType)
        {
            try
            {
                switch (filterType)
                {
                    case TestRequestFilter.All:
                        FilteredTestRequests = TestRequests;
                        break;
                    case TestRequestFilter.ForSelectedTestSet:
                        if (SelectedTestRequestGroup == null)
                        {
                            servicesRepository.DialogService.ShowInformationMessage($"You haven't selected any test set");
                        }
                        else
                        {
                            FilteredTestRequests = new ObservableCollection<TestRequest>(servicesRepository.TestRequestService.Filter(SelectedTestRequestGroup.TestSet, applicationCache.TestRequests));
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Error during filtering test requests");
            }
        }

        private IEnumerable<TestRequest> GetNotCompletedTestRequests()
        {
            return TestRequestGroups.SelectMany(x => x.TestRequests).Where(x => x.Progress < 100 && x.IsReadOnly == false);
        }

        private IEnumerable<DataSet> GetSelectedTestSets()
        {
            return this.TestRequestGroups.Where(x => x.IsSelected).Select(x => x.TestSet);
        }
    }
}