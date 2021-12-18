// See https://aka.ms/new-console-template for more information

namespace Day10;

class Program
{
    public static void Main(string[] args)
    {
        var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

        var output = PartOne(inputString);
        Console.WriteLine(output);

        var output2 = ParseTwo(inputString);
        Console.WriteLine(output2);
    }

    private static int PartOne(string inputString)
    {
        var pairDictionary = new Dictionary<char, char>()
        {
            { '}', '{' },
            { '>', '<' },
            { ')', '(' },
            { ']', '[' }
        };
        
        var scoreDictionary = new Dictionary<char, int>()
        {
            { '}', 1197 },
            { '>', 25137 },
            { ')', 3 },
            { ']', 57 }
        };

        var output = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s =>
            {
                var stack = new Stack<char>();
                foreach (var c in s)
                {
                    if (pairDictionary.Values.Contains(c))
                    {
                        stack.Push(c);
                    }
                    else if (stack.Peek() == pairDictionary[c])
                    {
                        stack.Pop();
                    }
                    else
                    {
                        // invalid!
                        return c;
                    }
                }

                if (stack.Count > 0)
                {
                    // incomplete
                }

                return (char?)null;
            })
            .Where(c => c.HasValue)
            .Select(c => scoreDictionary[c.Value])
            .Sum();
        return output;
    }
    
    private static long ParseTwo(string inputString)
    {
        var pairDictionary = new Dictionary<char, char>()
        {
            { '}', '{' },
            { '>', '<' },
            { ')', '(' },
            { ']', '[' }
        };
        
        var autoCompleteDictionary = new Dictionary<char, char>()
        {
            {  '{', '}' },
            { '<', '>' },
            { '(', ')' },
            { '[', ']' }
        };
        
        var scoreDictionary = new Dictionary<char, long>()
        {
            { '}', 3 },
            { '>', 4 },
            { ')', 1 },
            { ']', 2 }
        };

        var output = inputString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s =>
            {
                var stack = new Stack<char>();
                foreach (var c in s)
                {
                    if (pairDictionary.Values.Contains(c))
                    {
                        stack.Push(c);
                    }
                    else if (stack.Peek() == pairDictionary[c])
                    {
                        stack.Pop();
                    }
                    else
                    {
                        // invalid!
                        return null;
                    }
                }

                if (stack.Count > 0)
                {
                    return new string(stack.Select(c => c).ToArray());
                }

                return null;
            })
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s.Select(c => scoreDictionary[autoCompleteDictionary[c]]).Aggregate(0L, (a, b) => (a * 5L) + b))
            .OrderBy(value => value)
            .ToArray();

        return output[output.Length / 2];
    }
}