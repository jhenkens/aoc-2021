public static class Day3{
    private static string[] Example = new string[]{
        "00100",
        "11110",
        "10110",
        "10111",
        "10101",
        "01111",
        "00111",
        "11100",
        "10000",
        "11001",
        "00010",
        "01010",
    };
    public static void Run(){
        Console.WriteLine($"Day 3, Part 1:");
        Console.WriteLine($"Epsiolon * Gamma: {Part1.Run(File.ReadAllLines("Day3/input.txt"))}");
        Console.WriteLine($"Day 3, Part 2:");
        Console.WriteLine($"Example : {(Part2.Run(Example))}");
        Console.WriteLine($"Result: {Part2.Run(File.ReadAllLines("Day3/input.txt"))}");
    }
    public static class Part1{
        public static int Run(params string[] input){
            var counts = new int[input[0].Trim().Length];
            foreach( var s in input){
                for(int i = 0; i < s.Length; i++){
                    counts[i] += (s[i] == '0' ? -1 : 1);
                }
            }
            var gamma = 0;
            var epsiolon = 0;
            foreach( var count in counts){
                gamma <<= 1;
                epsiolon <<=1 ;
                if(count > 0){
                    gamma +=1;
                } else{
                    epsiolon += 1;
                }
            }
            return gamma * epsiolon;
        }
    }
    public static class Part2{
        public static int Run(params string[] input){
            return FilterStrings(input, true, 1) * FilterStrings(input,false,0);
        }
        public static int FilterStrings(string[] input, bool mostCommon, int defaultValue){
            var inputList = new List<string>(input);
            var pos = 0;
            while(inputList.Count > 1){
                var count = 0;
                foreach(var s in inputList){
                    count += (s[pos] == '0' ? -1 : 1);
                }
                inputList = inputList.Where(s => {
                    if(count == 0) return s[pos] == '0' + defaultValue;
                    if(mostCommon ^ count > 0){
                        return s[pos] == '0';
                    }else{
                        return s[pos] == '1';
                    }

                }).ToList();
                pos++;
            }
            return Convert.ToInt32(inputList[0],2);
        }
    }
}