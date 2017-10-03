using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.UserInterface.Services.Dialog;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class RuleSubsetGenerationViewModel : BaseWindowViewModel
    {
        private int minSupportFilter;
        private int maxSupportFilter;
        private int minLengthFilter;
        private int maxLengthFilter;

        #region Properties
        public int MinSupportFilter
        {
            get
            {
                return minSupportFilter;
            }
            set
            {
                minSupportFilter = value;
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
                maxSupportFilter = value;
                OnPropertyChanged("MaxSupportFilter");
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
                minLengthFilter = value;
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
                maxLengthFilter = value;
                OnPropertyChanged("MaxLengthFilter");
            }
        }
        #endregion

        public RuleSubsetGenerationViewModel(DialogService dialogService) : base(dialogService)
        {
        }

        protected override void InitializeCommands()
        {
            
        }
    }
}
