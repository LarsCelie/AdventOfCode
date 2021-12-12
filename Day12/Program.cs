using Day12;

var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

var caves = new List<Cave>();
Cave GetOrCreateCave(string caveId) => caves.FirstOrDefault(cave => cave.Id == caveId) ?? new Cave(caveId);

foreach (var (leftCaveId, rightCaveId) in inputString.Split(new[] { '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
             .Select(line => (line.Split('-')[0], line.Split('-')[1])))
{
    var leftCave = GetOrCreateCave(leftCaveId);
    var rightCave = GetOrCreateCave(rightCaveId);
    
    leftCave.NeighbouringCaves.Add(rightCave);
    rightCave.NeighbouringCaves.Add(leftCave);
    
    caves.Add(leftCave);
    caves.Add(rightCave);
}

caves = caves.DistinctBy(cave => cave.Id).ToList();

var startCave = caves.FirstOrDefault(cave => cave.Id == "start");
var endCave = caves.FirstOrDefault(cave => cave.Id == "end");

IEnumerable<IEnumerable<string>?> GetCaveRoutesUntilEnd(Cave cave, Stack<string> currentRoute, string duplicateSmallCaveId = null)
{
    var routes = new List<IEnumerable<string>>();
    if (currentRoute.Count > 0 && cave.Id == startCave.Id)
    {
        // don't return to start cave
        return routes;
    }
    if (cave.SmallCave && currentRoute.Contains(cave.Id))
    {
        if (!string.IsNullOrEmpty(duplicateSmallCaveId) && duplicateSmallCaveId == cave.Id && currentRoute.Count(r => r == duplicateSmallCaveId) < 2)
        {
            // ok
        }
        else
        {
            return routes;
        }
    }
    currentRoute.Push(cave.Id);

    if (cave.Id == endCave.Id)
    {
        routes.Add(currentRoute.ToArray());
    }
    else
    {
        routes.AddRange(cave.NeighbouringCaves.SelectMany(cav => GetCaveRoutesUntilEnd(cav, new Stack<string>(currentRoute.Reverse()), duplicateSmallCaveId).ToArray()));
        if (string.IsNullOrEmpty(duplicateSmallCaveId))
        {
            routes.AddRange(cave.NeighbouringCaves.Where(c => c.SmallCave).SelectMany(c => GetCaveRoutesUntilEnd(c, new Stack<string>(currentRoute.Reverse()), c.Id).ToArray()));
        }
    }
    return routes;
}

var routes = GetCaveRoutesUntilEnd(startCave, new Stack<string>())
    .Where(route => route != null)
    .ToArray();

Console.WriteLine($"Possible paths: {routes.Select(r => string.Join(',', r.Reverse())).Distinct().Count()}");

class Cave
{
    public string Id { get; }
    public bool SmallCave { get; }

    public Cave(string id)
    {
        Id = id;
        SmallCave = id == id.ToLower();
        NeighbouringCaves = new List<Cave>();
    }
    
    public List<Cave> NeighbouringCaves { get; }
}