// See https://aka.ms/new-console-template for more information

namespace Day9;

class Program
{
    public static void Main(string[] args)
    {
        var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

        var input = inputString.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();

        var lowPoints = new List<(int x, int y)>();

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                var value = GetPosition(input, j, i);

                var compareValues = new[]
                {
                    GetPosition(input, j - 1, i),
                    GetPosition(input, j + 1, i),
                    GetPosition(input, j, i + 1),
                    GetPosition(input,  j, i - 1)
                }.Where(v => v.HasValue);

                if (compareValues.All(cv => cv > value))
                {
                    lowPoints.Add((j, i));
                }

            }
        }
        
        Console.WriteLine($"1> Risk level of low points: {lowPoints.Select(p => input[p.y][p.x]).Sum() + lowPoints.Count}");
        
        var basinSizes = new List<int>();
        foreach (var (x, y) in lowPoints)
        {
            var positions = GetLargerSurroundingPositions(input, x, y).Distinct().Count();
            basinSizes.Add(positions);
        }
        
        Console.WriteLine($"2> Sizes multiplied of the three largest basins: {basinSizes.OrderByDescending(b => b).Take(3).Aggregate(1, (a, b) => a * b)}");
    }

    private static IEnumerable<(int x, int y)> GetLargerSurroundingPositions(int[][] input, int x, int y)
    {
        var currentValue = GetPosition(input, x, y);

        var surroundingValues = new[]
            {
                new { x, y = y - 1, v = GetPosition(input, x, y - 1) },
                new { x, y = y + 1, v = GetPosition(input, x, y + 1) },
                new { x = x + 1, y, v = GetPosition(input, x + 1, y) },
                new { x = x - 1, y, v = GetPosition(input, x - 1, y) }
            }.Where(p => p.v.HasValue && p.v.Value > currentValue && p.v.Value != 9)
            .ToArray();

        yield return (x, y);
        foreach (var (i, i1) in surroundingValues.SelectMany(s => GetLargerSurroundingPositions(input, s.x, s.y)))
        {
            yield return (i, i1);
        }
    }

    private static int? GetPosition(int[][] map, int x, int y)
    {
        try
        {
            return map[y][x];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }
}