using System.Collections;
using System.Text;

public static class Day14
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
        var part2SampleOutput = solver.RunPart2(solver.ParseInput(sample));
        Console.WriteLine($"\n\nBeginning Part2...");
        var part2Output = solver.RunPart2(solver.ParseInput(input));

        Console.WriteLine($"\n\nResults:");
        Console.WriteLine($"Part1 Sample: {part1SampleOutput}");
        Console.WriteLine($"Part1 Result: {part1Output}");
        Console.WriteLine($"Part2 Sample: {part2SampleOutput}");
        Console.WriteLine($"Part2 Result: {part2Output}");
    }

    public class Solver
    {
        public Solver() { }
        public (string, Dictionary<string, char>) ParseInput(string input)
        {
            var (pattern, insertions) = input.CleanSplit("\n\n").ToTuple();
            var insertionDictionary = insertions.CleanSplit("\n").Select(e => e.CleanSplit(" -> ").ToTuple()).ToDictionary(e => e.Item1, e => e.Item2[0]);
            return (pattern, insertionDictionary);
        }
        public string RunPart1((string, Dictionary<string, char>) input, int iterations = 10)
        {
            var inputAray = input.Item1.ToCharArray();
            var patternList = new LinkedList<char>(inputAray);
            for (var i = 0; i < iterations; i++)
            {
                var head = patternList.First;
                while (head?.Next != null)
                {
                    var nextHead = head.Next;
                    if (input.Item2.TryGetValue(new string(new char[] { head.Value, head.Next.Value }), out var insertion))
                    {
                        patternList.AddAfter(head, insertion);
                    }
                    head = nextHead;
                }
            }
            var ordered = patternList.GroupBy(e => e).Select(e => (e.Key, e.Count())).OrderBy(e => e.Item2).ToArray();
            var min = ordered.First().Item2;
            var max = ordered.Last().Item2;
            return (max-min).ToString();
        }

        public string RunPart2((string, Dictionary<string, char>) input)
        {
            var tuples = new Dictionary<string, long>();

            void IncrementString(Dictionary<string,long> t, string tuple, long amount){
                if(t.TryGetValue(tuple, out var value)){
                    amount = value+amount;
                }
                t[tuple] = amount;
            }

            void IncrementChar(Dictionary<string,long> t, char c1, char c2, long amount = 1) => IncrementString(t, new string(new[]{c1,c2}), amount);

            for(int i = 0; i < input.Item1.Length-1; i++){
                IncrementChar(tuples, input.Item1[i], input.Item1[i+1]);
            }
            
            for (var i = 0; i < 40; i++)
            {
                var newTuples = new Dictionary<string,long>();
                foreach(var kvp in tuples){
                    if(input.Item2.TryGetValue(kvp.Key, out var r)){
                        IncrementChar(newTuples, kvp.Key[0], r, kvp.Value);
                        IncrementChar(newTuples, r, kvp.Key[1], kvp.Value);
                    }else{
                        IncrementString(newTuples, kvp.Key, kvp.Value);
                    }
                }
                tuples = newTuples;
            }
            var chars = tuples.SelectMany(e => new[]{(e.Key[0], e.Value), (e.Key[1],e.Value)});
            // Weird, but first and last don't get duplicated by counts, need to add one extra since we divide by two.
            chars = chars.Prepend((input.Item1[0],1)).Append((input.Item1.Last(),1));
            var charCounts = chars.GroupBy(e => e.Item1).Select(e => (e.Key, e.Sum(v => v.Value) / 2)).OrderBy(e => e.Item2).ToArray();

            var min = charCounts.First().Item2;
            var max = charCounts.Last().Item2;
            return (max-min).ToString();
        }
    }
}