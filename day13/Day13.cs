using System.Collections;
public static class _Day13
{
    public static void RunAocDay()
    {
        var day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        var sampleFileName = $"{day}/sample.txt";
        var inputFileName = $"{day}/input.txt";
        var sample = File.ReadAllText(sampleFileName);
        var input = File.ReadAllText(inputFileName);

        Console.WriteLine($"*** AoC {day} ***");
        var solver = new Solver();

        Console.WriteLine($"\n\nBeginning Part1 Sample...");
        var part1SampleOutput = solver.RunPart1(solver.ParseInput(sample));
        Console.WriteLine($"\n\nBeginning Part1...");
        var part1Output = solver.RunPart1(solver.ParseInput(input));

        Console.WriteLine($"\n\nBeginning Part2 Sample...");
        var part2SampleOutput = solver.RunPart1(solver.ParseInput(sample));
        Console.WriteLine($"\n\nBeginning Part2...");
        var part2Output = solver.RunPart2(solver.ParseInput(input));

        Console.WriteLine($"\n\nResults:");
        Console.WriteLine($"Part1 Sample: {part1SampleOutput}");
        Console.WriteLine($"Part1 Result: {part1Output}");
        Console.WriteLine($"Part2 Sample: {part2SampleOutput}");
        Console.WriteLine($"Part2 Result: {part2Output}");
    }

    public class Solver{
        public Solver(){}
        public object ParseInput(string input){
            return null;
        }
        public string RunPart1(object input){
            return null;
        }
        public string RunPart2(object input){
            return null;
        }
    }
}