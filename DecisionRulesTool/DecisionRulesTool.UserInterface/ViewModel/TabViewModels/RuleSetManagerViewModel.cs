using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using DecisionRulesTool.UserInterface.Model.Factory;
using DecisionRulesTool.Model.RuleTester;
using Unity;
using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model;
using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.Model.IO;
using GalaSoft.MvvmLight.Command;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSetManagerViewModel : ApplicationViewModel
    {
        private RuleSetSubset selectedRuleSet;

        #region Commands
        public ICommand DeleteSubset { get; private set; }
        public ICommand EditFilters { get; private set; }
        public ICommand LoadRuleSets { get; private set; }
        public ICommand SaveRuleSetToFile { get; private set; }
        public ICommand GenerateSubsets { get; private set; }
        #endregion

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
                RaisePropertyChanged("SelectedRuleSet");
            }
        }

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
                applicationCache.RuleSets = new ObservableCollection<RuleSetSubset>(value);
                RaisePropertyChanged("RuleSets");
            }
        }
        #endregion

        public RuleSetManagerViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            InitializeCommands();
        }

        private void OnSaveRuleSetToFile()
        {
            servicesRepository.DialogService.ShowWarningMessage("Functionality not implemented yet");
        }

        private void OnEditFilters()
        {
            servicesRepository.DialogService.ShowWarningMessage("Functionality not implemented yet");
        }

        private void OnDeleteSubset()
        {
            var parentRuleSet = SelectedRuleSet.InitialRuleSet;
            if (parentRuleSet == null)
            {
                RuleSets.Remove(SelectedRuleSet);
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
            foreach (var ruleSet in servicesRepository.RuleSetLoaderService.LoadRuleSets())
            {
                RuleSets.Add(new RuleSetSubsetViewItem(ruleSet));
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
                var optionsViewModel = new RuleSubsetGenerationViewModel(SelectedRuleSet, new RuleSetSubsetViewItemFactory(), applicationCache, servicesRepository);
                if (servicesRepository.DialogService.ShowDialog(optionsViewModel) == true)
                {
                    IRuleSubsetGenerator ruleSubsetGenerator = optionsViewModel.GetSubsetGenerator();
                    ruleSubsetGenerator.GenerateSubsets();

                    servicesRepository.DialogService.ShowInformationMessage("Operation completed successfully");
                }
                else
                {
                }
            }
            else
            {
                servicesRepository.DialogService.ShowWarningMessage("To generate rule subsets, you must first select initial rule set from 'Loaded rule sets' panel");
            }


            //TODO: this line is for refresh tree view, change this
            //      so it will be refreshed without creating new collection
            RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
        }

        private void InitializeCommands()
        {
            SaveRuleSetToFile = new RelayCommand(OnSaveRuleSetToFile);
            GenerateSubsets = new RelayCommand(OnGenerateSubsets);
            LoadRuleSets = new RelayCommand(OnLoadRuleSet);
            EditFilters = new RelayCommand(OnEditFilters);
            DeleteSubset = new RelayCommand(OnDeleteSubset);
        }
    }
}
