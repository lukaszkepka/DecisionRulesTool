using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.RuleTester;
using DecisionRulesTool.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSetPickerViewModel : BaseDialogViewModel
    {
        private ICollection<RuleSetSubset> ruleSets;
        private ICollection<ConflictResolvingMethod> conflictResolvingMethods;
        private int selectedConflictResolvingMethodIndex;
        private DataSet testSet;

        #region Properties
        public ICollection<RuleSetSubset> RuleSets
        {
            get
            {
                return ruleSets;
            }
            set
            {
                ruleSets = value;
                OnPropertyChanged("RuleSets");
            }
        }
        public ICollection<ConflictResolvingMethod> ConflictResolvingMethods
        {
            get
            {
                return conflictResolvingMethods;
            }
            set
            {
                conflictResolvingMethods = value;
                OnPropertyChanged("ConflictResolvingMethods");
            }
        }
        public int SelectedConflictResolvingMethodIndex
        {
            get
            {
                return selectedConflictResolvingMethodIndex;
            }
            set
            {
                selectedConflictResolvingMethodIndex = value;
                OnPropertyChanged("SelectedConflictResolvingMethodIndex");
            }
        }
        #endregion

        public RuleSetPickerViewModel(ICollection<RuleSetSubset> ruleSets, DataSet testSet)
        {
            this.testSet = testSet;

            InitializeConflictResolvingMethods();
            InitializeRuleSets(ruleSets);
        }

        public void InitializeRuleSets(ICollection<RuleSetSubset> ruleSets)
        {
            this.ruleSets = new ObservableCollection<RuleSetSubset>();
            foreach (var ruleSet in ruleSets)
            {
                if (ruleSet.Attributes.SequenceEqual(testSet.Attributes))
                {
                    this.ruleSets.Add(ruleSet);
                }
            }
        }

        public void InitializeConflictResolvingMethods()
        {
            this.conflictResolvingMethods = (ConflictResolvingMethod[])Enum.GetValues(typeof(ConflictResolvingMethod));
        }

        public ICollection<RuleSet> GetSelectedRuleSets()
        {
            return GetSelectedRuleSets(ruleSets);
        }

        private List<RuleSet> GetSelectedRuleSets(ICollection<RuleSetSubset> ruleSets)
        {
            List<RuleSet> result = new List<RuleSet>();
            if (ruleSets.Any())
            {
                foreach (var ruleSet in ruleSets)
                {
                    if (((RuleSetSubsetViewItem)ruleSet).IsSelected)
                    {
                        result.Add(ruleSet);
                    }
                    result.AddRange(GetSelectedRuleSets(ruleSet.Subsets));
                }
            }
            return result;
        }
    }
}