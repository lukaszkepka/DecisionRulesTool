using DecisionRulesTool.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Utils;

namespace DecisionRulesTool.UserInterface.ViewModel.Filters
{
    public class LengthFilterViewModel : RelationFilterViewModel
    {
        private int lengthFilterLowerBound = 1;
        private int lengthFilterUpperBound = 1;
        private int minLengthFilter = 1;
        private int maxLengthFilter = 1;

        #region Properties
        public int MinLengthFilter
        {
            get
            {
                return minLengthFilter;
            }
            set
            {
                if (value > MaxLengthFilter)
                {
                    //TODO
                }
                else if (value >= lengthFilterLowerBound)
                {
                    minLengthFilter = value;
                }
                OnPropertyChanged("MinLengthFilter");
            }
        }
        public int MaxLengthFilter
        {
            get
            {
                return maxLengthFilter;
            }
            set
            {
                if (value > lengthFilterUpperBound)
                {
                    maxLengthFilter = lengthFilterUpperBound;
                }
                else if (value < MinLengthFilter)
                {
                    //TODO
                }
                else
                {
                    maxLengthFilter = value;
                }
                OnPropertyChanged("MaxLengthFilter");
            }
        }
        #endregion Properties


        public LengthFilterViewModel(RuleSetSubset rootRuleSet) : base(rootRuleSet)
        {
            SetFilterBounds();
        }

        public override IRuleFilterApplier GetRuleSeriesFilter()
        {
            IRuleFilterApplier ruleFilterApplier = default(IRuleFilterApplier);
            if(isEnabled)
            {
                ruleFilterApplier = new LengthFilterApplier(minLengthFilter, maxLengthFilter, SelectedRelation);
            }
            return ruleFilterApplier;
        }

        private void SetFilterBounds()
        {
            lengthFilterLowerBound = 1;

            if (rootRuleSet.Rules.Any())
            {
                //Length filter
                lengthFilterUpperBound = rootRuleSet.Rules.Max(x => x.Conditions.Count);
                maxLengthFilter = lengthFilterUpperBound;
            }
            else
            {
                //Length filter
                lengthFilterUpperBound = 0;
                lengthFilterLowerBound = 0;
                minLengthFilter = 0;
                maxLengthFilter = 0;
            }
        }

        public override void SetDefaultRelation()
        {
            selectedRelationIndex = availableRelations.GetIndexOf(Relation.LessOrEqual);
        }

        public override string ToString()
        {
            return "Length Filter";
        }
    }
}
