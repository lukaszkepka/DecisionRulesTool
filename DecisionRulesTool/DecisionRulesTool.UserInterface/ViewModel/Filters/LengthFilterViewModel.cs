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
    public class LengthFilterViewModel : FilterViewModel
    {
        private string[] availableRelations;
        private string selectedRelation;

        private int lengthFilterLowerBound = 1;
        private int lengthFilterUpperBound = 1;
        private int minLengthFilter = 1;
        private int maxLengthFilter = 1;

        #region Properties
        public string[] AvailableRelations
        {
            get
            {
                return availableRelations;
            }
            set
            {
                availableRelations = value;
                OnPropertyChanged("AvailableRelations");
            }
        }
        public string SelectedRelation
        {
            get
            {
                return selectedRelation;
            }
            set
            {

                selectedRelation = value;
                OnPropertyChanged("SelectedRelation");
            }
        }
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
            InitializeRelations();
            SetFilterBounds();
        }

        public void InitializeRelations()
        {
            var relations = (Relation[])Enum.GetValues(typeof(Relation));
            availableRelations = new string[relations.Length - 1];
            for (int i = 0; i < relations.Length - 1; i++)
            {
                availableRelations[i] = Tools.GetRelationString(relations[i]);
            }

            selectedRelation = availableRelations.Where(x => x.Equals(Tools.GetRelationString(Relation.Equality))).FirstOrDefault();
        }

        public override IRuleFilterApplier[] GetRuleSeriesFilter()
        {
            return new[] { new LengthSeriesFilter(minLengthFilter, maxLengthFilter, Tools.ParseRelationString(selectedRelation)) };
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
    }
}
