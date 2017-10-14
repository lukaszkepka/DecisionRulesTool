using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.UserInterface.Services.Dialog;
using System.Windows.Input;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.Model.RuleFilters;
using DecisionRulesTool.Model.RuleFilters.RuleSeriesFilters;
using DecisionRulesTool.Model.Model;
using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.ViewModel.Filters;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSubsetGenerationViewModel : BaseDialogViewModel
    {
        private IRuleSubsetGenerator ruleSubsetGenerator;
        private RuleSetSubset rootRuleSet;

        private LengthFilterViewModel lengthFilterViewModel;
        private SupportValueFilterViewModel supportValueFilterViewModel;
        private AttributePresenceFilterViewModel attributePresenceFilterViewModel;

        public ICommand Apply { get; private set; }
        public ICommand Cancel { get; private set; }

        #region Properties
        public LengthFilterViewModel LengthFilterViewModel
        {
            get
            {
                return lengthFilterViewModel;
            }
            set
            {
                lengthFilterViewModel = value;
                OnPropertyChanged("LengthFilterViewModel");
            }
        }
        public SupportValueFilterViewModel SupportValueFilterViewModel
        {
            get
            {
                return supportValueFilterViewModel;
            }
            set
            {
                supportValueFilterViewModel = value;
                OnPropertyChanged("SupportValueFilterViewModel");
            }
        }
        public AttributePresenceFilterViewModel AttributePresenceFilterViewModel
        {
            get
            {
                return attributePresenceFilterViewModel;
            }
            set
            {
                attributePresenceFilterViewModel = value;
                OnPropertyChanged("AttributePresenceFilterViewModel");
            }
        }
        #endregion

        public RuleSubsetGenerationViewModel(RuleSetSubset rootRuleSet)
        {
            this.ruleSubsetGenerator = new RuleSetSubsetGeneratorOLD(rootRuleSet);
            this.supportValueFilterViewModel = new SupportValueFilterViewModel(rootRuleSet);
            this.lengthFilterViewModel = new LengthFilterViewModel(rootRuleSet);
            this.attributePresenceFilterViewModel = new AttributePresenceFilterViewModel(rootRuleSet);
            this.rootRuleSet = rootRuleSet;

            InitializeCommands();
        }

        public IRuleSubsetGenerator GetSubsetGenerator()
        {
            IRuleSubsetGenerator ruleSubsetGenerator = new RuleSetSubsetGenerator(rootRuleSet);
            IRuleFilterApplier filter1 = lengthFilterViewModel.GetRuleSeriesFilter();
            IRuleFilterApplier filter2 = supportValueFilterViewModel.GetRuleSeriesFilter();
            IRuleFilterApplier filter3 = attributePresenceFilterViewModel.GetRuleSeriesFilter();

            ruleSubsetGenerator.AddFilter(filter1);
            ruleSubsetGenerator.AddFilter(filter2);
            ruleSubsetGenerator.AddFilter(filter3);
            return ruleSubsetGenerator;
        }

        private void InitializeCommands()
        {
            Apply = new RelayCommand(OnApply);
            Cancel = new RelayCommand(OnCancel);
        }

        public void OnApply()
        {
            Result = true;
            OnCloseRequest();
        }

        public void OnCancel()
        {
            Result = false;
            OnCloseRequest();
        }
    }
}
