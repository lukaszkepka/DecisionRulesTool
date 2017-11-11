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

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TestConfiguratorViewModel : ApplicationContextViewModel
    {
        public enum TestRequestFilter
        {
            All,
            ForSelectedTestSet,
            ForSelcetedRuleSet
        }

        #region Commands
        public ICommand LoadTestSets { get; private set; }
        public ICommand ViewTestSet { get; private set; }
        public ICommand FilterTestRequests { get; private set; }
        public ICommand GenerateTestRequests { get; private set; }
        public ICommand DeleteSelectedTestRequest { get; private set; }
        #endregion

        #region Properties

        /// <summary>
        /// Wrapper property for application's cache rule set collection
        /// </summary>
        public ICollection<RuleSetSubset> RuleSets
        {
            get
            {
                return applicationCache.RuleSets;
            }
            set
            {
                applicationCache.RuleSets = value;
                RaisePropertyChanged("RuleSets");
            }
        }

        /// <summary>
        /// Wrapper property for application's cache data set collection
        /// </summary>
        public ICollection<DataSet> TestSets
        {
            get
            {
                return applicationCache.TestSets;
            }
            set
            {
                applicationCache.TestSets = value;
                RaisePropertyChanged("TestSets");
            }
        }

        /// <summary>
        /// Wrapper property for application's cache test request collection
        /// </summary>
        public ICollection<TestRequest> TestRequests
        {
            get
            {
                return applicationCache.TestRequests;
            }
            set
            {
                applicationCache.TestRequests = value;
                RaisePropertyChanged("TestRequests");
            }
        }

        public ICollection<TestRequest> FilteredTestRequests { get; private set; }
        public RuleSetSubset SelectedRuleSet { get; set; }
        public TestRequest SelectedTestRequest { get; set; }
        public DataSet SelectedTestSet { get; set; }
        #endregion

        #region Constructors
        public TestConfiguratorViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            OnFilterTestRequests(TestRequestFilter.All);
            InitializeCommands();
        }
        #endregion

        public void OnLoadTestSets()
        {
            foreach (var testSet in servicesRepository.DataSetLoaderService.LoadDataSets())
            {
                TestSets.Add(testSet);
            }

            if (TestSets.Any())
            {
                SelectedTestSet = TestSets.Last();
            }

            //TODO: this line is for refresh tree view, change this
            //      so it will be refreshed without creating new collection
            TestSets = new ObservableCollection<DataSet>(TestSets);
        }

        private void OnViewTestSet()
        {
            try
            {
                TestSetViewModel testSetDialogViewModel = new TestSetViewModel(SelectedTestSet, servicesRepository);
                servicesRepository.DialogService.ShowDialog(testSetDialogViewModel);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void OnGenerateTestRequests(object generationType)
        {
            try
            {
                TestRequestGeneratorViewModel testRequestGeneratorViewModel = InstantiateTestRequestGeneratorViewModel(generationType);

                if (testRequestGeneratorViewModel != null && servicesRepository.DialogService.ShowDialog(testRequestGeneratorViewModel) == true)
                {
                    foreach (var testRequest in testRequestGeneratorViewModel.GenerateTestRequests())
                    {
                        TestRequests.Add(testRequest);
                    }
                    OnFilterTestRequests(TestRequestFilter.All);
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void InitializeCommands()
        {
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            ViewTestSet = new RelayCommand(OnViewTestSet);
            GenerateTestRequests = new RelayCommand<object>(OnGenerateTestRequests);
            FilterTestRequests = new RelayCommand<TestRequestFilter>(OnFilterTestRequests);
            DeleteSelectedTestRequest = new RelayCommand(OnDeleteSelectedTestRequest);
        }

        private void OnFilterTestRequests(TestRequestFilter filterType)
        {
            switch (filterType)
            {
                case TestRequestFilter.All:
                    FilteredTestRequests = TestRequests;
                    break;
                case TestRequestFilter.ForSelectedTestSet:
                    FilteredTestRequests = new ObservableCollection<TestRequest>(servicesRepository.TestRequestService.Filter(SelectedRuleSet, TestRequests));
                    break;
                case TestRequestFilter.ForSelcetedRuleSet:
                    FilteredTestRequests = new ObservableCollection<TestRequest>(servicesRepository.TestRequestService.Filter(SelectedTestSet, TestRequests));
                    break;
                default:
                    break;
            }
        }

        private void OnDeleteSelectedTestRequest()
        {
            TestRequests.Remove(SelectedTestRequest);
        }

        private TestRequestGeneratorViewModel InstantiateTestRequestGeneratorViewModel(object generationType)
        {
            TestRequestGeneratorViewModel testRequestGeneratorViewModel = null;

            switch (generationType.ToString())
            {
                case "FromRuleSet":
                    testRequestGeneratorViewModel = new TestRequestFromRuleSetGeneratorViewModel(TestSets, SelectedRuleSet, servicesRepository);
                    break;
                case "FromTestSet":
                    testRequestGeneratorViewModel = new TestRequestFromTestSetGeneratorViewModel(RuleSets, SelectedTestSet, servicesRepository);
                    break;
                default:
                    break;
            }

            return testRequestGeneratorViewModel;
        }
    }
}