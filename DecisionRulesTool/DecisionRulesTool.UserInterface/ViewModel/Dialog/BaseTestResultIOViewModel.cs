using DecisionRulesTool.Model.Parsers;
using DecisionRulesTool.Model.RuleTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel.Dialog
{
    public abstract class BaseTestResultIOViewModel : BaseDialogViewModel
    {
        private bool isInProgress = false;
        protected bool cancelRequest = false;


        public abstract string Title { get; }
        public abstract string Action { get; }
        public bool IsNotInProgress => !IsInProgress;
        public bool IsInProgress
        {
            get
            {
                return isInProgress;
            }
            set
            {
                isInProgress = value;
                RaisePropertyChanged("IsInProgress");
                RaisePropertyChanged("IsNotInProgress");
            }
        }
        public int ActualIteration { get; protected set; }
        public int MaxIteration { get; protected set; }
        public int Progress { get; protected set; }

        public BaseTestResultIOViewModel(Model.ApplicationCache applicationCache, Services.ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
        }

        public override void OnCancel()
        {
            cancelRequest = true;
        }

        protected void UpdateProgress(int actualIteration, int maxIterations)
        {
            Progress = (int)(((actualIteration + 1) / (double)maxIterations) * 100);
            ActualIteration = actualIteration + 1;
        }
    }
}

