using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Model.Factory;
using DecisionRulesTool.Model.RuleFilters.Appliers;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.UserInterface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

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
                RaisePropertyChanged("Name");
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
                RaisePropertyChanged("IsEnabled");
            }
        }
        #endregion IsEnabled

        public FilterViewModel(RuleSetSubset rootRuleSet, IRuleSetSubsetFactory ruleSetSubsetFactory, ServicesRepository servicesRepository) 
            : base(servicesRepository)
        {
            this.rootRuleSet = rootRuleSet;
            this.ruleSetSubsetFactory = ruleSetSubsetFactory;

            name = this.ToString();
        }

        public abstract IRuleFilterApplier GetRuleSeriesFilter();
    }
}
