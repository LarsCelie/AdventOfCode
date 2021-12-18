namespace Day4
{
    public class BingoValue
    {
        public BingoValue(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Value = value;
        }
        
        public int Value { get; }
        public int Row { get; }
        public int Column { get; }
        public bool Marked { get; set; }
    }
}