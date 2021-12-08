using System.Collections;
public static class Day8{
    public static void Run(){
        var day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        var sampleFileName = $"{day}/sample.txt";
        var inputFileName = $"{day}/input.txt";
        var sample = File.ReadAllText(sampleFileName);
        var input = File.ReadAllText(inputFileName);
        
        Console.WriteLine($"*** AoC {day} ***");

        Console.WriteLine($"\n\nBeginning Part1 Sample...");
        var part1SampleOutput = RunPart1(sample);
        Console.WriteLine($"\n\nBeginning Part1...");
        var part1Output = RunPart1(input);

        Console.WriteLine($"\n\nBeginning Part2 Sample...");
        var part2SampleOutput = RunPart2(sample);
        Console.WriteLine($"\n\nBeginning Part2...");
        var part2Output = RunPart2(input);
        
        Console.WriteLine($"\n\nResults:");
        Console.WriteLine($"Part1 Sample: {part1SampleOutput}");
        Console.WriteLine($"Part1 Result: {part1Output}");
        Console.WriteLine($"Part2 Sample: {part2SampleOutput}");
        Console.WriteLine($"Part2 Result: {part2Output}");
    }

    public static string RunPart1(string input){
        return Run(input).ToString();
    }
    public static string RunPart2(string input){
        return Run(input, true).ToString();
    }
    public static long Run(string input, bool part2 = false){
        var pieces = input.Split("\n").Select(e => e.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToArray();
        var inputs = pieces.Select(e => e.First().Split(' ').ToArray()).ToArray();
        var outputs = pieces.Select(e => e.Last().Split(' ').ToArray()).ToArray();
        // foreach( var output in outputs){
        //     var uniqueLength = output.Where(e => new[]{2,3,4,7}.Contains(e.Length));
        //     // Console.WriteLine(string.Join(" ", uniqueLength));
        // }
        if(!part2){
            var uniqueLengths = new[]{2,3,4,7};
            return outputs.SelectMany(e => e).Where(e => uniqueLengths.Contains(e.Length)).Count();
        }
        return inputs.Zip(outputs).Select(e => FindOutput(e.First, e.Second)).Sum();
    }

    public static readonly Dictionary<string,int> SegmentLookup = new Dictionary<string, int>(){
        {"abcefg", 0},
        {"cf",1},
        {"acdeg",2},
        {"acdfg",3},
        {"bcdf",4},
        {"abdfg",5},
        {"abdefg",6},
        {"acf",7},
        {"abcdefg",8},
        {"abcdfg",9},
    };

    public static int FindOutput(string[] input, string[] output){
        string SortString(string s){
            var chars = s.ToCharArray().OrderBy(e => e).ToArray();
            return new string(chars);
        }
        var all = input.Concat(output).Select(SortString).GroupBy(e => e.Length).ToDictionary(e => e.Key, e => e.Distinct().Select(e => e.ToCharArray()).ToArray());

        var mapping = new Dictionary<char,char>();
        var letterOne = all[2].Single();
        var letterFour = all[4].Single();
        var letterSeven = all[3].Single();
        mapping[letterSeven.Except(letterOne).Single()] = 'a';
        var lengthSix = all[6];
        var cde = lengthSix.SelectMany(e => e).GroupBy(e => e).Where(e => e.Count() == 2).Select(e => e.Key).ToArray();
        mapping.Add(cde.Intersect(letterOne).Single(),'c');
        mapping.Add(letterOne.Except(mapping.Keys).Single(), 'f');
        mapping.Add(cde.Except(letterFour).Single(),'e');
        mapping.Add(cde.Except(mapping.Keys).Single(),'d');
        mapping.Add(letterFour.Except(mapping.Keys).Single(), 'b');
        mapping[all[7].Single().Except(mapping.Keys).Single()] = 'g';

        var result = 0;
        foreach(var outputChar in output){
            var segments = new List<char>();
            foreach(var segment in outputChar){
                segments.Add(mapping[segment]);
            }
            result *= 10;
            result += SegmentLookup[new String(segments.OrderBy(e => e).ToArray())];
        }
        return result;
    }
}