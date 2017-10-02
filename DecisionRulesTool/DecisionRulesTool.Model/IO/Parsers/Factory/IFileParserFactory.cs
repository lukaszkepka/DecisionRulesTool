using DecisionRulesTool.Model.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO.Parsers.Factory
{
    public interface IFileParserFactory<T>
    {
        IFileParser<T> Create(string fileExtension);
    }
}
