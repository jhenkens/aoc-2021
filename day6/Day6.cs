using System.Collections;
public static class Day6{
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
        return Run(input, 80).ToString();
    }
    public static string RunPart2(string input){
        return Run(input, 256).ToString();
    }
    public static long Run(string input, int numDays, int birthingWindow = 7, int birthingDelay = 2){
        var counts = new long[birthingWindow + birthingDelay];
        foreach(var num in input.Split(",",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse)){
            counts[num]++;
        }
        var offset = 0;
        for(var i = 0; i < numDays; i++){
            var parentsIndex = i % birthingWindow;
            var oldestBabiesIndex = birthingWindow + (i%birthingDelay);
            var birthed = counts[parentsIndex];
            counts[parentsIndex] += counts[oldestBabiesIndex];
            counts[oldestBabiesIndex] = birthed;
            offset = (offset+birthingWindow+1) % birthingWindow;
        }
        return counts.Sum();
    }
}