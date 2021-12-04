using System;
using System.Linq;
using MoreLinq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var bingoCards = Input.BingoCards.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Batch(5)
                .Select(batch => new BingoCard(
                    batch
                        .SelectMany(line => line.Trim().Split())
                        .Where(line => !string.IsNullOrEmpty(line))
                        .Select(stringValue => int.Parse(stringValue.Trim()))))
                .ToArray();

            var callouts = Input.NumberCallouts.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(stringValue => int.Parse(stringValue.Trim()))
                .ToArray();
            
            BingoCard firstWinningBingoCard = null;
            var firstWinningBingoCardCallout = 0;
            BingoCard lastWinningBingoCard = null;
            var callOutIndex = -1;
            while (!bingoCards.All(card => card.HasBingo()))
            {
                callOutIndex++;
                var callout = callouts[callOutIndex];
                bingoCards.ForEach(card => card.MarkNumber(callout));

                if (firstWinningBingoCard == null)
                {
                    firstWinningBingoCard = bingoCards.FirstOrDefault(bingoCard => bingoCard.HasBingo());
                    firstWinningBingoCardCallout = callout;
                }

                if (bingoCards.Count(bingoCard => !bingoCard.HasBingo()) == 1)
                {
                    lastWinningBingoCard = bingoCards.FirstOrDefault(bingoCard => !bingoCard.HasBingo());
                }
            }
            
            Console.WriteLine($"The first winning bingo card has a score of: {firstWinningBingoCard.CalculateAnswer(firstWinningBingoCardCallout)}");
            Console.WriteLine($"The last winning bingo card has a score of: {lastWinningBingoCard.CalculateAnswer(callouts[callOutIndex])}");
        }
    }
}