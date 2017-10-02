using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.RuleFilters;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services.Dialog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public MainWindowViewModel(DialogService dialogService) : base(dialogService)
        {
            RuleSets = new ObservableCollection<RuleSetSubset>();
        }
        private void OnSaveRuleSetToFile()
        {
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
            if(SelectedRuleSet != null)
            {
                RuleSetSubsetGenerator a = new RuleSetSubsetGenerator(SelectedRuleSet);
                a.a();
                OnPropertyChanged("RuleSets");
            }
            else
            {

            }
        }
        protected override void InitializeCommands()
        {
            SaveRuleSetToFile = new Command(OnSaveRuleSetToFile);
            LoadRuleSets = new Command(OnLoadRuleSet);
            GenerateSubsets = new Command(OnGenerateSubsets);
        }
    }
}
