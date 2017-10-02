using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Exceptions
{
    public class InvalidFileBodyException : Exception
    {
        private string filePath;

        public string FilePath { get; }

        public InvalidFileBodyException(string message, string filePath) : base(message)
        {
            this.filePath = filePath;
        }
    }
}
