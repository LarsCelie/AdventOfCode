// See https://aka.ms/new-console-template for more information
namespace Day11;

class Program
{
    
    public static void Main(string[] args)
    {
        var inputString = args.Length > 0 ? Input.TestValue : Input.Value;
        var octopusInGrid = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Select(c => new Octopus(int.Parse(c.ToString()))).ToArray())
            .ToArray();
        
        for (var i = 0; i < octopusInGrid.Length; i++)
        {
            for (int j = 0; j < octopusInGrid[i].Length; j++)
            {
                var currentOctopus = GetOctopus(octopusInGrid, j, i);
                var surroundingOctopus = GetSurroundingOctopus(octopusInGrid, j, i);
                currentOctopus.SurroundingOctopus.AddRange(surroundingOctopus);
            }
        }

        var allOctos = octopusInGrid.SelectMany(octo => octo)
            .ToArray();

        // part one:
        // var steps = 100;
        //
        // for (int i = 0; i < steps; i++)
        // {
        //     foreach (var octo in allOctos)
        //     {
        //         octo.Flash();
        //     }
        //
        //     foreach (var octo in allOctos)
        //     {
        //         octo.FinishStep();
        //     }
        // }

        // var totalFlashCount = allOctos.Sum(octo => octo.FlashCount);
        // Console.WriteLine(totalFlashCount);

        var stepCount = 0;
        var allFlashed = false;
        while (!allFlashed)
        {
            stepCount++;
            foreach (var octo in allOctos)
            {
                octo.Flash();
            }

            allFlashed = allOctos.All(octo => octo.Flashed);
        
            foreach (var octo in allOctos)
            {
                octo.FinishStep();
            }
        }
        
        Console.WriteLine(stepCount);
    }

    private static IEnumerable<Octopus> GetSurroundingOctopus(Octopus[][] octopusInGrid, int x, int y)
    {
        return Enumerable.Range(x - 1, 3).SelectMany(vX => Enumerable.Range(y - 1, 3).Select(vY => new { x = vX, y = vY }))
            .Where(arg => !(arg.x == x && arg.y == y)).ToArray()
            .Select(arg => GetOctopus(octopusInGrid, arg.x, arg.y))
            .Where(octo => octo != null)
            .ToArray();
    }

    private static Octopus? GetOctopus(Octopus[][] input, int x, int y)
    {
        try
        {
            return input[y][x];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }
}

class Octopus
{
    private int energyLevel;
    
    public Octopus(int initialEnergyLevel)
    {
        energyLevel = initialEnergyLevel;
        SurroundingOctopus = new List<Octopus>();
    }

    public bool Flashed { get; private set; }
    public int FlashCount { get; set; }
    public List<Octopus> SurroundingOctopus { get; }

    public void Flash()
    {
        energyLevel++;
        if (energyLevel > 9 && !Flashed)
        {
            Flashed = true;
            foreach (var surroundingOctopus in SurroundingOctopus)
            {
                surroundingOctopus.Flash();
            }
        }
    }

    public void FinishStep()
    {
        if (energyLevel > 9)
        {
            FlashCount++;
            energyLevel = 0;
            Flashed = false;
        }
    }
}