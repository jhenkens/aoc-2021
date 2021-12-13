using System.Collections;
public static class Day9{
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
        int[][] pieces = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(e => e.ToCharArray().Select(c => c - '0').ToArray() ).ToArray();
        if(!part2){
            var lows = FindRelativeLows(pieces).ToArray();
            return lows.Select(e => e.low + 1).Sum();
        }
        var pq = new PriorityQueue<int,int>();
        for(int y = 0; y < pieces.Length; y++){
            for(int x = 0; x < pieces[y].Length; x++){
                var currentSize = FindCurrentBasinSize(pieces, x, y);
                pq.Enqueue(currentSize, int.MaxValue - currentSize); 
            }
        }
        return pq.Dequeue() * pq.Dequeue() * pq.Dequeue();
    }

    public static IEnumerable<(int low, int x, int y)> FindRelativeLows(int[][] input){
        for(int y = 0; y < input.Length; y++){
            for(int x = 0; x < input[y].Length; x++){
                if(x != 0 && input[y][x-1] <= input[y][x]){
                    continue;
                }
                if(x != input[y].Length-1 && input[y][x+1] <= input[y][x]){
                    continue;
                }
                if(y != 0 && input[y-1][x] <= input[y][x]){
                    continue;
                }
                if(y != input.Length-1 && input[y+1][x] <= input[y][x]){
                    continue;
                }
                yield return (input[y][x], x, y);
            }
        }
    }

    public static int FindCurrentBasinSize(int[][] input, int x, int y){
        if(x < 0 || y < 0 || y >= input.Length || x >= input[y].Length) return 0;
        if(input[y][x] == 9) return 0;
        input[y][x] = 9;
        return 1 + 
            FindCurrentBasinSize(input, x-1, y) + 
            FindCurrentBasinSize(input, x+1, y) + 
            FindCurrentBasinSize(input, x, y-1) + 
            FindCurrentBasinSize(input, x, y+1);
    }
}