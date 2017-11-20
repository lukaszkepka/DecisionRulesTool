using DecisionRulesTool.Model.Utils;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel.Dialog
{
    public class ProgressDialogViewModel : BaseDialogViewModel
    {
        private IProgressNotifier progressNotifier;

        public int Progress
        {
            get
            {
                return 0;
                //return progressNotifier.Progress;
            }
            private set
            {
                RaisePropertyChanged("Progress");
            }
        }

        public ProgressDialogViewModel(IProgressNotifier progressNotifier, ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            this.progressNotifier = progressNotifier;

            progressNotifier.ProgressChanged += (sender, i) => { Progress = i; };
            progressNotifier.Completed += (sender, i) => { OnCloseRequest(); };
        }
    }
}
