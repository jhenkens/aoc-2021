using System.Collections;
using System.Diagnostics;
using System.Text;

public static class Day15
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

        var files = Directory
            .EnumerateFiles($"{day}/","*.txt")
            .OrderByDescending(e => Path.GetFileName(e).StartsWith($"sample"))
            .ThenBy(e => e)
            .Select(e => (name: e, content: solver.ParseInput(File.ReadAllText(e))))
            .ToArray()
            ;
        Console.WriteLine($"Found input files:\n{string.Join("\n",files.Select(f => $"  {f.name}"))}");

        var results = new StringBuilder("Results:\n");

        foreach(var p in new[]{"Part1", "Part2"}){
            Console.WriteLine($"\nExecuting {p}...");
            foreach(var file in files){
                Console.WriteLine($"  on {file.name}");
                var method = typeof(Solver).GetMethod($"Run{p}");
                var sw = Stopwatch.StartNew();
                var result = method.Invoke(solver, new[]{file.content});
                sw.Stop();
                Console.WriteLine($"  ..{file.name} took {sw.Elapsed.TotalSeconds} seconds\n");
                results.AppendLine($"{p} - {file.name}: {result}");
            }
        }
        Console.WriteLine();
        Console.WriteLine(results.ToString());
    }

    public class Solver
    {
        public Solver() { }
        public int[][] ParseInput(string input)
        {
            return input.CleanSplit("\n").Select(e => e.ToCharArray().Select(c => (int)(c - '0')).ToArray()).ToArray();
        }
        public string RunPart1(int[][] input) => Run(input);

        public string RunPart2(int[][] input)
        {
            return Run(input, 5);
        }
        public string Run(int[][] input, int gridSize = 1)
        {
            bool InRange((int, int) dims)
            {
                var (dim1, dim2) = dims;
                return dim1 >= 0 && dim1 < gridSize*input.Length && dim2 >= 0 && dim2 < gridSize*input[0].Length;
            }
            IEnumerable<(int, int)> GetNeighbors((int, int) vertex)
            {
                var (dim1, dim2) = vertex;
                return new[] { (dim1 - 1, dim2), (dim1 + 1, dim2), (dim1, dim2 - 1), (dim1, dim2 + 1) }
                    .Where(InRange).ToArray();
            }
            var stepDistances = new Dictionary<(int,int), long>();
            var distances = new Dictionary<(int,int), long>();
            var distancesQueue = new PriorityQueue<(int,int), long>();
            var vertices = new HashSet<(int,int)>();
            var prev = new Dictionary<(int, int), (int, int)?>();
            // var queue = new Queue<(int, int)>();
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[0].Length; j++)
                {
                    for( var m1 = 0; m1 < gridSize; m1++ ){
                        for( var m2 = 0; m2 < gridSize; m2++ ){
                            var vertex = (i+(m1*input.Length),j+(m2*input[0].Length));
                            var distance = vertex == (0,0) ? 0 : int.MaxValue;
                            distances.Add(vertex, distance);
                            distancesQueue.Enqueue(vertex, distance);
                            vertices.Add(vertex);
                            prev[vertex] = null;
                            var stepDistance = input[i][j] + m1 + m2;
                            stepDistance = ( (stepDistance - 1) % 9) + 1;
                            stepDistances[vertex] = stepDistance;
                        }
                    }
                }
            }
            var max = vertices.OrderByDescending(e => e.Item1).ThenByDescending(e => e.Item2).First();

            while(vertices.Any()){
                var success = false;
                (int,int) vertex;
                while ((success = distancesQueue.TryDequeue(out vertex, out var p)) && !vertices.Contains(vertex) ){
                    // empty body, try again till P = p
                }
                if(!success){
                    throw new Exception("No valid vertices?");
                }
                vertices.Remove(vertex);
                var currentDistance = distances[vertex];
                foreach(var nVertex in GetNeighbors(vertex).Where(vertices.Contains)){
                    var alt = currentDistance + stepDistances[nVertex];
                    var currentNeighborDistance = distances[nVertex];
                    if(alt < currentNeighborDistance){
                        distances[nVertex] = alt;
                        distancesQueue.Enqueue(nVertex, alt);
                        prev[nVertex] = vertex;
                    }
                }
            }
            return distances[max].ToString();
        }
    }
}