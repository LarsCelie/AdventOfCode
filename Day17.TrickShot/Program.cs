using System.Diagnostics;
using Day17;

var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

var sw = Stopwatch.StartNew();

var startX = "target area: x=".Length;
var endX = inputString.IndexOf(',') - startX;
var xRange = inputString.Substring(startX, endX);

var startY = inputString.IndexOf('y') + 2;
var yRange = inputString.Substring(startY);

var xStart = int.Parse(xRange.Substring(0, xRange.IndexOf('.')));
var xEnd = int.Parse(xRange.Substring(xRange.IndexOf('.') + 2));

var yEnd = int.Parse(yRange.Substring(0, yRange.IndexOf('.')));
var yStart = int.Parse(yRange.Substring(yRange.IndexOf('.') + 2));

var correctTrajectories = Enumerable.Range(1, xEnd)
    .SelectMany(velocity => Enumerable.Range(yEnd, velocity * 30).Select(angle => new { velocity, angle }))
    .Where(trajectory =>
    {
        var coordinates = new Coordinates(0, 0);
        var step = 0;
        
        while (coordinates.x <= xEnd && coordinates.y >= yEnd)
        {
            coordinates = CalculateNextLocation(coordinates, trajectory.velocity, trajectory.angle, step);
            if (InRange(coordinates, xStart, xEnd, yStart, yEnd))
            {
                return true;
            }

            step++;
        }

        return false;
    }).ToArray();

sw.Stop();

Console.WriteLine($"{correctTrajectories.Length}");
Console.WriteLine($"{sw.ElapsedMilliseconds}ms");

Coordinates CalculateNextLocation(Coordinates coordinates, int initialVelocity, int initialAngle, int step)
{
    var angle = initialAngle - step;
    var velocity = initialVelocity - step;
    if (velocity < 0)
    {
        velocity = 0;
    }

    return new Coordinates(coordinates.x + velocity, coordinates.y + angle);
}

bool InRange(Coordinates coordinates, int xStart, int xEnd, int yStart, int yEnd)
{
    return xStart <= coordinates.x && coordinates.x <= xEnd
                                   && yStart >= coordinates.y && coordinates.y >= yEnd;
}

record struct Coordinates(int x, int y);