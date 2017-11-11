using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel.MainViewModels
{
    /// <summary>
    /// Base class for every view model in this application
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        protected ApplicationCache applicationCache;
        protected ServicesRepository servicesRepository;

        public ApplicationViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository)
        {
            this.servicesRepository = servicesRepository;
            this.applicationCache = applicationCache;
        }
    }
}
