using System;
using System.Threading.Tasks;

namespace MySortLib
{
    public static class OddEvenMergeSort
    {
        private static int processors = 1;

        public static async Task<float[][]> NSort(float[][] unsortedMatrix, int maxCPU = 1)
        {
            int matrixSize = unsortedMatrix.Length;
            if (matrixSize == 1)
            {
                return unsortedMatrix;
            }

            processors = maxCPU;

            var options = new ParallelOptions();
            if (processors > 0) options.MaxDegreeOfParallelism = processors;

            float[][] leftMatrix = new float[matrixSize / 2][];
            float[][] rightMatrix = new float[matrixSize / 2][];

            Parallel.For(0, matrixSize / 2, options, i =>
            {
                leftMatrix[i] = unsortedMatrix[i];
                rightMatrix[i] = unsortedMatrix[matrixSize / 2 + i];
            });

            var leftTask = NSort(leftMatrix);
            var rightTask = NSort(rightMatrix);
            await Task.WhenAll(leftTask, rightTask);

            leftMatrix = await leftTask;
            rightMatrix = await rightTask;

            Parallel.For(0, matrixSize / 2, options, i =>
            {
                unsortedMatrix[i] = leftMatrix[i];
                unsortedMatrix[matrixSize / 2 + i] = rightMatrix[i];
            });

            return await OEMergeSort(unsortedMatrix);
        }

        private static async Task<float[][]> OEMergeSort(float[][] unsortedMatrix)
        {
            int matrixSize = unsortedMatrix.Length;
            if (matrixSize == 1)
            {
                return unsortedMatrix;
            }

            float[][] leftMatrix = new float[matrixSize / 2][];
            float[][] rightMatrix = new float[matrixSize / 2][];

            var options = new ParallelOptions();
            if (processors > 0) options.MaxDegreeOfParallelism = processors;

            Parallel.For(0, matrixSize / 2, options, i =>
            {
                leftMatrix[i] = unsortedMatrix[2 * i];
                rightMatrix[i] = unsortedMatrix[2 * i + 1];
            });

            var leftTask = OEMergeSort(leftMatrix);
            var rightTask = OEMergeSort(rightMatrix);

            await Task.WhenAll(leftTask, rightTask);

            leftMatrix = await leftTask;
            rightMatrix = await rightTask;

            return await ComparatorStep(leftMatrix, rightMatrix);
        }

        private static async Task<float[][]> ComparatorStep(float[][] leftMatrix, float[][] rightMatrix)
        {
            int halfMatrixSize = leftMatrix.Length;
            float[][] sortedMatrix = new float[halfMatrixSize * 2][];

            if (halfMatrixSize == 1)
            {
                if (leftMatrix[0][0] < rightMatrix[0][0])
                {
                    sortedMatrix[0] = leftMatrix[0];
                    sortedMatrix[1] = rightMatrix[0];
                }
                else if (leftMatrix[0][0] > rightMatrix[0][0])
                {
                    sortedMatrix[0] = rightMatrix[0];
                    sortedMatrix[1] = leftMatrix[0];
                }
                else
                {
                    if (leftMatrix[0][1] > rightMatrix[0][1])
                    {
                        sortedMatrix[0] = leftMatrix[0];
                        sortedMatrix[1] = rightMatrix[0];
                    }
                    else
                    {
                        sortedMatrix[0] = rightMatrix[0];
                        sortedMatrix[1] = leftMatrix[0];
                    }
                }
                return sortedMatrix;
            }

            sortedMatrix[0] = leftMatrix[0];
            sortedMatrix[halfMatrixSize * 2 - 1] = rightMatrix[halfMatrixSize - 1];

            var options = new ParallelOptions();
            if (processors > 0) options.MaxDegreeOfParallelism = processors;

            await Task.Run(() =>
            {
                Parallel.For(0, halfMatrixSize - 1, options, i =>
                {
                    if (leftMatrix[i + 1][0] < rightMatrix[i][0])
                    {
                        sortedMatrix[i * 2 + 1] = leftMatrix[i + 1];
                        sortedMatrix[i * 2 + 2] = rightMatrix[i];
                    }
                    else if (leftMatrix[i + 1][0] > rightMatrix[i][0])
                    {
                        sortedMatrix[i * 2 + 1] = rightMatrix[i];
                        sortedMatrix[i * 2 + 2] = leftMatrix[i + 1];
                    }
                    else
                    {
                        if (leftMatrix[i + 1][1] > rightMatrix[i][1])
                        {
                            sortedMatrix[i * 2 + 1] = leftMatrix[i + 1];
                            sortedMatrix[i * 2 + 2] = rightMatrix[i];
                        }
                        else
                        {
                            sortedMatrix[i * 2 + 1] = rightMatrix[i];
                            sortedMatrix[i * 2 + 2] = leftMatrix[i + 1];
                        }
                    }
                });
            });

            return sortedMatrix;
        }
    }
}