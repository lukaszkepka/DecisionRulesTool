using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;
using DecisionRulesTool.Model.RuleSubsetGeneration;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.ViewModel.Filters;
using Unity;
using GalaSoft.MvvmLight.Command;
using DecisionRulesTool.UserInterface.Services;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSubsetGenerationViewModel : BaseDialogViewModel
    {
        private IRuleSetSubsetFactory ruleSetSubsetFactory;
        private IList<FilterViewModel> filterViewModels;
        private FilterViewModel selectedFilterViewModel;
        private RuleSetSubset rootRuleSet;

        public ICommand MoveViewModelLeft { get; private set; }
        public ICommand MoveViewModelRight { get; private set; }

        #region Properties
        public ICollection<FilterViewModel> Filters
        {
            get
            {
                return filterViewModels;
            }
        }
        public FilterViewModel SelectedFilterViewModel
        {
            get
            {
                return selectedFilterViewModel;
            }
            set
            {
                selectedFilterViewModel = value;
                RaisePropertyChanged("SelectedFilterViewModel");
            }
        }
        #endregion

        public RuleSubsetGenerationViewModel(RuleSetSubset rootRuleSet, IRuleSetSubsetFactory ruleSetSubsetFactory, ServicesRepository servicesRepository) 
            : base(servicesRepository)
        {
            this.ruleSetSubsetFactory = ruleSetSubsetFactory;
            this.rootRuleSet = rootRuleSet;

            InitializeFilterViewModels();
            InitializeCommands();
        }

        public IRuleSubsetGenerator GetSubsetGenerator()
        {
            IRuleSubsetGenerator ruleSubsetGenerator = new LowestLevelSubsetGenerator(rootRuleSet);
            foreach (var filter in Filters)
            {
                ruleSubsetGenerator.AddFilterApplier(filter.GetRuleSeriesFilter());
            }
            return ruleSubsetGenerator;
        }

        private void InitializeCommands()
        {
            MoveViewModelLeft = new RelayCommand(OnMoveViewModelLeft);
            MoveViewModelRight = new RelayCommand(OnMoveViewModelRight);
        }

        private void InitializeFilterViewModels()
        {
            this.filterViewModels = new ObservableCollection<FilterViewModel>()
            {
                new SupportValueFilterViewModel(rootRuleSet, ruleSetSubsetFactory, servicesRepository),
                new LengthFilterViewModel(rootRuleSet, ruleSetSubsetFactory, servicesRepository),
                new AttributePresenceFilterViewModel(rootRuleSet, ruleSetSubsetFactory, servicesRepository)
            };

            SelectedFilterViewModel = filterViewModels[1];
        }

        private void OnMoveViewModelLeft()
        {
            FilterViewModel viewModel = SelectedFilterViewModel;
            int i = filterViewModels.IndexOf(viewModel);
            if(i > 0)
            {
                filterViewModels.RemoveAt(i);
                filterViewModels.Insert(i - 1, viewModel);
            }
            else
            {
                filterViewModels.RemoveAt(i);
                filterViewModels.Add(viewModel);
            }

            SelectedFilterViewModel = viewModel;
        }

        private void OnMoveViewModelRight()
        {
            FilterViewModel viewModel = SelectedFilterViewModel;
            int i = filterViewModels.IndexOf(viewModel);
            if (i == filterViewModels.Count - 1)
            {
                filterViewModels.RemoveAt(i);
                filterViewModels.Insert(0, viewModel);
            }
            else
            {
                filterViewModels.RemoveAt(i);
                filterViewModels.Insert(i + 1, viewModel);
            }

            SelectedFilterViewModel = viewModel;
        }
    }
}
