using System.Diagnostics;
using System.Text;
using Day18.Snailfish;

var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

var sw = Stopwatch.StartNew();

var snailFishNumbers = inputString.Split(new[] { '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
    .ToArray();

var reader = new SnailfishReader(snailFishNumbers[0]);

foreach (var snailFishNumber in snailFishNumbers[1..])
{
    reader.AddSnailfishNumber(snailFishNumber);
}

var largestMagnitude = new List<long>();
for (var i = 0; i < snailFishNumbers.Length; i++)
{
    for (var j = 0; j < snailFishNumbers.Length; j++)
    {
        var reader2 = new SnailfishReader(snailFishNumbers[i]);
        reader2.AddSnailfishNumber(snailFishNumbers[j]);
        largestMagnitude.Add(reader2.RootFish.Magnitude);
    }
}

// Console.WriteLine($"Sum check: {"[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]" == reader.RootFish.ToString()}");
// Console.WriteLine($"Magnitude check: {4140 == reader.RootFish.Magnitude}");

sw.Stop();

Console.WriteLine(reader.RootFish.Magnitude);
Console.WriteLine(largestMagnitude.Max());
Console.WriteLine($"{sw.ElapsedMilliseconds}ms");

class Snailfish
{
    public int Depth { get; }

    public Snailfish(int depth)
    {
        Depth = depth;
    }
    public Snailfish? Parent { get; set; }
    public Snailfish? LeftFish { get; set; }
    public Snailfish? RightFish { get; set; }
    public int? LeftLiteral { get; set; }
    public int? RightLiteral { get; set; }
    public long Magnitude => (LeftFish?.Magnitude ?? LeftLiteral.Value) * 3 + (RightFish?.Magnitude ?? RightLiteral.Value) * 2;

    public IEnumerable<Snailfish> GetAll()
    {
        var snails = new List<Snailfish>();
        if (LeftFish != null)
        {
            snails.AddRange(LeftFish.GetAll());    
        }
        
        snails.Add(this);
        
        if (RightFish != null)
        {
            snails.AddRange(RightFish.GetAll());
        }

        return snails;
    }

    public override string ToString()
    {
        return $"[{LeftFish?.ToString() ?? LeftLiteral.ToString()},{RightFish?.ToString() ?? RightLiteral.ToString()}]";
    }
}

class SnailfishReader
{
    private StringBuilder _currentSnailfishSequence = new(string.Empty);
    private IList<Snailfish> OrderedSnailFish = new List<Snailfish>();
    public Snailfish RootFish = null;

    public SnailfishReader(string initialSnailfishNumber)
    {
        _currentSnailfishSequence.Append(initialSnailfishNumber);
    }
    
    public void AddSnailfishNumber(string snailfishNumber)
    {
        _currentSnailfishSequence.Insert(0, '[');
        _currentSnailfishSequence.Append($",{snailfishNumber}]");
        
        
        var index = 1;
        RootFish = ReadFish(ref index, depth: 1);
        OrderedSnailFish = RootFish.GetAll().ToList();
        
        // Console.WriteLine(RootFish.ToString());
        while (Reduce())
        {
            OrderedSnailFish = RootFish.GetAll().ToList();
            // Console.WriteLine(RootFish.ToString());
        }
        // Console.WriteLine(RootFish.ToString());
        
        _currentSnailfishSequence.Clear();
        _currentSnailfishSequence.Append(RootFish);
    }

    private Snailfish ReadFish(ref int index, int depth)
    {
        Snailfish snailFish = new Snailfish(depth);
        OrderedSnailFish.Add(snailFish);
        var digitSequence = string.Empty;
        while (true)
        {
            var c = _currentSnailfishSequence[index];
            index++;
            if (c >= 48 && c <= 57) // digits
            {
                digitSequence += c - 48;
            }
            else if (c == 91) // opening bracket
            {
                if (snailFish.LeftFish == null && !snailFish.LeftLiteral.HasValue)
                {
                    snailFish.LeftFish = ReadFish(ref index, depth + 1);
                    snailFish.LeftFish.Parent = snailFish;
                } else if (snailFish.RightFish == null && !snailFish.RightLiteral.HasValue)
                {
                    snailFish.RightFish = ReadFish(ref index, depth + 1);
                    snailFish.RightFish.Parent = snailFish;
                }
            }
            else if (c == 93) // closing bracket
            {
                if (digitSequence.Length == 0)
                {
                    // Possible right value was a fish, so ignore
                    break;
                }
                var digit = int.Parse(digitSequence);
                snailFish.RightLiteral = digit;
                break;
            }
            else if (c == 44) // comma
            {
                if (digitSequence.Length == 0)
                {
                    // Possible left value was a fish, so ignore
                    continue;
                }
                var digit = int.Parse(digitSequence);
                snailFish.LeftLiteral = digit;
                digitSequence = string.Empty;
            }
        }

        return snailFish;
    }

    private bool Reduce()
    {
        for (var i = 0; i < OrderedSnailFish.Count; i++)
        {
            var snailReference = OrderedSnailFish[i];
            if (snailReference.Depth > 4)
            {
                // explode!
                if (snailReference.LeftFish != null || snailReference.RightFish != null)
                {
                    // break here
                }
                
                if (i > 0)
                {
                    var neighbourFishWithLiteralValue = OrderedSnailFish.Take(i).Reverse().FirstOrDefault(f => f.LeftFish == null || f.RightFish == null);
                    if (neighbourFishWithLiteralValue != null && neighbourFishWithLiteralValue.RightFish == null)
                    {
                        neighbourFishWithLiteralValue.RightLiteral += snailReference.LeftLiteral;
                    } else if (neighbourFishWithLiteralValue != null && neighbourFishWithLiteralValue.LeftFish == null)
                    {
                        neighbourFishWithLiteralValue.LeftLiteral += snailReference.LeftLiteral;
                    }
                }
                
                if (i < OrderedSnailFish.Count - 1)
                {
                    var neighbourFishWithLiteralValue = OrderedSnailFish.Skip(i + 1).FirstOrDefault(f => f.LeftFish == null || f.RightFish == null);
                    if (neighbourFishWithLiteralValue != null && neighbourFishWithLiteralValue.LeftFish == null)
                    {
                        neighbourFishWithLiteralValue.LeftLiteral += snailReference.RightLiteral;
                    } else if (neighbourFishWithLiteralValue != null && neighbourFishWithLiteralValue.RightFish == null)
                    {
                        neighbourFishWithLiteralValue.RightLiteral += snailReference.RightLiteral;
                    }
                }

                OrderedSnailFish.Remove(snailReference);
                if (snailReference.Parent.LeftFish == snailReference)
                {
                    snailReference.Parent.LeftFish = null;
                    snailReference.Parent.LeftLiteral = 0;
                }
                else
                {
                    snailReference.Parent.RightFish = null;
                    snailReference.Parent.RightLiteral = 0;
                }

                return true;
            }
        }


        for (var i = 0; i < OrderedSnailFish.Count; i++)
        {
            var snailReference = OrderedSnailFish[i];
                    
            if (snailReference.LeftFish == null && snailReference.LeftLiteral > 9)
            {
                snailReference.LeftFish = new Snailfish(snailReference.Depth + 1)
                {
                    LeftLiteral = snailReference.LeftLiteral / 2,
                    RightLiteral = (int)Math.Ceiling(snailReference.LeftLiteral.Value / 2d),
                    Parent = snailReference
                };
                snailReference.LeftLiteral = null;

                OrderedSnailFish = OrderedSnailFish.Take(i + 1).Append(snailReference.LeftFish).Concat(OrderedSnailFish.Skip(i + 1)).ToList();
                
                return true;
            }
            
            if (snailReference.RightFish == null && snailReference.RightLiteral > 9)
            {
                snailReference.RightFish = new Snailfish(snailReference.Depth + 1)
                {
                    LeftLiteral = snailReference.RightLiteral / 2,
                    RightLiteral = (int)Math.Ceiling(snailReference.RightLiteral.Value / 2d),
                    Parent = snailReference
                };
                snailReference.RightLiteral = null;

                OrderedSnailFish = OrderedSnailFish.Take(i + 1).Append(snailReference.RightFish).Concat(OrderedSnailFish.Skip(i + 1)).ToList();

                return true;
            }
        }

        return false;
    }
}