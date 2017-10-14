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

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSubsetGenerationViewModel : BaseDialogViewModel
    {
        private IRuleSubsetGenerator ruleSubsetGenerator;
        private IList<FilterViewModel> filters;
        private FilterViewModel selectedFilterViewModel;
        private RuleSetSubset rootRuleSet;

        public ICommand Apply { get; private set; }
        public ICommand Cancel { get; private set; }
        public ICommand MoveViewModelLeft { get; private set; }

        #region Properties
        public ICollection<FilterViewModel> Filters
        {
            get
            {
                return filters;
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
            this.ruleSubsetGenerator = new RuleSetSubsetGeneratorOLD(rootRuleSet);
            this.rootRuleSet = rootRuleSet;

            InitializeFilterViewModels();
            InitializeCommands();
        }

        public IRuleSubsetGenerator GetSubsetGenerator()
        {
            IRuleSubsetGenerator ruleSubsetGenerator = new RuleSetSubsetGenerator(rootRuleSet);
            foreach (var filter in Filters)
            {
                ruleSubsetGenerator.AddFilter(filter.GetRuleSeriesFilter());
            }
            return ruleSubsetGenerator;
        }

        private void InitializeCommands()
        {
            Apply = new RelayCommand(OnApply);
            Cancel = new RelayCommand(OnCancel);
            MoveViewModelLeft = new RelayCommand(OnMoveViewModelLeft);
        }

        private void OnMoveViewModelLeft()
        {
            FilterViewModel viewModel = SelectedFilterViewModel;
            int i = filters.IndexOf(viewModel);
            if(i > 0)
            {
                filters.RemoveAt(i);
                filters.Insert(i - 1, viewModel);
            }
            else
            {
                filters.RemoveAt(i);
                filters.Add(viewModel);
            }

            SelectedFilterViewModel = viewModel;
        }

        private void InitializeFilterViewModels()
        {
            this.filters = new ObservableCollection<FilterViewModel>()
            {
                new SupportValueFilterViewModel(rootRuleSet),
                new LengthFilterViewModel(rootRuleSet),
                new AttributePresenceFilterViewModel(rootRuleSet)
            };

            SelectedFilterViewModel = filters[1];
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
