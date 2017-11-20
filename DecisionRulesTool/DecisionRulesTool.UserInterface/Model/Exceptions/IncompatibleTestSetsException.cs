using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.UserInterface.Model.Exceptions
{
    public class IncompatibleTestSetsException : Exception
    {
        public IncompatibleTestSetsException(string message) : base(message)
        {

        }
    }
}
