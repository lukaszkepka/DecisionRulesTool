using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.RuleFilters;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.UserInterface.Model;
using System.Collections.ObjectModel;

namespace DecisionRulesTool.UserInterface.ViewModel.Filters
{
    public class AttributePresenceFilterViewModel : FilterViewModel
    {
        private ICollection<SelectableItem<string>> attributes;
        protected AttributePresenceFilter.Mode[] availableModes;
        protected int selectedModeIndex;

        #region Properties
        public AttributePresenceFilter.Mode[] AvailableModes
        {
            get
            {
                return availableModes;
            }
        }
        public int SelectedModeIndex
        {
            get
            {
                return selectedModeIndex;
            }
            set
            {
                selectedModeIndex = value;
                OnPropertyChanged("SelectedModeIndex");
            }
        }
        public ICollection<SelectableItem<string>> Attributes
        {
            get
            {
                return attributes;
            }
        }
        #endregion Properties

        public AttributePresenceFilterViewModel(RuleSetSubset rootRuleSet) : base(rootRuleSet)
        {
            InitializeAvailableModes();


            attributes = new ObservableCollection<SelectableItem<string>>(rootRuleSet.Attributes.Select(x => new SelectableItem<string>(x.Name)));
        }

        public void InitializeAvailableModes()
        {
            availableModes = (AttributePresenceFilter.Mode[])Enum.GetValues(typeof(AttributePresenceFilter.Mode));
            selectedModeIndex = 0;
        }

        public override IRuleFilterApplier GetRuleSeriesFilter()
        {
            return new AttributePresenceFilterApplier(availableModes[selectedModeIndex], Attributes.GetSelectedItems().ToArray());
        }
    }
}
