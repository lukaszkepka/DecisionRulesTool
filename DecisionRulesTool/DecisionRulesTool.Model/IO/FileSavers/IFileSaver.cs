using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.FileSavers
{
    public interface IFileSaver<T>
    {
        StreamWriter OpenFile(string path);
        void Save(T content, string path);
        void Save(T content, StreamWriter fileStream);
    }
}
