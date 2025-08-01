using System;

namespace MySortLib
{
    public static class SequentialPrefix
    {
        public static float[] CalcSequentialPrefix(int n = 0, float[][]? data = null)
        {
            if(data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if(n <= 0 || n > data.Length / 2)
            {
                n = data.Length / 2;
                Console.WriteLine($"n is set to {n} based on the data length.");
            }

            int length = 2 * n;
            float[] inputs = new float[length];
            float[] globalmax = new float[length];
            float[] currentmax = new float[length];
            int[] u = new int[length];
            int[] v = new int[length];
            int[] q = new int[length];
            float[] answer;
            bool isCurrentMaxNegative = true;

            for (int i = 0; i < length; i++)
            {
                inputs[i] = data[i][1];
                globalmax[i] = float.MinValue;
            }

            for (int i = 0; i < length; i++)
            {
                CalcCurrentMax(inputs[i], currentmax, q, i, ref isCurrentMaxNegative);
                CalcGlobalMax(currentmax, globalmax, u, v, q, i);
            }
            answer = new float[2];
            answer[0] = globalmax[length - 1];
            answer[1] = data[v[length - 1]][0];
            return answer;
        }

        public static void CalcCurrentMax(float input, float[] currentmax, int[] q, int i, ref bool isCurrentMaxNegative)
        {
            if (isCurrentMaxNegative)
            {
                isCurrentMaxNegative = false;
                currentmax[i] = input;
                q[i] = i;
            }
            else
            {
                if (i == 0)
                {
                    Console.WriteLine("Error: i cannot be 0 when isCurrentMaxNegative is false.");
                    return;
                }
                currentmax[i] = currentmax[i - 1] + input;
                q[i] = q[i - 1];
            }
            if (currentmax[i] < 0)
            {
                isCurrentMaxNegative = true;
            }
        }

        public static void CalcGlobalMax(float[] currentmax, float[] globalmax, int[] u, int[] v, int[] q, int i)
        {
            if (i == 0 || currentmax[i] > globalmax[i - 1])
            {
                globalmax[i] = currentmax[i];
                u[i] = q[i];
                v[i] = i;
            }
            else
            {
                globalmax[i] = globalmax[i - 1];
                u[i] = u[i - 1];
                v[i] = v[i - 1];
            }
        }
    }
}