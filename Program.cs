// See https://aka.ms/new-console-template for more information
/*Console.WriteLine("Day1_Part1");
Console.WriteLine($"Sample Input: {Day1.NumberOfDecreasing(199,200,208,210,200,207,240,269,260,263)}");
Console.WriteLine($"Problem Input: {Day1.NumberOfDecreasing(Util.ReadIntsFromFile("Day1/input_part1.txt"))}");
Console.WriteLine("Day1_Part2");
Console.WriteLine($"Sample Input: {Day1.NumberOfDecreasingWindows(199,200,208,210,200,207,240,269,260,263)}");
Console.WriteLine($"Problem Input: {Day1.NumberOfDecreasingWindows(Util.ReadIntsFromFile("Day1/input_part1.txt"))}");

Console.WriteLine("Day2_Part1");
Console.WriteLine($"Sample Input: {Day2.Part1.PositionPart2(File.ReadAllLines("Day2/input.txt"))}");
*/
// Day4.Run();
using System.Reflection;

Assembly mscorlib = typeof(Program).Assembly;
var pq = new PriorityQueue<Type, int>();
foreach (Type type in mscorlib.GetTypes())
{
    if(type.Name.StartsWith("Day")){
        pq.Enqueue(type, int.MaxValue - int.Parse(type.Name[3..]));
    }
}
pq.Dequeue().GetMethod("RunAocDay").Invoke(null,null);
