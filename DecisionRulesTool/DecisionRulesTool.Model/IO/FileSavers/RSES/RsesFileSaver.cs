using DecisionRulesTool.Model.FileSavers;
using DecisionRulesTool.Model.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.FileLoaders.RSES
{
    public abstract class RsesFileSaver<T> : BaseFileSaver<T>
    {
        protected RsesFileFormat fileFormat;
        public RsesFileSaver()
        {
            fileFormat = new RsesFileFormat();
        }
    }
}
