using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.RuleFilters;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.UserInterface.ViewModel.Dialog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        private ICollection<RuleSetSubset> ruleSets;
        private RuleSetSubset selectedRuleSet;

        public ICommand LoadRuleSets { get; private set; }
        public ICommand SaveRuleSetToFile { get; private set; }
        public ICommand GenerateSubsets { get; private set; }

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
        #endregion

        public MainWindowViewModel() : base()
        {
            RuleSets = new ObservableCollection<RuleSetSubset>();
            InitializeCommands();
        }
        private void OnSaveRuleSetToFile()
        {
            //TODO: this line is for refresh tree view, change this
            //      so it will be refreshed without creating new collection
            RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
            dialogService.ShowWarningMessage("Functionality not implemented yet");
        }
        public void OnLoadRuleSet()
        {
            foreach (var ruleSet in ruleSetLoaderService.LoadRuleSets())
            {
                RuleSets.Add(new RuleSetSubset(ruleSet, ruleSet));
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
            //IProgressNotifier subsetGenerationProgressNotifier = new ProgressNotifier();
            //var progressDialogViewModel = new ProgressDialogViewModel(subsetGenerationProgressNotifier);

            if (SelectedRuleSet != null)
            {
                var optionsViewModel = new RuleSubsetGenerationViewModel(SelectedRuleSet);
                if (dialogService.ShowDialog(optionsViewModel) == true)
                {
                    LengthSeriesFilter ruleSubsetGenerator = optionsViewModel.GetSubsetGenerator();
                    ruleSubsetGenerator.ApplyFilterSeries(SelectedRuleSet);

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
        }
    }
}
