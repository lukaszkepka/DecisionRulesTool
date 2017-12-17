using DecisionRulesTool.UserInterface.Model;
using DecisionRulesTool.UserInterface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class TestRequestGeneratorOptionsViewModel : BaseDialogViewModel
    {
        public TestRequestGeneratorOptionsViewModel(ApplicationRepository applicationCache, ServicesRepository servicesRepository) 
            : base(applicationCache, servicesRepository)
        {
        }
    }
}
