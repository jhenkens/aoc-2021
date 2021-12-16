using System.Collections;
using System.Diagnostics;
using System.Text;

public static class Day16
{
    public static void RunAocDay()
    {
        var day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();

        Console.WriteLine($"*** AoC {day} ***");
        var solver = new Solver();

        var files = Directory
            .EnumerateFiles($"{day}/", "*.txt")
            .OrderByDescending(e => Path.GetFileName(e).StartsWith($"sample"))
            .ThenBy(e => e)
            .Select(e => (name: e, content: solver.ParseInput(File.ReadAllText(e))))
            .ToArray()
            ;
        Console.WriteLine($"Found input files:\n{string.Join("\n", files.Select(f => $"  {f.name}"))}");

        var results = new StringBuilder("Results:\n");

        foreach (var p in new[] { "Part1", "Part2" })
        {
            Console.WriteLine($"\nExecuting {p}...");
            foreach (var file in files)
            {
                Console.WriteLine($"  on {file.name}");
                var method = typeof(Solver).GetMethod($"Run{p}");
                var sw = Stopwatch.StartNew();
                var result = method.Invoke(solver, new[] { file.content });
                sw.Stop();
                Console.WriteLine($"  ..{file.name} took {sw.Elapsed.TotalSeconds} seconds");
                Console.WriteLine($"  Result: {result}\n");
                results.AppendLine($"{p} - {file.name}: {result}");
            }
        }
        Console.WriteLine();
        Console.WriteLine(results.ToString());
    }

    public class Solver
    {
        public Solver() { }
        public byte[] ParseInput(string input)
        {
            byte[] StringToByteArray(string hex)
            {
                return Enumerable.Range(0, hex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                 .ToArray();
            }
            return StringToByteArray(input.Trim());
        }
        public string RunPart1(byte[] input) => Run(input).GetAllPackets().Sum(e => e.Version).ToString();

        public string RunPart2(byte[] input) => Run(input).GetResult().ToString(); //Run(input);
        public Packet Run(byte[] input)
        {
            long offset = 0;
            return new Packet(input, ref offset);
        }

        public enum TypeEnum : byte{
            Sum = 0,
            Product = 1,
            Min = 2,
            Max = 3,
            Literal = 4,
            Gt = 5,
            Lt = 6,
            Eq = 7,
        }
        public class Packet{
            public byte Version;
            public TypeEnum Type;
            public List<Packet> SubPackets = new();
            public long LiteralValue;
            public Packet(byte[] rawData, ref long bitOffset){
                Version = ReadByte(rawData, ref bitOffset, 3);
                Type = (TypeEnum) ReadByte(rawData, ref bitOffset, 3);
                switch(Type){
                    case TypeEnum.Literal:
                        var bytes = ParseLiteralNumber(rawData, ref bitOffset);
                        LiteralValue = bytes.Aggregate((long)0, (a,b) => a << 4 | b);
                        break;
                    default:
                        var typeLengthId = ReadByte(rawData, ref bitOffset, 1);
                        if(typeLengthId == 0){
                            var lengthInBytes = ReadLong(rawData, ref bitOffset, 15);
                            var end = bitOffset + lengthInBytes;
                            while(bitOffset < end){
                                SubPackets.Add(new Packet(rawData, ref bitOffset));
                            }
                        }else{
                            var lengthInPackets = ReadLong(rawData, ref bitOffset, 11);
                            while(lengthInPackets-- > 0){
                                SubPackets.Add(new Packet(rawData, ref bitOffset));
                            }
                        }
                        break;
                }
            }

            public IEnumerable<Packet> GetAllPackets(){
                yield return this;
                foreach( var subPacket in this.SubPackets.SelectMany(e => e.GetAllPackets())){
                    yield return subPacket;
                }
            }

            public long GetResult(){
                var subPacketResults = this.SubPackets.Select(e => e.GetResult());
                long a,b;
                switch(Type){
                    case TypeEnum.Literal:
                        return LiteralValue;
                    case TypeEnum.Sum:
                        return subPacketResults.Sum();
                    case TypeEnum.Product:
                        return subPacketResults.Aggregate((a,b) => a*b);
                    case TypeEnum.Max:
                        return subPacketResults.Max();
                    case TypeEnum.Min:
                        return subPacketResults.Min();
                    case TypeEnum.Lt:
                        (a,b) = subPacketResults.ToList().ToTuple();
                        return a < b ? 1 : 0;
                    case TypeEnum.Gt:
                        (a,b) = subPacketResults.ToList().ToTuple();
                        return a > b ? 1 : 0;
                    case TypeEnum.Eq:
                        (a,b) = subPacketResults.ToList().ToTuple();
                        return a == b ? 1 : 0;
                }
                throw new Exception();
            }

            public List<byte> ParseLiteralNumber(byte[] rawData, ref long bitOffset){
                var result = new List<byte>();
                while(true){
                    var read = ReadByte(rawData, ref bitOffset, 5);
                    result.Add((byte)(read & 0b1111));
                    const int flag = 0b10000;
                    if((read & flag) != flag){
                        break;
                    }
                }
                return result;
            }

            public static long ReadLong(byte[] rawData, ref long bitOffset, byte length){
                var remainder = length % 8;
                var dividend = length / 8;
                long result = 0;
                if(length > 64){
                    throw new Exception();
                }
                if(remainder != 0){
                    result = ReadByte(rawData, ref bitOffset, (byte) remainder);
                }
                while(dividend-- > 0){
                    result <<= 8;
                    result |= ReadByte(rawData, ref bitOffset, 8);
                }
                return result;
            }

            public static byte ReadByte(byte[] rawData, ref long bitOffset, byte bits = 8){
                var target = bitOffset / 8;
                var readFromSecond = (byte) bitOffset % 8;
                var splitRead = readFromSecond + bits > 8; // I.e., reading 8, offset 1, reading 7, offset 2
                var result = (byte)((rawData[target] << readFromSecond) & 0xFF);
                if(splitRead){
                    result |= (byte)((rawData[target+1] >> 8-readFromSecond) & 0xFF);
                }
                if(bits < 8){
                    result >>= 8-bits;
                }
                bitOffset+= bits;
                return result;
            }
        }
    }
}