using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Utils
{
    public static class ArrayExtensions
    {
        public static int GetIndexOfMax(this int[] array)
        {
            int maxIndex = -1;
            if(array != null)
            {
                int max = array[0];
                maxIndex = 0;
                for (int i = 1; i < array.Length; i++)
                {
                    if(array[i] > max)
                    {
                        max = array[i];
                        maxIndex = i;
                    }
                }
            }
            return maxIndex;
        }
    }
}
