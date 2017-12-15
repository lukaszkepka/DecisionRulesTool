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

        public static int GetIndexOf<T>(this T[] array, T element)
        {
            int index = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if(array[i].Equals(element))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, T element)
        {
            int index = -1;

            int i = 0;
            foreach (var item in collection)
            {
                if (item.Equals(element))
                {
                    index = i++;
                    break;
                }
            }

            return index;
        }

        internal static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action.Invoke(item);
            }
        }
    }
}
