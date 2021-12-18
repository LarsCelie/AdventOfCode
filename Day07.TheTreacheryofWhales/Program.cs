using System;
using System.Linq;

namespace Day7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

            var crabLocations = inputString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .OrderBy(v => v)
                .ToList();

            var median = crabLocations.Count / 2;

            var optimalPosition = crabLocations.ElementAt(median);

            var totalFuel = crabLocations.Select(v => Math.Abs(optimalPosition - v)).Sum();
            
            Console.WriteLine($"1> Total fuel required: {totalFuel}");

            var average = crabLocations.Sum() / crabLocations.Count;
            var totalFuel2 = crabLocations.Select(v => FuelRate(Math.Abs(average - v))).Sum();
            
            Console.WriteLine($"2> Total fuel required: {totalFuel2}");
        }

        private static int FuelRate(int i)
        {
            if (i <= 1)
            {
                return 1;
            }

            return i + FuelRate(i - 1);
        }
    }
}