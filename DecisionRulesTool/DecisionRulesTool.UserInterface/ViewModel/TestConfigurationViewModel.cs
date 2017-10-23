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
        public ICommand AddRuleSets { get; private set; }

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
            this.TestSets = new ObservableCollection<DataSet>();
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

        private void OnAddRuleSets()
        {
            try
            {
                RuleSetPickerViewModel ruleSetPickerViewModel = new RuleSetPickerViewModel(loadedRuleSets, SelectedTestSet);
                dialogService.ShowDialog(ruleSetPickerViewModel);

                var t = ruleSetPickerViewModel.GetSelectedRuleSets();

                RuleTesterManager ruleManager = new RuleTesterManager();
                foreach (var item in ruleManager.GenerateTests(SelectedTestSet, t))
                {
                    testRequests.Add(item);
                }

            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void InitializeCommands()
        {
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            ViewTestSet = new RelayCommand(OnViewTestSet);
            AddRuleSets = new RelayCommand(OnAddRuleSets);
        }
    }
}