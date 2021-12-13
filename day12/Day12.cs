using System.Collections;
public static class Day12
{
    public static void RunAocDay()
    {
        var day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        var sampleFileName = $"{day}/sample.txt";
        var sample2FileName = $"{day}/sample2.txt";
        var sample3FileName = $"{day}/sample3.txt";
        var inputFileName = $"{day}/input.txt";
        var sample = File.ReadAllText(sampleFileName);
        var sample2 = File.ReadAllText(sample2FileName);
        var sample3 = File.ReadAllText(sample3FileName);
        var input = File.ReadAllText(inputFileName);

        Console.WriteLine($"*** AoC {day} ***");

        Console.WriteLine($"\n\nBeginning Part1 Sample...");
        var part1SampleOutput = RunPart1(sample);
        var part1Sample2Output = RunPart1(sample2);
        var part1Sample3Output = RunPart1(sample3);
        
        Console.WriteLine($"\n\nBeginning Part1...");
        var part1Output = RunPart1(input);

        Console.WriteLine($"\n\nBeginning Part2 Sample...");
        var part2SampleOutput = RunPart2(sample);
        var part2Sample2Output = RunPart2(sample2);
        var part2Sample3Output = RunPart2(sample3);

        Console.WriteLine($"\n\nBeginning Part2...");
        var part2Output = RunPart2(input);

        
        Console.WriteLine($"\n\nResults:");
        Console.WriteLine($"Part1 Sample: {part1SampleOutput}");
        Console.WriteLine($"Part1 Sample2: {part1Sample2Output}");
        Console.WriteLine($"Part1 Sample3: {part1Sample3Output}");
        Console.WriteLine($"Part1 Result: {part1Output}");

        Console.WriteLine($"Part2 Sample: {part2SampleOutput}");
        Console.WriteLine($"Part2 Sample2: {part2Sample2Output}");
        Console.WriteLine($"Part2 Sample3: {part2Sample3Output}");
        Console.WriteLine($"Part2 Result: {part2Output}");
        
    }

    public static string RunPart1(string input)
    {
        return Run(input).ToString();
    }
    public static string RunPart2(string input)
    {
        return Run(input, true).ToString();
    }
    public class CaveNode
    {
        public bool IsStart;
        public bool IsEnd;
        public bool IsSmallCave;
        public string Token;
        public readonly HashSet<string> Neighbors = new();
        public CaveNode(string token)
        {
            if (token.All(e => e <= 'z' && e >= 'a'))
            {
                IsSmallCave = true;
            }
            if (token == "start")
            {
                IsStart = true;
            }
            if (token == "end")
            {
                IsEnd = true;
            }
            Token = token;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is CaveNode node && this.Equals(node);
        }

        public bool Equals(CaveNode other)
        {
            return other != null && this.Token == other.Token;
        }

        public override int GetHashCode() => this.Token.GetHashCode();

        public override string? ToString() => this.Token.ToString();
    }
    public static long Run(string input, bool part2 = false){
        var nodes = new Dictionary<string, CaveNode>();
        foreach (var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var parts = line.Split("-", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var part in parts)
            {
                if (!nodes.TryGetValue(part, out var currentNode))
                {
                    nodes[part] = currentNode = new CaveNode(part);
                }
                foreach (var neighbor in parts.Where(e => e != part))
                {
                    currentNode.Neighbors.Add(neighbor);
                }
            }
        }
        var start = nodes.Single(kvp => kvp.Value.IsStart);
        var end = nodes.Single(kvp => kvp.Value.IsEnd);
        return FindAllPaths(nodes, start.Value, end.Value, null, part2);
    }

    public static int FindAllPaths(Dictionary<string, CaveNode> caves, CaveNode start, CaveNode end, Stack<string> path, bool visitOneTwice)
    {
        if (path == null)
        {
            path = new Stack<string>();
        }
        if (start.Equals(end))
        {
            return 1;
        }
        if(start.IsStart && path.Contains(start.Token)){
            return 0;
        }
        if(start.IsSmallCave && path.Contains(start.Token)){
            if(visitOneTwice){
                visitOneTwice = false;
            } else{
                return 0;
            }
        }
        path.Push(start.Token);
        var sum = 0;
        foreach (var neighbor in start.Neighbors)
        {
            sum += FindAllPaths(caves, caves[neighbor], end, path, visitOneTwice);
        }
        path.Pop();
        return sum;
    }
}