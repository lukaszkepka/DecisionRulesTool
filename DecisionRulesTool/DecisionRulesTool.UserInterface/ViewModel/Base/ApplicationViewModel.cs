﻿using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using DecisionRulesTool.UserInterface.ViewModel.Dialog;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel.MainViewModels
{
    /// <summary>
    /// Base class for every view model in this application
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        protected ApplicationRepository applicationRepository;
        protected ServicesRepository servicesRepository;

        public ICommand ShowOptions { get; private set; }

        public ApplicationViewModel(ApplicationRepository applicationCache, ServicesRepository servicesRepository)
        {
            this.servicesRepository = servicesRepository;
            this.applicationRepository = applicationCache;
            ShowOptions = new RelayCommand(OnShowOptions);
        }

        private void OnShowOptions()
        {
            servicesRepository.DialogService.ShowDialog(new ApplicationOptionsViewModel(applicationRepository, servicesRepository));
        }
    }
}
