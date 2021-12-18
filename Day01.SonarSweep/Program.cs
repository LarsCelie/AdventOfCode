using System;
using System.Linq;
using MoreLinq;

namespace Day01.SonarSweep
{
    class Program
    {
        static void Main(string[] args)
        {
            var increaseDepthCount = 0;
            var previousValue = 9999;
            var values = Input.Value.Split('\n').Select(int.Parse).ToArray();
            foreach (var value in values)
            {
                if (previousValue < value)
                {
                    increaseDepthCount++;
                }

                previousValue = value;
            }
            
            Console.WriteLine($"Number of depth increases: {increaseDepthCount}");

            var increaseBatchDepthCount = 0;
            int? previousBatchValue = null;

            int? value1 = null;
            int? value2 = null;
            int? value3 = null;

            foreach (var value in values)
            {
                value1 = value2;
                value2 = value3;
                value3 = value;

                if (!value1.HasValue || !value2.HasValue || !value3.HasValue)
                {
                    continue;
                }

                var batchValue = value1.Value + value2.Value + value3.Value;

                if (previousBatchValue < batchValue)
                {
                    increaseBatchDepthCount++;
                }

                previousBatchValue = batchValue;
            }
            
            Console.WriteLine($"Number of batch depth increases: {increaseBatchDepthCount}");
        }
    }
}