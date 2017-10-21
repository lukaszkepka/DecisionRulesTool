using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    internal class TestConfigurationViewModel : BaseWindowViewModel
    {
        private ICollection<DataSet> testSets;
        private DataSet selectedTestSet;

        public ICommand LoadTestSets { get; private set; }
        public ICommand ViewTestSet { get; private set; }

        #region Properties
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

        public TestConfigurationViewModel()
        {
            InitializeCommands();

            TestSets = new ObservableCollection<DataSet>();
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
            catch(Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message})"); 
            }
        }

        private void InitializeCommands()
        {
            LoadTestSets = new RelayCommand(OnLoadTestSets);
            ViewTestSet = new RelayCommand(OnViewTestSet);
        }

    }
}