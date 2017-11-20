using DecisionRulesTool.Model.FileSavers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.IO.FileSavers.Factory
{
    public interface IFileSaverFactory<T>
    {
        IFileSaver<T> Create(string fileExtension);
    }
}
