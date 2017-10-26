using DecisionRulesTool.UserInterface.Model;
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
        public ICommand Apply { get; private set; }

        public TestRequestGeneratorOptionsViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            Apply = new RelayCommand(OnApply);
        }

        public void OnApply()
        {
            Result = true;
            OnCloseRequest();
        }
    }
}
