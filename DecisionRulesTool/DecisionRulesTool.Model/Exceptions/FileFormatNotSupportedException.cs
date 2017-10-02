using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Exceptions
{
    public class FileFormatNotSupportedException : Exception
    {
        public string FileExtension { get; }

        public FileFormatNotSupportedException(string filePath)
        {
            FileExtension = filePath;
        }
    }
}
