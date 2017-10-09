using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.ViewModel
{
    public class BaseDialogViewModel : BaseWindowViewModel
    {
        public bool Result { get; protected set; }

        protected override void InitializeCommands()
        {
            //throw new NotImplementedException();
        }
    }
}
