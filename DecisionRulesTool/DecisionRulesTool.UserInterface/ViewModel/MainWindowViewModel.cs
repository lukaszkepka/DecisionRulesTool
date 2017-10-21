using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        private ICollection<RuleSetSubset> ruleSets;
        private ICollection<string> logs;
        private RuleSetSubset selectedRuleSet;

        public ICommand DeleteSubset { get; private set; }
        public ICommand EditFilters { get; private set; }
        public ICommand LoadRuleSets { get; private set; }
        public ICommand SaveRuleSetToFile { get; private set; }
        public ICommand GenerateSubsets { get; private set; }
        public ICommand ConfigureTests { get; private set; }

        #region Properties
        public RuleSetSubset SelectedRuleSet
        {
            get
            {
                return selectedRuleSet;
            }
            set
            {
                selectedRuleSet = value;
                OnPropertyChanged("SelectedRuleSet");
            }
        }
        public ICollection<RuleSetSubset> RuleSets
        {
            get
            {
                return ruleSets;
            }
            set
            {
                ruleSets = value;
                OnPropertyChanged("RuleSets");
            }
        }

        public ICollection<string> Logs
        {
            get
            {
                return logs;
            }
            set
            {
                logs = value;
                OnPropertyChanged("Logs");
            }
        }
        #endregion

        public MainWindowViewModel() : base()
        {
            RuleSets = new ObservableCollection<RuleSetSubset>();
            Logs = new ObservableCollection<string>();
            InitializeCommands();
        }

        private void OnConfigureTests()
        {
            try
            {
                TestConfigurationViewModel testConfigurationViewModel = new TestConfigurationViewModel();
                windowNavigatorService.SwitchContext(testConfigurationViewModel);
            }
            catch(Exception ex)
            {
                dialogService.ShowInformationMessage($"Exception thrown : {ex.Message}");
            }
        }

        private void OnSaveRuleSetToFile()
        {
            dialogService.ShowWarningMessage("Functionality not implemented yet");
        }
        private void OnEditFilters()
        {
            dialogService.ShowWarningMessage("Functionality not implemented yet");
        }

        private void OnDeleteSubset()
        {
            var parentRuleSet = SelectedRuleSet.InitialRuleSet;
            if (parentRuleSet == null)
            {
                ruleSets.Remove(SelectedRuleSet);
            }
            else
            {
                parentRuleSet.Subsets.Remove(SelectedRuleSet);
                SelectedRuleSet = parentRuleSet;
            }

            //TODO: this line is for refresh tree view, change this
            //      so it will be refreshed without creating new collection
            RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
        }

        public void OnLoadRuleSet()
        {
            foreach (var ruleSet in ruleSetLoaderService.LoadRuleSets())
            {
                RuleSets.Add(new RuleSetSubset(ruleSet));
            }

            if (RuleSets.Any())
            {
                SelectedRuleSet = RuleSets.Last();
            }

            //TODO: this line is for refresh tree view, change this
            //      so it will be refreshed without creating new collection
            RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
        }

        private void OnGenerateSubsets()
        {
            if (SelectedRuleSet != null)
            {
                var optionsViewModel = new RuleSubsetGenerationViewModel(SelectedRuleSet);
                if (dialogService.ShowDialog(optionsViewModel) == true)
                {
                    IRuleSubsetGenerator ruleSubsetGenerator = optionsViewModel.GetSubsetGenerator();
                    ruleSubsetGenerator.GenerateSubsets();

                    dialogService.ShowInformationMessage("Operation completed successfully");
                }
                else
                {
                }
            }
            else
            {
                dialogService.ShowWarningMessage("To generate rule subsets, you must first select initial rule set from 'Loaded rule sets' panel");
            }


            //TODO: this line is for refresh tree view, change this
            //      so it will be refreshed without creating new collection
            RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
        }

        private void InitializeCommands()
        {
            SaveRuleSetToFile = new RelayCommand(OnSaveRuleSetToFile);
            LoadRuleSets = new RelayCommand(OnLoadRuleSet);
            GenerateSubsets = new RelayCommand(OnGenerateSubsets);
            EditFilters = new RelayCommand(OnEditFilters);
            DeleteSubset = new RelayCommand(OnDeleteSubset);
            ConfigureTests = new RelayCommand(OnConfigureTests);
        }
    }
}
