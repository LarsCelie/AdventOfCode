using System;
using System.Linq;

namespace Day6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

            var initialDivision = inputString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .GroupBy(value => value)
                .ToDictionary(value => value.Key, value => value.LongCount());

            initialDivision[0] = 0;
            initialDivision[6] = 0;
            initialDivision[7] = 0;
            initialDivision[8] = 0;

            var days = 256;
            var fishTimers = 8;

            for (int i = 1; i <= days; i++)
            {
                var duplicateFish = initialDivision[0];
                for (int j = 0; j < fishTimers; j++)
                {
                    initialDivision[j] = initialDivision[j + 1];
                }
                
                initialDivision[6] += duplicateFish;
                initialDivision[8] = duplicateFish;
                
                var dayTotal = initialDivision.Values.Sum();
                Console.WriteLine($"Fish after {i} days: {dayTotal}");
            }

            var total = initialDivision.Values.Sum();
            Console.WriteLine(total);
        }
    }
}