using System.Collections;
public static class Day5{
    public static void Run(){
        var day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        var sampleFileName = $"{day}/sample.txt";
        var inputFileName = $"{day}/input.txt";
        var sample = File.ReadAllText(sampleFileName);
        var input = File.ReadAllText(inputFileName);
        
        Console.WriteLine($"*** AoC {day} ***");

        Console.WriteLine($"\n\nBeginning Part1 Sample...");
        var part1SampleOutput = Run(sample, false);
        Console.WriteLine($"\n\nBeginning Part1...");
        var part1Output = Run(input, false);

        Console.WriteLine($"\n\nBeginning Part2 Sample...");
        var part2SampleOutput = Run(sample,true);
        Console.WriteLine($"\n\nBeginning Part2...");
        var part2Output = Run(input, true);
        
        Console.WriteLine($"\n\nResults:");
        Console.WriteLine($"Part1 Sample: {part1SampleOutput}");
        Console.WriteLine($"Part1 Result: {part1Output}");
        Console.WriteLine($"Part2 Sample: {part2SampleOutput}");
        Console.WriteLine($"Part2 Result: {part2Output}");
    }
        public static int Run(string input, bool allowDiagonal){
            var points = new Dictionary<(int,int),int>();
            foreach(var inputLine in input.Split("\n")){
                var line = new Line(inputLine);
                foreach(var point in line.PointsOnLine(allowDiagonal)){
                    if(!points.ContainsKey(point)){
                        points[point]=0;
                    }
                    points[point]++;
                }
            }

            var maxX = points.Max(e => e.Key.Item1);
            var maxY = points.Max(e => e.Key.Item2);
            void DebugPrint(){
                for(int j = 0; j <= maxY; j++){
                    Console.Write($"Y={j}: ");
                    for(int i = 0; i <= maxX; i++){
                        var count = points.TryGetValue((i,j), out var val) ? val : 0;
                        Console.Write(count == 0 ? "." : count.ToString());
                    }
                    Console.WriteLine();
                }
            }

            return points.Values.Where(e => e>1).Count();
        }
        public class Line{
            public Point Start;
            public Point End;
            public Line(string input){
                var parts = input.Split(" -> ").ToArray();
                this.Start = new Point(parts[0]);
                this.End = new Point(parts[1]);
            }
            public IEnumerable<(int,int)> PointsOnLine(bool allowDiagonal){
                void Swap<T>(ref T min, ref T max, Func<T,T,bool> compare){
                    if(compare(min,max)){
                        var temp = min;
                        min = max;
                        max = temp;
                    }
                }
                if(this.Start.X == this.End.X || this.Start.Y == this.End.Y){
                    var minX = this.Start.X;
                    var maxX = this.End.X;
                    Swap(ref minX, ref maxX, (min,max) => min > max);
                    var minY = this.Start.Y;
                    var maxY = this.End.Y;
                    Swap(ref minY, ref maxY, (min,max) => min > max);
                    for(var x  = minX; x <= maxX; x++){
                        for(var y  = minY; y <= maxY; y++){
                            yield return (x,y);
                        }
                    }
                } else if(allowDiagonal){
                    var start = this.Start;
                    var end = this.End;
                    Swap(ref start, ref end, (start,end) => start.X > end.X);
                    var slope = end.Y > start.Y ? 1 : -1;
                    var diff = end.X - start.X;
                    for(int i = 0; i <= diff; i++){
                        var x = start.X + i;
                        var y = start.Y + (i * slope);
                        yield return (x,y);
                    }
                }
            }
        }
        public class Point{
            public int X;
            public int Y;
            public Point(string input){
                var parts = input.Split(",").Select(int.Parse).ToArray();
                this.X = parts[0];
                this.Y = parts[1];
            }   
        }
}