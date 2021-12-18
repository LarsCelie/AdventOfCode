using System;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    public class Program
    {
        public static int Result;
        
        static void Main(string[] args)
        {
            var inputString = args.Length > 0 ? Input.TestValue : Input.Value;
            var lines = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            var ranges = ParseFissures(lines);
            var seaFloor = new SeaFloor(ranges);

            Result = seaFloor.CountDuplicates();
            
            // seaFloor.PrintMap();
            
            Console.WriteLine();
            Console.WriteLine($"The number of duplicates is: {Result}");
        }

        private static IEnumerable<FissureRange> ParseFissures(string[] lines)
        {
            foreach (var line in lines)
            {
                var xAndY = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
                var xString = xAndY.First().Split(',', StringSplitOptions.RemoveEmptyEntries);
                var yString = xAndY.Last().Split(',', StringSplitOptions.RemoveEmptyEntries);

                yield return new FissureRange(
                    new FissureCoordinate(int.Parse(xString.First()), int.Parse(xString.Last())),
                    new FissureCoordinate(int.Parse(yString.First()), int.Parse(yString.Last())));
            }
        }
    }
}