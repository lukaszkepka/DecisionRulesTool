using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System.Windows.Input;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.Model.RuleFilters;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.ViewModel.Filters;
using System.Collections.ObjectModel;
using DecisionRulesTool.Model.RuleSubsetGeneration;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSubsetGenerationViewModel : BaseDialogViewModel
    {
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

        public RuleSubsetGenerationViewModel(RuleSetSubset rootRuleSet)
        {
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
                new SupportValueFilterViewModel(rootRuleSet),
                new LengthFilterViewModel(rootRuleSet),
                new AttributePresenceFilterViewModel(rootRuleSet)
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
