using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Parsers
{
    public interface IFileParser<T>
    {
        StreamReader OpenFile(string path);
        T ParseFile(StreamReader fileStream);
        T ParseFile(string path);
        bool IsFileFormatSupported(string path);
        string SupportedFormat { get; }
    }
}
