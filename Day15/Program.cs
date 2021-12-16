using System.Diagnostics;
using Day15;

var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

var sw = Stopwatch.StartNew();

var lines = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

Node startNode = null;
Node endNode = null;
var nodes = new Dictionary<Coordinates, Node>();

Console.WriteLine("Generating Nodes");
const int multiplier = 5;
foreach (var yAddition in Enumerable.Range(0, multiplier))
{
    for (int y = 0; y < lines.Length; y++)
    {
        foreach (var xAddition in Enumerable.Range(0, multiplier))
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var node = new Node(new Coordinates(x + (xAddition * line.Length), y + (yAddition * lines.Length)), int.Parse(line[x].ToString() + xAddition + yAddition));
                nodes.Add(node.Coordinates, node);

                if (x == 0 && xAddition == 0 && y == 0 && yAddition == 0)
                {
                    startNode = node;
                    startNode.isStartNode = true;
                    startNode.LowestRisk = 0;
                }

                if (node.Coordinates.X == (line.Length * multiplier) - 1 && node.Coordinates.Y == (lines.Length * multiplier) - 1)
                {
                    endNode = node;
                    endNode.isEndNode = true;
                }
            }
        }
    }
}

Console.WriteLine("Assigning neighbours");
foreach (var node in nodes.Values)
{
    var neighbors = new Node?[]
    {
        TryGetNode(nodes, node.Coordinates with { Y = node.Coordinates.Y + 1}),
        TryGetNode(nodes, node.Coordinates with { Y = node.Coordinates.Y - 1}),
        TryGetNode(nodes, node.Coordinates with { X = node.Coordinates.X + 1}),
        TryGetNode(nodes, node.Coordinates with { X = node.Coordinates.X - 1}),
    }.Where(n => n != null).OrderBy(n => n.RiskLevel).ToArray();

    node.Neighbours = neighbors;
}

Node? TryGetNode(Dictionary<Coordinates, Node> nodes, Coordinates coordinates)
{
    nodes.TryGetValue(coordinates, out var node);
    return node;
}

Console.WriteLine("Start pathfinding");
var queue = new List<Node>();
queue.Add(startNode);

while (queue.Count > 0)
{
    var node = queue.ElementAt(0);
    queue.RemoveAt(0);
    
    foreach (var neighbour in node.Neighbours)
    {
        var estimatedLowestRisk = node.LowestRisk + neighbour.RiskLevel;
        if (estimatedLowestRisk < neighbour.LowestRisk)
        {
            neighbour.LowestRisk = estimatedLowestRisk;
            neighbour.PreviousNode = node;

            if (!queue.Contains(neighbour))
            {
                queue.Add(neighbour);
            }
        }
    }

    if (node == endNode)
    {
        break;
    }
}

sw.Stop();

Console.WriteLine($"Shortest path risk level: {endNode.LowestRisk} in {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"Shortest path risk level2: {SumRiskValues(endNode) - startNode.RiskLevel}");

int SumRiskValues(Node node)
{
    return node.RiskLevel + (node.PreviousNode == null ? 0 : SumRiskValues(node.PreviousNode));
}

record struct Coordinates(int X, int Y);

class Node
{
    public Coordinates Coordinates { get; }
    public int RiskLevel { get; }
    public bool isEndNode { get; set; }
    public bool isStartNode { get; set; }
    public long LowestRisk { get; set; } = long.MaxValue;
    public bool Visited { get; set; }
    public Node PreviousNode { get; set; } = null;

    public Node(Coordinates coordinates, int riskLevel)
    {
        Coordinates = coordinates;
        RiskLevel = riskLevel;
        while (RiskLevel > 9)
        {
            RiskLevel -= 9;
        }
    }

    public ICollection<Node> Neighbours { get; set; }
}




