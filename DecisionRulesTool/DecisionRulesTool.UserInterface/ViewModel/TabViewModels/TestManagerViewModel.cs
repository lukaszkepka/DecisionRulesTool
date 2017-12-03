using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using DecisionRulesTool.Model.RuleTester;
using PropertyChanged;
using Unity;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight.Command;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
using GalaSoft.MvvmLight.Ioc;
using DecisionRulesTool.UserInterface.Model.Exceptions;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System.Data;
using ClosedXML.Excel;
using System.Threading.Tasks;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.Model.Comparers;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    using DecisionRulesTool.Model.Model;
    using DecisionRulesTool.Model.Parsers;
    using DecisionRulesTool.Model.RuleTester.Result;
    using DecisionRulesTool.Model.RuleTester.Result.Interfaces;
    using DecisionRulesTool.UserInterface.ViewModel.Results;
    using DecisionRulesTool.UserInterface.ViewModel.Windows;
    using System.IO;

    [AddINotifyPropertyChangedInterface]
    public class TestManagerViewModel : ApplicationViewModel
    {
        private TestRequest selectedTestRequest;
        private TestRequestGroup selectedTestRequestAggregate;
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
        public ICommand DeleteSelectedTestRequest { get; private set; }
        public ICommand LoadTestResult { get; private set; }
        public ICommand ShowTestResults { get; private set; }
        public ICommand SaveAllResults { get; private set; }
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
            ShowGroupedTestResults = new RelayCommand(OnShowResultsForTestSet);
            ShowTestResults = new RelayCommand<TestRequest>(OnShowSingleTestResult);

            SaveAllResults = new RelayCommand(OnSaveAllResults);
            ViewTestSet = new RelayCommand(OnViewTestSet);
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            LoadTestResult = new RelayCommand(OnLoadTestResult);
            FilterTestRequests = new RelayCommand<TestRequestFilter>(OnFilterTestRequests);
            GenerateTestRequests = new RelayCommand(OnGenerateTestRequests);
            DeleteSelectedTestRequest = new RelayCommand(OnDeleteSelectedTestRequest);
        }

        private void OnSaveAllResults()
        {
            string folderPath = servicesRepository.DialogService.BrowseFolderDialog(Environment.CurrentDirectory);
            if (!string.IsNullOrEmpty(folderPath))
            {
                foreach (var testRequest in applicationCache.TestRequests.Where(x => x.Progress == 100))
                {
                    TestResultViewModel testResultViewModel = new TestResultViewModel(testRequest, applicationCache, servicesRepository);
                    testResultViewModel.SaveResultToFile($"{folderPath}\\{DateTime.Now.ToString("yyyyMMdd")}{testRequest.GetShortenName()}.xlsx");
                }

                servicesRepository.DialogService.ShowInformationMessage("Saving results completed successfully");
            }
        }

        private void OnLoadTestResult()
        {
            IFileParser<TestRequest> testResultLoader = new TestResultLoader();

            OpenFileDialogSettings settings = new OpenFileDialogSettings()
            {
                Multiselect = true
            };

            string[] paths = servicesRepository.DialogService.OpenFileDialog(settings);
            foreach (string path in paths)
            {
                try
                {
                    AddTestRequest(testResultLoader.ParseFile(path));
                }
                catch (Exception ex)
                {
                    servicesRepository.DialogService.ShowInformationMessage(ex.Message);
                }
            }
        }

        private async void RunTestsAsync(RuleTesterManager ruleTesterManager)
        {
            ruleTesterProgressNotifier.ProgressChanged += (s, progress) => { Progress = progress; };
            await Task.Factory.StartNew(() => ruleTesterManager.RunTesting(ruleTester));
            //Refactor:
            SimpleIoc.Default.GetInstance<TestResultComparisionViewModel>().OnCalculateResultTable();
            //
            ruleTesterProgressNotifier.ProgressChanged -= (s, progress) => { Progress = progress; }; ;
        }

        private TestRequestGeneratorViewModel InstantiateTestRequestGeneratorViewModel()
        {
            TestRequestGeneratorViewModel viewModel = SimpleIoc.Default.GetInstance<TestRequestGeneratorViewModel>();
            viewModel.TestSets = GetSelectedTestSets();
            return viewModel;
        }

        private IEnumerable<TestRequest> GetNotCompletedTestRequests()
        {
            return TestRequestGroups.SelectMany(x => x.TestRequests).Where(x => x.Progress < 100 && x.IsReadOnly == false);
        }

        private IEnumerable<DataSet> GetSelectedTestSets()
        {
            return this.TestRequestGroups.Where(x => x.IsSelected).Select(x => x.TestSet);
        }

        private void AddTestSet(DataSet testSet)
        {
            applicationCache.TestSets.Add(testSet);
            TestRequestGroups.Add(new TestRequestGroup(testSet, new ObservableCollection<TestRequest>()));
        }

        private void AddTestRequest(TestRequest testRequest)
        {
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
        }

        public void OnLoadTestSets()
        {
            foreach (var testSet in servicesRepository.DataSetLoaderService.LoadDataSets())
            {
                if(applicationCache.TestSets.Any(x => x.Name.Equals(testSet.Name)))
                {
                    servicesRepository.DialogService.ShowInformationMessage($"Test Set with name {testSet.Name} is already loaded");
                }
                else
                {
                    AddTestSet(testSet);
                }
            }
        }

        private void OnViewTestSet()
        {
            try
            {
                TestSetViewModel testSetDialogViewModel = new TestSetViewModel(SelectedTestRequestGroup.TestSet, applicationCache, servicesRepository);
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
                    foreach (var testRequest in testRequestGeneratorViewModel.GenerateTestRequests())
                    {
                        AddTestRequest(testRequest);
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

        private void OnDeleteSelectedTestRequest()
        {
            applicationCache.TestRequests.Remove(SelectedTestRequest);
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
        private void OnShowResultsForTestSet()
        {
            servicesRepository.DialogService.ShowDialog(new AlgorithmsToTestSetsResultViewModel(SelectedTestRequestGroup, applicationCache, servicesRepository));
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
            switch (filterType)
            {
                case TestRequestFilter.All:
                    FilteredTestRequests = TestRequests;
                    break;
                case TestRequestFilter.ForSelectedTestSet:
                    FilteredTestRequests = new ObservableCollection<TestRequest>(servicesRepository.TestRequestService.Filter(SelectedTestRequestGroup.TestSet, applicationCache.TestRequests));
                    break;
                default:
                    break;
            }
        }
    }
}