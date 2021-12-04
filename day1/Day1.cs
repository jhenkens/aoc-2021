class Day1{
    ///
    /// Day 1 Part 1 - find number of successively increasing values (i.e., [3,1,2] -> 1)
    ///
    public static int NumberOfDecreasing(params int[] ints){
        int increasing = 0;
        for(int i = 1; i < ints.Length; i++){
            if(ints[i-1] < ints[i]){
                increasing++;
            }
        }
        return increasing;
    }

    public static int NumberOfDecreasingWindows(params int[] ints){
        int increasing = 0;
        for(int i = 3; i < ints.Length; i++){
            if( ints[i-3] < ints[i] ) increasing++;
        }
        return increasing;
    }
}