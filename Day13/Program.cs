using Day13;

var inputString = args.Length > 0 ? Input.TestValue : Input.Value;
var foldInstructionsString = args.Length > 0 ? Input.TestFoldInstructions : Input.FoldInstructions;

var coordinates = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
    .Select(line =>
    {
        var xAndY = line.Split(',');
        return new Coordinate(int.Parse(xAndY[0]), int.Parse(xAndY[1]));
    })
    .ToArray();

var foldInstructions = foldInstructionsString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
    .Select(line =>
    {
        var split = line.Split('=');
        return new { axis = split[0].Last(), value = int.Parse(split[1]) };
    })
    .ToArray();

foreach (var instruction in foldInstructions)
{
    if (instruction.axis == 'y')
    {
        foreach (var coordinate in coordinates)
        {
            coordinate.FoldHorizontally(instruction.value);
        }
    }
    else
    {
        foreach (var coordinate in coordinates)
        {
            coordinate.FoldVertically(instruction.value);
        }
    }
    Console.WriteLine($"visible dots: {coordinates.DistinctBy(c => new { c.X, c.Y }).Count()}");
}

var uniqueCoords = coordinates.DistinctBy(c => new { c.X, c.Y }).ToArray();

foreach (var line in uniqueCoords
             .GroupBy(c => c.Y)
             .OrderBy(c => c.Key))
{
    Console.WriteLine();
    for (var i = 0; i <= line.Max(l => l.X); i++)
    {
        Console.Write(line.FirstOrDefault(l => l.X == i) == null ? "." : "#");
    }
}

class Coordinate
{
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; private set; }
    public int Y { get; private set; }
    public void FoldHorizontally(int foldValue)
    {
        if (Y > foldValue)
        {
            Y = foldValue - (Y - foldValue);
        }
    }

    public void FoldVertically(int foldValue)
    {
        if (X > foldValue)
        {
            X = foldValue - (X - foldValue);
        }
    }
}