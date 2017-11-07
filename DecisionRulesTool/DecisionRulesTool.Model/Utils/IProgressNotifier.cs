using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Utils
{
    public interface IProgressNotifier
    {
        void OnProgressChanged(int interval);
        void OnCompleted();
        void OnStart();

        event EventHandler<int> ProgressChanged;
        event EventHandler Completed;
    }
}
