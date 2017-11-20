using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class BaseDialogViewModel : BaseWindowViewModel
    {
        public ICommand Apply { get; protected set; }
        public ICommand Cancel { get; protected set; }

        public bool Result { get; protected set; }

        public BaseDialogViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            Apply = new RelayCommand(OnApply);
            Cancel = new RelayCommand(OnCancel);
        }

        public virtual void OnApply()
        {
            Result = true;
            OnCloseRequest();
        }

        public virtual void OnCancel()
        {
            Result = false;
            OnCloseRequest();
        }
    }
}
