using System.Collections;
public static class Day7{
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
        var parts = input.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
        Array.Sort(parts);

        long GetFuel(int target){
            long fuel = 0;
            foreach(var part in parts){
                var diff = (int)Math.Abs(target - part);
                var currentFuel = (part2) ? (diff * (diff + 1)) / 2 : diff;
                fuel += currentFuel;
            }
            return fuel;
        }
        if(!part2){
            var median = parts[parts.Length / 2];
            return GetFuel(median);
        }

        // It would be great to have an implementation that isn't O(n^2) brute force here
        long minFuel = long.MaxValue;
        for(var i = 0; i < parts.Length;i++){
            if(i != 0 && parts[i-1] == parts[i]){
                continue;
            }
            var currentFuel = GetFuel(parts[i]);
            minFuel = Math.Min(currentFuel,minFuel);
        }
        return minFuel;
    }
}