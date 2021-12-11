using System;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Input.Value.Split('\n').ToArray();
            
            RunPuzzlePartOne(input);
            
            Console.WriteLine("Part 2:");
            RunPuzzlePartTwo(input);
        }

        public static void RunPuzzlePartOne(string[] input)
        {
            var horizontalMovement = 0;
            var depth = 0;
            
            foreach (var movement in input)
            {
                var directionAndAmount = movement.Split();
                var direction = directionAndAmount[0];
                var amount = int.Parse(directionAndAmount[1]);

                switch (direction)
                {
                    case "forward": horizontalMovement += amount;
                        break;
                    case "up": depth -= amount;
                        break;
                    case "down": depth += amount;
                        break;
                };
            }
            
            Console.WriteLine($"The submarine moved {horizontalMovement} forward and is currently at {depth} depth");
            Console.WriteLine($"The answer is: {horizontalMovement * depth}");
        }
        
        public static void RunPuzzlePartTwo(string[] input)
        {
            var horizontalMovement = 0;
            var depth = 0;
            var aim = 0;
            
            foreach (var movement in input)
            {
                var directionAndAmount = movement.Split();
                var direction = directionAndAmount[0];
                var amount = int.Parse(directionAndAmount[1]);

                switch (direction)
                {
                    case "forward": 
                        horizontalMovement += amount;
                        depth += (amount * aim);
                        break;
                    case "up": aim -= amount;
                        break;
                    case "down": aim += amount;
                        break;
                };
            }
            
            Console.WriteLine($"The submarine moved {horizontalMovement} forward and is currently at {depth} depth");
            Console.WriteLine($"The answer is: {horizontalMovement * depth}");
        }
    }
}