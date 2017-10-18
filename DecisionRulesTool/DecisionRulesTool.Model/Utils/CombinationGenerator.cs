using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionRulesTool.Model.Utils
{
    public class CombinationGenerator
    {
        private T[] GenerateSubset<T>(T[] elements, bool[] mask)
        {
            IList<T> subset = new List<T>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (mask[i] == true)
                {
                    subset.Add(elements[i]);
                }
            }
            return subset.ToArray();
        }
        public IEnumerable<T[]> GenerateAllSubsets<T>(IEnumerable<T> elements)
        {
            IList<T[]> allSubsets = new List<T[]>();
            T[] elementsArray = elements.ToArray();

            bool[][] binaryMasks = GenerateBinarySequences(elements.Count());

            for (int i = 0; i < binaryMasks.Length; i++)
            {
                allSubsets.Add(GenerateSubset(elementsArray, binaryMasks[i]));
            }

            return allSubsets;
        }

        private bool[][] GenerateBinarySequences(int numberOfBits)
        {
            int length = Convert.ToInt32(Math.Pow(2, numberOfBits));
            bool[][] binarySequences = new bool[length][];

            for (int i = 0; i < length; i++)
            {
                binarySequences[i] = IntToBinary(i, numberOfBits);
            }
            return binarySequences;
        }

        private bool[] IntToBinary(int number, int numberOfBits)
        {
            IList<bool> binaryRepresentation = new List<bool>();

            for (int i = 1; i <= numberOfBits; i++)
            {
                if (number % 2 == 0)
                {
                    binaryRepresentation.Add(false);
                }
                else
                {
                    binaryRepresentation.Add(true);
                }

                number = number >> 1;
            }
            return binaryRepresentation.ToArray();
        }
    }
}
