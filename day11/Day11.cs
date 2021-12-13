using System.Collections;
public static class Day11
{
    public static void RunAocDay()
    {
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

    public static string RunPart1(string input)
    {
        return Run(input).ToString();
    }
    public static string RunPart2(string input)
    {
        return Run(input, true).ToString();
    }
    public static long Run(string input, bool part2 = false)
    {
        var grid = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.ToCharArray().Select(e => e - '0').ToArray())
            .ToArray();
        if (!part2)
        {
            var result = Part1(grid, 100);
            // var str = string.Join("\n",grid.Select(i => string.Join("",i.Select(j => j.ToString()))));
            // Console.WriteLine(str);
            return result;
        }
        return Part2(grid);
    }
    public static long Part2(int[][] grid)
    {
        const int flashingLimit = 10;
        for (var step = 0; ; step++)
        {
            var flashing = new List<(int, int)>();
            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (++grid[i][j] == flashingLimit)
                    {
                        flashing.Add((i, j));
                    }
                }
            }
            for (var f = 0; f < flashing.Count; f++)
            {
                var center = flashing[f];
                foreach (var (i,j) in GetNeighbors(center.Item1,center.Item2,grid))
                {
                    if(grid[i][j] < flashingLimit){
                        if(++grid[i][j] == flashingLimit){
                            flashing.Add((i,j));
                        }
                    }
                }
            }
            foreach(var (i,j) in flashing){
                grid[i][j] = 0;
            }
            if(flashing.Count == grid.Length * grid[0].Length){
                return step;
            }
        }
        return -1;
    }
    public static long Part1(int[][] grid, int steps)
    {
        const int flashingLimit = 10;
        var sum = 0;
        for (var step = 0; step < steps; step++)
        {
            var flashing = new List<(int, int)>();
            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (++grid[i][j] == flashingLimit)
                    {
                        flashing.Add((i, j));
                    }
                }
            }
            for (var f = 0; f < flashing.Count; f++)
            {
                var center = flashing[f];
                foreach (var (i,j) in GetNeighbors(center.Item1,center.Item2,grid))
                {
                    if(grid[i][j] < flashingLimit){
                        if(++grid[i][j] == flashingLimit){
                            flashing.Add((i,j));
                        }
                    }
                }
            }
            foreach(var (i,j) in flashing){
                grid[i][j] = 0;
            }
            sum += flashing.Count;
        }
        return sum;
    }
    private static IEnumerable<(int, int)> GetNeighbors(int i, int j, int[][] items)
    {
        var possibles = new[]{
            (i-1,j-1),
            (i-1,j),
            (i-1,j+1),
            (i,j-1),
            (i,j+1),
            (i+1,j-1),
            (i+1,j),
            (i+1,j+1),
        };
        foreach (var p in possibles)
        {
            if (p.Item1 < 0 || p.Item2 < 0 || p.Item1 >= items.Length || p.Item2 >= items[p.Item1].Length)
            {
                continue;
            }
            yield return p;
        }
    }
}