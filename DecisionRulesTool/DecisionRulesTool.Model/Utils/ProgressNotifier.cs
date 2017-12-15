using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Utils
{
    public class ProgressNotifier : IProgressNotifier
    {
        public const int MinValue = 0;
        public const int MaxValue = 100;

        public void OnProgressChanged(int interval)
        {
            ProgressChanged?.Invoke(this, interval);
        }

        public void OnCompleted()
        {
            OnProgressChanged(MaxValue);
            Completed?.Invoke(this, EventArgs.Empty);
        }

        public void OnStart()
        {
            OnProgressChanged(MinValue);
        }

        public event EventHandler<int> ProgressChanged;
        public event EventHandler Completed;
    }
}
