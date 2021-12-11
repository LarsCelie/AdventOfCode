using System;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    public class FissureRange
    {
        public readonly FissureCoordinate StartLocation;
        public readonly FissureCoordinate EndLocation;

        public FissureRange(FissureCoordinate startLocation, FissureCoordinate endLocation)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
        }

        public IEnumerable<FissureCoordinate> GetHorizontalAndVerticalCoordinates()
        {
            if (StartLocation.X == EndLocation.X)
            {
                var lowest = Math.Min(StartLocation.Y, EndLocation.Y);
                var highest = Math.Max(StartLocation.Y, EndLocation.Y);
                return Enumerable.Range(lowest, highest - lowest + 1)
                    .Select(value => new FissureCoordinate(StartLocation.X, value))
                    .ToArray();
            }

            if (StartLocation.Y == EndLocation.Y)
            {
                var lowest = Math.Min(StartLocation.X, EndLocation.X);
                var highest = Math.Max(StartLocation.X, EndLocation.X);
                return Enumerable.Range(lowest, highest - lowest + 1)
                    .Select(value => new FissureCoordinate(value, StartLocation.Y))
                    .ToArray();
            }

            return Enumerable.Empty<FissureCoordinate>();
        }

        public IEnumerable<FissureCoordinate> GetHorizontalAndVerticalAndDiagonalCoordinates()
        {
            var coordinates = GetHorizontalAndVerticalCoordinates().ToList();
            
            if (coordinates.Count == 0)
            {
                
                // 1,1 -> 9,9
                var lowestX = Math.Min(StartLocation.X, EndLocation.X);
                var yWithLowestX = StartLocation.X == lowestX ? StartLocation.Y : EndLocation.Y;
                var lowestYIsStartPosition = Math.Min(StartLocation.Y, EndLocation.Y) == yWithLowestX;
                
                var highestX = Math.Max(StartLocation.X, EndLocation.X);

                return Enumerable.Range(lowestX, highestX - lowestX + 1)
                    .Select((value, index) => new FissureCoordinate(value, (((lowestYIsStartPosition ? 1 : -1) * index) + yWithLowestX)))
                    .ToArray();
            }

            return coordinates;
        }
    }
}