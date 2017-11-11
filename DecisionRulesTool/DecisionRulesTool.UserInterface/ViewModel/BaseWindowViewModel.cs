using DecisionRulesTool.Model.IO.Parsers.Factory;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.Services.Dialog;
using GalaSoft.MvvmLight;
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
    public abstract class BaseWindowViewModel : ViewModelBase
    {
        protected ServicesRepository servicesRepository;

        public event EventHandler CloseRequest;

        public BaseWindowViewModel(ServicesRepository servicesRepository)
        {
            this.servicesRepository = servicesRepository;
        }

        protected void OnCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
