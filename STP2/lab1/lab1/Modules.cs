using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Modules
    {
        public static double GetEvenProduct(float[] sequence)
        {
            double product = 1;
            for (int i = 0;
                i < sequence.Length;
                i += 2)
            {
                product *= sequence[i];
            }
            return product;
        }

        public static float[] ShiftSequence(float[] sequence, int shift)
        {
            int fixedShift = Math.Abs(shift % sequence.Length);

            float[] newSequence = new float[sequence.Length];

            if (shift >= 0)
            {
                Array.Copy(sequence, 0, newSequence, fixedShift, sequence.Length - fixedShift);
                Array.Copy(sequence, sequence.Length - fixedShift, newSequence, 0, fixedShift);
            }
            else
            {
                Array.Copy(sequence, fixedShift, newSequence, 0, sequence.Length - fixedShift);
                Array.Copy(sequence, 0, newSequence, sequence.Length - fixedShift, fixedShift);
            }

            return newSequence;
        }

        public static (long, int) GetMaxEvenAndIndex(int[] sequence)
        {
            long maxNumber = long.MinValue;
            int maxNumberIndex = -1;
            for (int i = 0;
                i < sequence.Length;
                i += 2)
            {
                if (maxNumber < sequence[i])
                {
                    maxNumber = sequence[i];
                    maxNumberIndex = i;
                }
            }
            return (maxNumber, maxNumberIndex);
        }
    }
}
