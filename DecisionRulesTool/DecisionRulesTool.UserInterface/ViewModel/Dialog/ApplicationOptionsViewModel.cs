﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using PropertyChanged;

namespace DecisionRulesTool.UserInterface.ViewModel.Dialog
{
    [AddINotifyPropertyChangedInterface]
    public class ApplicationOptionsViewModel : BaseDialogViewModel
    {
        public bool DumpResults { get; set; }
        public ApplicationOptionsViewModel(ApplicationCache applicationCache, ServicesRepository servicesRepository) : base(applicationCache, servicesRepository)
        {
        }

        public override void OnApply()
        {
            servicesRepository.TestManagerViewModel.DumpResults = this.DumpResults;
            base.OnApply();          
        }

    }
}
