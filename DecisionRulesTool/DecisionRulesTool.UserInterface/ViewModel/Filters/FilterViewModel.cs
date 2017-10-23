using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;
using DecisionRulesTool.Model.RuleFilters.Appliers;
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
        protected IRuleSetSubsetFactory ruleSetSubsetFactory;
        protected RuleSetSubset rootRuleSet;
        protected bool isEnabled;
        protected string name;

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
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

        public FilterViewModel(RuleSetSubset rootRuleSet, IRuleSetSubsetFactory ruleSetSubsetFactory)
        {
            this.rootRuleSet = rootRuleSet;
            this.ruleSetSubsetFactory = ruleSetSubsetFactory;

            name = this.ToString();
        }

        public abstract IRuleFilterApplier GetRuleSeriesFilter();
    }
}
