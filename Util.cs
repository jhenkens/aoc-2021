public static class Util
{
    public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
    {
        first = list.Count > 0 ? list[0] : default(T); // or throw
        rest = list.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
    {
        first = list.Count > 0 ? list[0] : default(T); // or throw
        second = list.Count > 1 ? list[1] : default(T); // or throw
        rest = list.Skip(2).ToList();
    }
    public static int[] ReadIntsFromFile(string filePath)
    {
        var ints = File.ReadAllLines(filePath).Select(e => int.Parse(e.Trim())).ToArray();
        return ints;
    }

    public static (T,T) ToTuple<T>(this IList<T> input){
        if(input.Count != 2){
            throw new Exception("Invalid length");
        }
        return (input[0], input[1]);
    }

    public static (T,T) ToTuple<T>(this T[] input){
        if(input.Length != 2){
            throw new Exception("Invalid length");
        }
        return (input[0], input[1]);
    }

    public static string[] CleanSplit(this string input, params string[] split){
        return input.Split(split, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}