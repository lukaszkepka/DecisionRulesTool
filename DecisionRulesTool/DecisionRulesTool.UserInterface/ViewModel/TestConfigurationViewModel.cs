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
using DecisionRulesTool.UserInterface.Model.Commands;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class TestConfiguratorViewModel : BaseWindowViewModel
    {
        private ICollection<TestRequest> generatedTestRequests;

        #region Commands
        public ICommand LoadTestSets { get; private set; }
        public ICommand ViewTestSet { get; private set; }
        public ICommand RunTests { get; private set; }
        public ICommand FilterTestRequests { get; private set; }
        public ICommand GenerateTestRequests { get; private set; }
        public ICommand DeleteSelectedTestRequest { get; private set; }
        #endregion

        #region Properties
        public ICollection<RuleSetSubset> RuleSets { get; private set; }
        public ICollection<TestRequest> FilteredTestRequests { get; private set; }
        public ICollection<DataSet> TestSets { get; private set; }
        public RuleSetSubset SelectedRuleSet { get; set; }
        public TestRequest SelectedTestRequest { get; set; }
        public DataSet SelectedTestSet { get; set; }
        #endregion

        public TestConfiguratorViewModel(ICollection<RuleSetSubset> loadedRuleSets, IUnityContainer container) : base(container)
        {
            InitializeCommands();

            this.RuleSets = new ObservableCollection<RuleSetSubset>(loadedRuleSets);
            this.TestSets = new ObservableCollection<DataSet>();
            this.generatedTestRequests = new ObservableCollection<TestRequest>();
        }

        protected override void OnMoveToTestResultViewer()
        {
            windowNavigatorService.SwitchContext(new TestResultViewerViewModel(generatedTestRequests, containter));
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
                TestSetViewModel testSetDialogViewModel = new TestSetViewModel(SelectedTestSet, containter);
                dialogService.ShowDialog(testSetDialogViewModel);
            }
            catch (Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message})");
            }
        }

        private void OnGenerateTestRequests(object generationType)
        {
            try
            {
                TestRequestGeneratorViewModel testRequestGeneratorViewModel = InstantiateTestRequestGeneratorViewModel(generationType);

                if (testRequestGeneratorViewModel != null && dialogService.ShowDialog(testRequestGeneratorViewModel) == true)
                {
                    foreach (var testRequest in testRequestGeneratorViewModel.GenerateTestRequests())
                    {
                        generatedTestRequests.Add(testRequest);
                    }
                    OnFilterTestRequests("All");
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
            GenerateTestRequests = new ParameterCommand(OnGenerateTestRequests);
            FilterTestRequests = new ParameterCommand(OnFilterTestRequests);
            DeleteSelectedTestRequest = new RelayCommand(OnDeleteSelectedTestRequest);
        }

        private void OnFilterTestRequests(object filterType)
        {
            switch (filterType?.ToString())
            {
                case "All":
                    FilteredTestRequests = generatedTestRequests;
                    break;
                case "ForSelectedRuleSet":
                    FilteredTestRequests = new ObservableCollection<TestRequest>(testRequestService.Filter(SelectedRuleSet, generatedTestRequests));
                    break;
                case "ForSelectedTestSet":
                    FilteredTestRequests = new ObservableCollection<TestRequest>(testRequestService.Filter(SelectedTestSet, generatedTestRequests));
                    break;
                default:
                    break;
            }
        }

        private void OnDeleteSelectedTestRequest()
        {
            generatedTestRequests.Remove(SelectedTestRequest);
        }

        private TestRequestGeneratorViewModel InstantiateTestRequestGeneratorViewModel(object generationType)
        {
            TestRequestGeneratorViewModel testRequestGeneratorViewModel = null;

            switch (generationType.ToString())
            {
                case "FromRuleSet":
                    testRequestGeneratorViewModel = new TestRequestFromRuleSetGeneratorViewModel(TestSets, SelectedRuleSet, containter);
                    break;
                case "FromTestSet":
                    testRequestGeneratorViewModel = new TestRequestFromTestSetGeneratorViewModel(RuleSets, SelectedTestSet, containter);
                    break;
                default:
                    break;
            }

            return testRequestGeneratorViewModel;
        }
    }
}