using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.Model.Model.Factory;
using Unity;
using DecisionRulesTool.UserInterface.Services;

namespace DecisionRulesTool.UserInterface.ViewModel.Filters
{
    public abstract class RelationFilterViewModel : FilterViewModel
    {
        protected Relation[] availableRelations;
        protected int selectedRelationIndex;

        #region Properties
        public bool GenerateChildFilters { get; set; }
        public Relation[] AvailableRelations
        {
            get
            {
                return availableRelations;
            }
            set
            {
                availableRelations = value;
                RaisePropertyChanged("AvailableRelations");
            }
        }
        public Relation SelectedRelation
        {
            get
            {
                return availableRelations[selectedRelationIndex];
            }
        }
        public int SelectedRelationIndex
        {
            get
            {
                return selectedRelationIndex;
            }
            set
            {

                selectedRelationIndex = value;
                RaisePropertyChanged("SelectedRelationIndex");
            }
        }
        #endregion Properties

        public RelationFilterViewModel(RuleSetSubset rootRuleSet, IRuleSetSubsetFactory ruleSetSubsetFactory, ServicesRepository servicesRepository)
            : base(rootRuleSet, ruleSetSubsetFactory, servicesRepository)
        {
            InitializeRelations();
        }

        public virtual void SetDefaultRelation()
        {
            selectedRelationIndex = availableRelations.GetIndexOf(Relation.Equality);
        }

        public void InitializeRelations()
        {
            availableRelations = Enum.GetValues(typeof(Relation)).Cast<Relation>().Where(x => x != Relation.Undefined).ToArray();
            SetDefaultRelation();
        }

    }
}
