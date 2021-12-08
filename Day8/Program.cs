using System;
using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputString = args.Length > 0 ? Input.TestValue : Input.Value;
            
            var output = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(splitString => splitString.Split(" | ", StringSplitOptions.RemoveEmptyEntries)[1].Split().ToArray())
                .Count(s => s.Length <= 4 || s.Length >= 7);

            Console.WriteLine($"1> Count unique digits: {output}");
            
            var outputSum = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line =>
                {
                    var keyAndValue = line.Split(" | ", StringSplitOptions.RemoveEmptyEntries);
                    var keys = keyAndValue[0];

                    var uniqueKeys = keys.Split().Select(k => string.Join(string.Empty, k.OrderBy(c => c))).ToArray();
                    
                    var one = uniqueKeys.Single(k => k.Length == 2);
                    var seven = uniqueKeys.Single(k => k.Length == 3);
                    var four = uniqueKeys.Single(k => k.Length == 4);
                    var eight = uniqueKeys.Single(k => k.Length == 7);
                    var nine = uniqueKeys.Single(k => k.Length == 6 && four.All(k.Contains));
                    var three = uniqueKeys.Single(k => k.Length == 5 && one.All(k.Contains));
                    
                    var fourDifferenceToOne = four.Except(one);
                    var five = uniqueKeys.Single(k => k.Length == 5 && fourDifferenceToOne.All(k.Contains));
                    var two = uniqueKeys.Single(k => k.Length == 5 && k != five && k != three);
                    var six = uniqueKeys.Single(k => k.Length == 6 && !one.All(k.Contains));
                    var zero = uniqueKeys.Single(k => k.Length == 6 && k != six && k != nine);

                    var dictionary = new Dictionary<string, int>()
                    {
                        { zero, 0  },
                        { one, 1 },
                        { two, 2 },
                        { three, 3 },
                        { four, 4 },
                        { five, 5 },
                        { six, 6 },
                        { seven, 7 },
                        { eight, 8 },
                        { nine, 9 }
                    };
                    
                    var valuesToDecipher = keyAndValue[1].Split();

                    var outputString = string.Join(string.Empty, valuesToDecipher.Select(value => dictionary[string.Join(string.Empty, value.OrderBy(c => c))]));

                    return int.Parse(outputString);
                }).Sum();
            
            Console.WriteLine($"2> Count sum of all digits: {outputSum}");
        }
    }
}

