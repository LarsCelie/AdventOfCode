using System;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    public class SeaFloor
    {
        private int[][] _floor;
        public SeaFloor(IEnumerable<FissureRange> ranges)
        {
            var xRange = ranges.Select(range => Math.Max(range.StartLocation.X, range.EndLocation.X))
                .Max() + 1;
            var yRange = ranges.Select(range => Math.Max(range.StartLocation.Y, range.EndLocation.Y))
                .Max() + 1;
            _floor = new int[xRange][];
            for (var i = 0; i < xRange; i++)
            {
                _floor[i] = new int[yRange];
            }
            
            // Change into GetHorizontalAndVerticalCoordinates for part one
            foreach (var coordinate in ranges.SelectMany(range => range.GetHorizontalAndVerticalAndDiagonalCoordinates()))
            {
                var currentValue = _floor[coordinate.X][coordinate.Y];
                _floor[coordinate.X][coordinate.Y] = currentValue + 1;
            }
        }

        public int CountDuplicates()
        {
            return _floor
                .SelectMany(row => row)
                .Count(value => value > 1);
        }

        public void PrintMap()
        {
            foreach (var row in _floor)
            {
                Console.WriteLine();
                foreach (var cell in row)
                {
                    Console.Write(cell);
                }
            }
        }
    }
}