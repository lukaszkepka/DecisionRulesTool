using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Model;

namespace DecisionRulesTool.UserInterface.ViewModel.Filters
{
    public class SupportValueFilterViewModel : RelationFilterViewModel
    {
        private int supportFilterLowerBound = 1;
        private int supportFilterUpperBound = 1;
        private int minSupportFilter = 1;
        private int maxSupportFilter = 1;

        #region Properties
        public int MinSupportFilter
        {
            get
            {
                return minSupportFilter;
            }
            set
            {
                if (value > MaxSupportFilter)
                {
                    //TODO
                }
                else if (value >= supportFilterLowerBound)
                {
                    minSupportFilter = value;
                }
                OnPropertyChanged("MinSupportFilter");
            }
        }
        public int MaxSupportFilter
        {
            get
            {
                return maxSupportFilter;
            }
            set
            {
                if (value > supportFilterUpperBound)
                {
                    maxSupportFilter = supportFilterUpperBound;
                }
                else if (value < MinSupportFilter)
                {
                    //TODO
                }
                else
                {
                    maxSupportFilter = value;
                }
                OnPropertyChanged("MaxSupportFilter");
            }
        }
        #endregion

        public SupportValueFilterViewModel(RuleSetSubset rootRuleSet) : base(rootRuleSet)
        {
            SetFilterBounds();
        }

        private void SetFilterBounds()
        {
            supportFilterLowerBound = 1;

            if (rootRuleSet.Rules.Any())
            {
                //Support filter
                supportFilterUpperBound = rootRuleSet.Rules.Max(x => x.SupportValue);
                maxSupportFilter = supportFilterUpperBound;
            }
            else
            {
                //Support filter
                supportFilterUpperBound = 0;
                supportFilterLowerBound = 0;
                minSupportFilter = 0;
                maxSupportFilter = 0;
            }
        }

        public override IRuleFilterApplier GetRuleSeriesFilter()
        {
            IRuleFilterApplier ruleFilterApplier = default(IRuleFilterApplier);
            if (isEnabled)
            {
                ruleFilterApplier = new SupportValueFilterApplier(minSupportFilter, maxSupportFilter, SelectedRelation);
            }
            return ruleFilterApplier;
        }
    }
}
