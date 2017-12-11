using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.Model.Model.Factory;
using DecisionRulesTool.Model.RuleFilters.Appliers;
using Unity;
using DecisionRulesTool.UserInterface.Services;

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
                RaisePropertyChanged("MinSupportFilter");
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
                RaisePropertyChanged("MaxSupportFilter");
            }
        }
        #endregion

        public SupportValueFilterViewModel(RuleSetSubset rootRuleSet, IRuleSetSubsetFactory ruleSetSubsetFactory, ServicesRepository servicesRepository)
            : base(rootRuleSet, ruleSetSubsetFactory, servicesRepository)
        {
            SetFilterBounds();
        }

        private void SetFilterBounds()
        {
            supportFilterLowerBound = 1;

            if (rootRuleSet.Rules.Any())
            {
                supportFilterUpperBound = rootRuleSet.Rules.Max(x => x.SupportValue);
                maxSupportFilter = supportFilterUpperBound;
            }
            else
            {
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
                ruleFilterApplier = new SupportValueFilterApplier(minSupportFilter, maxSupportFilter, SelectedRelation, ruleSetSubsetFactory)
                {
                    GenerateChildFilters = this.GenerateChildFilters
                };
            }
            return ruleFilterApplier;
        }

        public override void SetDefaultRelation()
        {
            selectedRelationIndex = availableRelations.GetIndexOf(Relation.GreatherOrEqual);
        }

        public override string ToString()
        {
            return "Support Filter";
        }
    }
}
