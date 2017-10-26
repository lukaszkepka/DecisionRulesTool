using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;
using DecisionRulesTool.Model.RuleSubsetGeneration;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.ViewModel.Filters;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSubsetGenerationViewModel : BaseDialogViewModel
    {
        private IRuleSetSubsetFactory ruleSetSubsetFactory;
        private IList<FilterViewModel> filterViewModels;
        private FilterViewModel selectedFilterViewModel;
        private RuleSetSubset rootRuleSet;

        public ICommand Apply { get; private set; }
        public ICommand Cancel { get; private set; }
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
                OnPropertyChanged("SelectedFilterViewModel");
            }
        }
        #endregion

        public RuleSubsetGenerationViewModel(RuleSetSubset rootRuleSet, IRuleSetSubsetFactory ruleSetSubsetFactory)
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
            Apply = new RelayCommand(OnApply);
            Cancel = new RelayCommand(OnCancel);
            MoveViewModelLeft = new RelayCommand(OnMoveViewModelLeft);
            MoveViewModelRight = new RelayCommand(OnMoveViewModelRight);
        }

        private void InitializeFilterViewModels()
        {
            this.filterViewModels = new ObservableCollection<FilterViewModel>()
            {
                new SupportValueFilterViewModel(rootRuleSet, ruleSetSubsetFactory),
                new LengthFilterViewModel(rootRuleSet, ruleSetSubsetFactory),
                new AttributePresenceFilterViewModel(rootRuleSet, ruleSetSubsetFactory)
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

        public void OnApply()
        {
            Result = true;
            OnCloseRequest();
        }

        public void OnCancel()
        {
            Result = false;
            OnCloseRequest();
        }
    }
}
