using System;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Input.Value.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            Console.WriteLine("Part 1");
            RunPuzzlePartOne(input);
            
            Console.WriteLine();
            Console.WriteLine("-----------");
            Console.WriteLine();
            
            Console.WriteLine("Part 2");
            RunPuzzlePartTwo(input);
        }

        private static void RunPuzzlePartOne(string[] input)
        {
            var binaryLength = input[0].Length;
            var gammaRateChars = new char[binaryLength];
            for (var i = 0; i < binaryLength; i++)
            {
                gammaRateChars[i] = input.Select(s => s[i])
                    .GroupBy(s => s)
                    .OrderByDescending(group => group.Count())
                    .Select(g => g.Key)
                    .First();
            }

            var gammaRate = new string(gammaRateChars);
            var gammaRateDecimal = Convert.ToInt32(new string(gammaRate), 2);
            
            Console.WriteLine($"Gamma rate is: {gammaRate}, which is in decimal: {gammaRateDecimal}");

            var epsilonRateChars = new char[binaryLength];
            for (var i = 0; i < gammaRate.Length; i++)
            {
                var inverted = gammaRate[i] switch
                {
                    '1' => '0',
                    '0' => '1',
                    _ => throw new ArgumentException()
                };

                epsilonRateChars[i] = inverted;
            }
            
            var epsilonRate = new string(epsilonRateChars);
            var epsilonRateDecimal = Convert.ToInt32(new string(epsilonRate), 2);
            
            Console.WriteLine($"Epsilon rate is: {epsilonRate}, which is in decimal: {epsilonRateDecimal}");
            
            Console.WriteLine($"Answer is: {gammaRateDecimal * epsilonRateDecimal}");
        }
        
        private static void RunPuzzlePartTwo(string[] input)
        {
            var oxygenGeneratorRating = FilterMostSignificant(input);
            var oxygenGeneratorRatingDecimal = Convert.ToInt32(new string(oxygenGeneratorRating), 2);
            var oxygenScrubbingRating = FilterLeastSignificant(input);
            var oxygenScrubbingRatingDecimal = Convert.ToInt32(new string(oxygenScrubbingRating), 2);
            
            Console.WriteLine($"Oxygen Generator Rating rate is: {oxygenGeneratorRating}, which is in decimal: {oxygenGeneratorRatingDecimal}");
            Console.WriteLine($"Oxygen Scrubbing Rating rate is: {oxygenScrubbingRating}, which is in decimal: {oxygenScrubbingRatingDecimal}");
            Console.WriteLine($"Answer is: {oxygenGeneratorRatingDecimal * oxygenScrubbingRatingDecimal}");
        }

        private static string FilterMostSignificant(string[] input, int indexChar = 0)
        {
            var significantBit = GetMostSignificantBit(input, indexChar);
            var filteredInput = input.Where(s => s[indexChar] == significantBit)
                .ToArray();

            return filteredInput.Length == 1 ? filteredInput.First() : FilterMostSignificant(filteredInput, indexChar + 1);
        }

        private static char GetMostSignificantBit(string[] input, int indexChar)
        {
            const char tieBreaker = '1';
            var countedChars = input.Select(s => s[indexChar])
                .GroupBy(s => s)
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToArray();

            return countedChars.First().Count == countedChars.Last().Count ? tieBreaker : countedChars.First().Key;
        }
        
        private static string FilterLeastSignificant(string[] input, int indexChar = 0)
        {
            var significantBit = GetLeastSignificantBit(input, indexChar);
            var filteredInput = input.Where(s => s[indexChar] == significantBit)
                .ToArray();

            return filteredInput.Length == 1 ? filteredInput.First() : FilterLeastSignificant(filteredInput, indexChar + 1);
        }

        private static char GetLeastSignificantBit(string[] input, int indexChar)
        {
            const char tieBreaker = '0';
            var countedChars = input.Select(s => s[indexChar])
                .GroupBy(s => s)
                .Select(g => new { g.Key, Count = g.Count() })
                .OrderBy(g => g.Count)
                .ToArray();

            return countedChars.First().Count == countedChars.Last().Count ? tieBreaker : countedChars.First().Key;
        }
    }
}