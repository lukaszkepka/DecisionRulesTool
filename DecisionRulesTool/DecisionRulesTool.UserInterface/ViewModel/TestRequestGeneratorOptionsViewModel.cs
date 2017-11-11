using DecisionRulesTool.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class TestRequestGeneratorOptionsViewModel : BaseDialogViewModel
    {
        public TestRequestGeneratorOptionsViewModel(Services.ServicesRepository servicesRepository) 
            : base(servicesRepository)
        {
        }
    }
}
