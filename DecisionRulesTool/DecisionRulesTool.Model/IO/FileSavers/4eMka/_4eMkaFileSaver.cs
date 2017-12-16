using DecisionRulesTool.Model.FileSavers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecisionRulesTool.Model.IO.FileSavers._4eMka
{
    public abstract class _4eMkaFileSaver<T> : BaseFileSaver<T>
    {
        protected _4eMkaFileFormat fileFormat;
        public _4eMkaFileSaver()
        {
            fileFormat = new _4eMkaFileFormat();
        }
    }
}
