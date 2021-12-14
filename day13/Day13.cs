using System.Collections;
using System.Text;

public static class Day13
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
        public ((int,int)[], (int?,int?)[]) ParseInput(string input)
        {
            (int?, int?) GetFold(string input)
            {
                var intVal = int.Parse(input[2..]);
                if (input.StartsWith("x="))
                {
                    return (intVal, null);
                }
                return (null, intVal);
            }
            var (gridParts, foldParts) = input.CleanSplit("\n\n").ToTuple();
            var gridValues = gridParts.CleanSplit("\n").Select(e => e.CleanSplit(",").Select(int.Parse).ToArray().ToTuple()).ToArray();
            var foldValues = foldParts.CleanSplit("\n").Select(e => e.CleanSplit(" ").Last()).Select(GetFold).ToArray();
            return (gridValues, foldValues);
        }
        public string RunPart1(((int,int)[], (int?,int?)[]) input)
        {
            var (gridParts, foldParts) = input;
            var gridSet = gridParts.ToHashSet();
           
            var (foldx, foldy) = foldParts[0]; 
            var newSet = new HashSet<(int,int)>();

            foreach( var (pointx, pointy) in gridSet ){
                if(foldx != null){
                    if(pointx < foldx){
                        newSet.Add((pointx, pointy));
                    }else if(pointx > foldx){
                        var newX = (int) (foldx - (pointx - foldx));
                        newSet.Add((newX, pointy));
                    }
                }else if(foldy != null){
                    if(pointy < foldy){
                        newSet.Add((pointx, pointy));
                    }else if(pointy > foldy){
                        var newY = (int) (foldy - (pointy - foldy));
                        newSet.Add((pointx, newY));
                    }
                }
            }
            return newSet.Count.ToString();
        }
        public string RunPart2(((int,int)[], (int?,int?)[]) input)
        {
            var (gridParts, foldParts) = input;
            var gridSet = gridParts.ToHashSet();
           
            foreach( var (foldx, foldy) in foldParts){
                var newSet = new HashSet<(int,int)>();

                foreach( var (pointx, pointy) in gridSet ){
                    if(foldx != null){
                        if(pointx < foldx){
                            newSet.Add((pointx, pointy));
                        }else if(pointx > foldx){
                            var newX = (int) (foldx - (pointx - foldx));
                            newSet.Add((newX, pointy));
                        }
                    }else if(foldy != null){
                        if(pointy < foldy){
                            newSet.Add((pointx, pointy));
                        }else if(pointy > foldy){
                            var newY = (int) (foldy - (pointy - foldy));
                            newSet.Add((pointx, newY));
                        }
                    }
                }
                gridSet = newSet;
            }
            var maxX = gridSet.Max(e => e.Item1);
            var maxY = gridSet.Max(e => e.Item2);
            var minX = gridSet.Min(e => e.Item1);
            var minY = gridSet.Min(e => e.Item2);
            var grid = new bool[(maxX - minX)+1, (maxY-minY)+1];
            foreach(var (x, y) in gridSet){
                grid[x - minX, y - minY] = true;
            }
            var sb = new StringBuilder("\n",grid.Length + grid.GetLength(0));
            for(int i = 0; i < grid.GetLength(1); i++){
                for(int j = 0; j < grid.GetLength(0); j++){
                    sb.Append(grid[j,i] ? "*" : " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}