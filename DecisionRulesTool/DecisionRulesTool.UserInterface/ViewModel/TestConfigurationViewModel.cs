using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using DecisionRulesTool.Model.RuleTester;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class TestConfigurationViewModel : BaseWindowViewModel
    {
        private ICollection<TestRequest> testRequests;
        private ICollection<RuleSetSubset> loadedRuleSets;
        private ICollection<DataSet> testSets;
        private DataSet selectedTestSet;

        public ICommand LoadTestSets { get; private set; }
        public ICommand ViewTestSet { get; private set; }
        public ICommand RunTests { get; private set; }
        public ICommand SelectRuleSet { get; private set; }
        public ICommand UnselectRuleSet { get; private set; }
        public ICommand GenerateTestRequests { get; private set; }

        #region Properties
        public ICollection<TestRequest> TestRequests
        {
            get
            {
                return testRequests;
            }
            set
            {
                testRequests = value.ToList();
                OnPropertyChanged("TestRequests");
            }
        }
        public ICollection<DataSet> TestSets
        {
            get
            {
                return testSets;
            }
            set
            {
                testSets = value;
                OnPropertyChanged("TestSets");
            }
        }
        public DataSet SelectedTestSet
        {
            get
            {
                return selectedTestSet;
            }
            set
            {
                selectedTestSet = value;
                OnPropertyChanged("SelectedTestSet");
            }
        }
        #endregion

        public TestConfigurationViewModel(ICollection<RuleSetSubset> loadedRuleSets)
        {
            InitializeCommands();

            this.loadedRuleSets = loadedRuleSets;
            this.testRequests = new ObservableCollection<TestRequest>();
            this.testSets = new ObservableCollection<DataSet>();
        }

        public void OnLoadTestSets()
        {
            foreach (var testSet in dataSetLoaderService.LoadDataSets())
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
                TestSetViewModel testSetDialogViewModel = new TestSetViewModel(SelectedTestSet);
                dialogService.ShowDialog(testSetDialogViewModel);
            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void OnGenerateTestRequests()
        {
            try
            {
                TestRequestGeneratorViewModel testRequestGeneratorViewModel = new TestRequestGeneratorViewModel(loadedRuleSets, SelectedTestSet);
                dialogService.ShowDialog(testRequestGeneratorViewModel);

                foreach (var testRequest in testRequestGeneratorViewModel.GetTestRequests())
                {
                    testRequests.Add(testRequest);
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void OnUnselectRuleSet()
        {
            
        }

        private void OnSelectRuleSet()
        {
            
        }

        private void InitializeCommands()
        {
            RunTests = new RelayCommand(OnRunTests);
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            ViewTestSet = new RelayCommand(OnViewTestSet);
            SelectRuleSet = new RelayCommand(OnSelectRuleSet);
            UnselectRuleSet = new RelayCommand(OnUnselectRuleSet);
            GenerateTestRequests = new RelayCommand(OnGenerateTestRequests);
        }

        private void OnRunTests()
        {
            windowNavigatorService.SwitchContext(new TestManagerViewModel(testRequests));
        }
    }
}