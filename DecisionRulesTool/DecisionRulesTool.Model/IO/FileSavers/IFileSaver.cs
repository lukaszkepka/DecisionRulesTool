using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.FileSavers
{
    interface IFileSaver<T>
    {
        FileStream OpenFile(string path);
        void Save(T content, string path);
        void Save(T content, FileStream fileStream);
    }
}
