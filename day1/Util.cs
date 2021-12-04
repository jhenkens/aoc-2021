public static class Util{
    public static int[] ReadIntsFromFile(string filePath){
        var ints = File.ReadAllLines(filePath).Select(e => int.Parse(e.Trim())).ToArray();
        return ints;
    }
}