using System.Diagnostics;
using Day14;
using MoreLinq;

var inputString = args.Length > 0 ? Input.TestInputPairString : Input.InputPairString;
var polymerString = args.Length > 0 ? Input.TestPolymerString : Input.PolymerString;

var sw = Stopwatch.StartNew();
sw.Start();

var pairMapping = inputString.Split(new[] { '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(" -> "))
    .ToDictionary(value => value[0], value => new[] { value[0][0] + value[1].ToString(), value[1].ToString() + value[0][1]});
    
var currentPairs = polymerString.Pairwise((a, b) => a.ToString() + b).ToArray().GroupBy(v => v).Select(g => new { g.Key, Count = g.LongCount()});

const int steps = 40;

for (var i = 1; i <= steps; i++)
{ 
    currentPairs = currentPairs.SelectMany(pair => pairMapping[pair.Key].Select(key => new { Key = key, pair.Count }))
        .GroupBy(v => v.Key)
        .Select(g => new { g.Key, Count = g.Sum(z => z.Count)});
}

var measurements = currentPairs
    .SelectMany(v => new[] { new{ Key = v.Key[0], v.Count}, new{ Key = v.Key[1], v.Count}})
    .GroupBy(v => v.Key)
    .Select(v => new { v.Key, Count = v.Sum(x => x.Count) })
    .OrderBy(v => v.Count)
    .ToArray();

Console.WriteLine($"Difference between highest and lowest value: {(measurements.Last().Count - measurements.First().Count) / 2} in {sw.ElapsedMilliseconds} milliseconds.");