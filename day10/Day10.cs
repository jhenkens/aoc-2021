using System.Collections;
public static class Day10{
    public static void RunAocDay(){
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
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToArray();
        if(!part2){
            return Part1(lines);
        }
        return Part2(lines);
    }
    public static long ValuePart1(char invalidChar){
        switch(invalidChar){
            case ')': return 3;
            case ']': return 57;
            case '}': return 1197;
            case '>': return 25137;
        }
        throw new Exception();
    }

    public static long ValuePart2(char invalidChar){
        switch(invalidChar){
            case '(': return 1;
            case '[': return 2;
            case '{': return 3;
            case '<': return 4;
        }
        throw new Exception();
    }

    public static long Part1(IEnumerable<string> lines){
        return lines.Sum(CheckLineForErrorValues);
    }

    public static long Part2(IEnumerable<string> lines){
        var correctionValues = lines.Where(e =>CheckLineForErrorValues(e) == 0).Select(FindIncompleteLineValue).OrderBy(e=>e).ToArray();
        return correctionValues[correctionValues.Length/2];
    }
    public static long CheckLineForErrorValues(string input){
        var stack = new Stack<char>();
        foreach( var c in input){
            switch(c){
                case '(':
                case '[':
                case '{':
                case '<':
                    stack.Push(c);
                    break;
                case ')':
                case ']':
                case '}':
                case '>':
                    var desired = c == ')' ? '(' : (char)(c - 2);
                    if(stack.Pop() != desired){
                        return ValuePart1(c);
                    }
                    break;
            }
        }
        return 0;
    }
    public static long FindIncompleteLineValue(string input){
        var stack = new Stack<char>();
        foreach( var c in input){
            switch(c){
                case '(':
                case '[':
                case '{':
                case '<':
                    stack.Push(c);
                    break;
                case ')':
                case ']':
                case '}':
                case '>':
                    var desired = c == ')' ? '(' : (char)(c - 2);
                    if(stack.Pop() != desired){
                        throw new Exception();
                    }
                    break;
            }
        }
        return Enumerable.Range(0,stack.Count).Select(e => stack.Pop()).Aggregate((long)0, (a,c) => a*5 + ValuePart2(c));
    }
}