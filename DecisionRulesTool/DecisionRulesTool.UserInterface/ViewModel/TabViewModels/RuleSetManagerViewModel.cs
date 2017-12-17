using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DecisionRulesTool.UserInterface.Model.Factory;
using GalaSoft.MvvmLight.Command;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.ViewModel.MainViewModels;
using System;
using DecisionRulesTool.UserInterface.Services.Dialog;
using DecisionRulesTool.Model.FileSavers.RSES;
using DecisionRulesTool.Model.IO;
using DecisionRulesTool.Model.IO.FileSavers.Factory;
using DecisionRulesTool.Model.FileSavers;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSetManagerViewModel : ApplicationViewModel
    {
        private RuleSetSubset selectedRuleSet;

        #region Commands
        public ICommand DeleteRuleSet { get; private set; }
        public ICommand DeleteSubsets { get; private set; }
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
                return applicationRepository.RuleSets;
            }
            set
            {
                applicationRepository.RuleSets = new ObservableCollection<RuleSetSubset>(value);
                RaisePropertyChanged("RuleSets");
            }
        }
        #endregion

        public RuleSetManagerViewModel(ApplicationRepository applicationCache, ServicesRepository servicesRepository)
            : base(applicationCache, servicesRepository)
        {
            InitializeCommands();
        }

        private void OnSaveRuleSetToFile()
        {
            try
            {
                if (selectedRuleSet != null)
                {
                    servicesRepository.RuleSetSaverService.SaveRuleSet(selectedRuleSet);
                    servicesRepository.DialogService.ShowInformationMessage($"File saved sucessfully");
                }
                else
                {
                    servicesRepository.DialogService.ShowErrorMessage($"You haven't selected any rule set");
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage($"Error during saving rule set to file : {ex.Message}");
            }
        }

        private void OnDeleteSubset()
        {
            try
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
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage("Error during deleting rule set");
            }
        }

        public void OnLoadRuleSet()
        {
            try
            {
                foreach (var ruleSet in servicesRepository.RuleSetLoaderService.LoadRuleSets())
                {
                    RuleSets.Add(new RuleSetViewModel(ruleSet));
                }

                if (RuleSets.Any())
                {
                    SelectedRuleSet = RuleSets.Last();
                }

                //TODO: this line is for refresh tree view, change this
                //      so it will be refreshed without creating new collection
                RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage("Error during adding rule sets");
            }
        }

        private void OnGenerateSubsets(RuleSetSubset ruleSet)
        {
            try
            {
                if (ruleSet != null)
                {
                    var optionsViewModel = new RuleSubsetGenerationViewModel(ruleSet, new RuleSetViewModelFactory(), applicationRepository, servicesRepository);
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
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage("Error occured during generating subsets");
            }
        }

        private void InitializeCommands()
        {
            LoadRuleSets = new RelayCommand(OnLoadRuleSet);
            SaveRuleSetToFile = new RelayCommand(OnSaveRuleSetToFile);
            GenerateSubsets = new RelayCommand<RuleSetSubset>(OnGenerateSubsets);
            DeleteSubsets = new RelayCommand<RuleSetSubset>(OnDeleteSubsets);
            DeleteRuleSet = new RelayCommand<RuleSetSubset>(OnDeleteRuleSet);
        }

        private void OnDeleteRuleSet(RuleSetSubset obj)
        {
            try
            {
                if (applicationRepository.RuleSets.Contains(obj))
                {
                    applicationRepository.RuleSets.Remove(obj);
                }
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage("Fatal error during deleting rule sets");
            }
        }

        private void OnDeleteSubsets(RuleSetSubset obj)
        {
            try
            {
                obj.Subsets.Clear();
                //TODO: this line is for refresh tree view, change this
                //      so it will be refreshed without creating new collection
                RuleSets = new ObservableCollection<RuleSetSubset>(RuleSets);
            }
            catch (Exception ex)
            {
                servicesRepository.DialogService.ShowErrorMessage("Fatal error during deleting rule sets subsets");
            }
        }
    }
}
