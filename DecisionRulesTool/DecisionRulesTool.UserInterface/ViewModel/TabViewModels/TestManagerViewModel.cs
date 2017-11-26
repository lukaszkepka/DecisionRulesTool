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
    using DecisionRulesTool.UserInterface.ViewModel.Windows;

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


        public ICommand ShowTestResults { get; private set; }
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
            ShowGroupedTestResults = new RelayCommand(OnShowGroupedTestResults);
            ShowTestResults = new RelayCommand<TestRequest>(OnShowTestResults);

            ViewTestSet = new RelayCommand(OnViewTestSet);
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            FilterTestRequests = new RelayCommand<TestRequestFilter>(OnFilterTestRequests);
            GenerateTestRequests = new RelayCommand(OnGenerateTestRequests);
            DeleteSelectedTestRequest = new RelayCommand(OnDeleteSelectedTestRequest);
        }



        private async void RunTestsAsync(RuleTesterManager ruleTesterManager)
        {
            ruleTesterProgressNotifier.ProgressChanged += (s, progress) => { Progress = progress; };
            await Task.Factory.StartNew(() => ruleTesterManager.RunTesting(ruleTester));
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
            return TestRequestGroups.SelectMany(x => x.TestRequests).Where(x => x.Progress < 100);
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
            var f = TestRequestGroups.FirstOrDefault(x => x.TestSet == testRequest.TestSet);
            if (f != null)
            {
                applicationCache.TestRequests.Add(testRequest);
                f.AddTestRequest(testRequest);
            }          
        }

        public void OnLoadTestSets()
        {
            foreach (var testSet in servicesRepository.DataSetLoaderService.LoadDataSets())
            {
                AddTestSet(testSet);
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

        private void OnShowGroupedTestResults()
        {
            servicesRepository.DialogService.ShowDialog(new GroupedTestResultViewModel(SelectedTestRequestGroup, applicationCache, servicesRepository));
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

        private void OnShowTestResults(TestRequest testRequest)
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