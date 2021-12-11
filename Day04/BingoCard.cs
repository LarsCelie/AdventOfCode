using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Day4
{
    public class BingoCard
    {
        private BingoValue[] bingoValues;
        
        public BingoCard(IEnumerable<int> numbers)
        {
            if (numbers.Count() != 25)
            {
                throw new ArgumentException(nameof(numbers));
            }
            
            bingoValues = numbers.Select((value, index) => new BingoValue(index / 5, index % 5, value))
                .ToArray();
        }

        public bool HasBingo()
        {
            var rowBingo = bingoValues.GroupBy(value => value.Row)
                .Any(group => group.All(v => v.Marked));
            
            var columnBingo = bingoValues.GroupBy(value => value.Column)
                .Any(group => group.All(v => v.Marked));

            return rowBingo || columnBingo;
        }

        public void MarkNumber(int number)
        {
            if (HasBingo())
            {
                return;
            }
            bingoValues.ForEach(bingo =>
            {
                if (bingo.Value == number)
                {
                    bingo.Marked = true;
                }
            });
        }

        public int CalculateAnswer(int lastNumberCalled)
        {
            var sumOfUnusedNumbers = bingoValues.Where(value => !value.Marked)
                .Sum(value => value.Value);

            return sumOfUnusedNumbers * lastNumberCalled;
        }
    }
}