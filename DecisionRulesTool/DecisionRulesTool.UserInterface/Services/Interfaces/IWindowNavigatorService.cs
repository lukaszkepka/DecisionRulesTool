using DecisionRulesTool.UserInterface.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Services.Interfaces
{
    public interface IWindowNavigatorService
    {
        void SwitchContext(BaseWindowViewModel viewModel);
    }
}
