using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DecisionRulesTool.UserInterface.Model
{
    class Command : ICommand
    {
        private Action targetExecuteMethod;
        private Func<bool> targetCanExecuteMethod;
        public event EventHandler CanExecuteChanged;

        public Command(Action executeMethod)
        {
            targetExecuteMethod = executeMethod;
        }

        public Command(Action executeMethod, Func<bool> canExecuteMethod)
        {
            targetExecuteMethod = executeMethod;
            targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (targetCanExecuteMethod != null)
            {
                return targetCanExecuteMethod();
            }

            if (targetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter = null)
        {
            targetExecuteMethod?.Invoke();
        }
    }
}
