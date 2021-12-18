using System.Diagnostics;
using System.Text;
using Day16;

const int LiteralValueIdentifier = 4;  
const string TotalLengthTypeInBits = "0";  

var inputString = args.Length > 0 ? Input.TestValue : Input.Value;

var sw = Stopwatch.StartNew();

var binaryString = string.Join(string.Empty, inputString.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), fromBase: 16), toBase: 2).PadLeft(totalWidth: 4, paddingChar:'0')));

var rootPacket = ReadPacket(new PacketReader(binaryString));

// var part1 = ReadVersionSum(new[] { rootPacket });
var part2 = rootPacket.CalculateValue();

sw.Stop();


// Console.WriteLine($"Version sum: {part1} in {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"Value: {part2} in {sw.ElapsedMilliseconds} ms");

long ReadVersionSum(ICollection<Packet> packets)
{
    return packets.Sum(packet => packet.Version + ReadVersionSum(packet.SubPackets));
}

Packet ReadPacket(PacketReader packetReader)
{
    var version =  Convert.ToInt32(string.Join(string.Empty, packetReader.Read(3)), fromBase: 2);
    var id = Convert.ToInt32(string.Join(string.Empty, packetReader.Read(3)), fromBase: 2);
    
    // Console.WriteLine($"Reading packet with version '{version}' and id '{id}'");

    if (id != LiteralValueIdentifier)
    {
        // Console.WriteLine($"Reading subpackets");
        
        var lengthTypeIdentifier = packetReader.Read(1);
        if (lengthTypeIdentifier == TotalLengthTypeInBits)
        {
            var totalNumberBits = Convert.ToInt32(packetReader.Read(15), fromBase: 2);
            var stopAtIndex = packetReader.CurrenCharIndex + totalNumberBits;
            
            var subPackets = new List<Packet>();
            while (packetReader.CurrenCharIndex < stopAtIndex)
            {
                var packet = ReadPacket(packetReader);
                subPackets.Add(packet);
            }
            
            return new Packet(version, id, subPackets);
        }
        else
        {
            var totalNumberOfSubPackages = Convert.ToInt32(packetReader.Read(11), fromBase: 2);
            var subPackets = Enumerable.Range(0, totalNumberOfSubPackages)
                .Select(x => ReadPacket(packetReader))
                .ToList();

            return new Packet(version, id, subPackets);
        }
    }
    else
    {
        var valueGroup = "1";
        var totalValue = new StringBuilder();
        while (valueGroup[0] != '0')
        {
            valueGroup = packetReader.Read(5);
            totalValue.Append(valueGroup[1..]);
        }

        // Console.WriteLine($"Reading literalValue: {Convert.ToInt64(totalValue.ToString(), fromBase: 2)}");
        
        return new Packet(version, id, Convert.ToInt64(totalValue.ToString(), fromBase: 2));
    }
}

class Packet
{
    private long? _literalValue;
    public Packet(int version, int id, long value)
    {
        Version = version;
        Id = id;
        _literalValue = value;
        SubPackets = new List<Packet>();
    }

    public Packet(int version, int id, ICollection<Packet> subPackets)
    {
        Version = version;
        Id = id;
        SubPackets = subPackets;
    }
    
    public long Version { get; }
    public int Id { get; }
    public ICollection<Packet> SubPackets { get; }

    public long CalculateValue()
    {
        return Id switch
        {
            0 => SubPackets.Sum(packet => packet.CalculateValue()),
            1 => SubPackets.Aggregate(1L, (i, packet) => i * packet.CalculateValue()),
            2 => SubPackets.Min(packet => packet.CalculateValue()),
            3 => SubPackets.Max(packet => packet.CalculateValue()),
            4 => _literalValue!.Value,
            5 => SubPackets.First().CalculateValue() > SubPackets.Last().CalculateValue() ? 1L : 0L,
            6 => SubPackets.First().CalculateValue() < SubPackets.Last().CalculateValue() ? 1L : 0L,
            7 => SubPackets.First().CalculateValue() == SubPackets.Last().CalculateValue() ? 1L : 0L,
            _ => throw new NotSupportedException()
        };
    }
}

class PacketReader
{
    private readonly string _packetString;
    public int CurrenCharIndex { get; private set; }

    public PacketReader(string packetString)
    {
        _packetString = packetString;
    }

    public string Read(int charCount)
    {
        var packetPiece = _packetString[CurrenCharIndex..(CurrenCharIndex + charCount)];
        CurrenCharIndex += charCount;
        return packetPiece;
    }
}