using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel.Filters
{
    public abstract class FilterViewModel : BaseWindowViewModel
    {
        protected RuleSetSubset rootRuleSet;
        protected bool isEnabled;

        #region Properties
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        #endregion IsEnabled

        public FilterViewModel(RuleSetSubset rootRuleSet)
        {
            this.rootRuleSet = rootRuleSet;
        }

        public abstract IRuleFilterApplier[] GetRuleSeriesFilter();
    }
}
