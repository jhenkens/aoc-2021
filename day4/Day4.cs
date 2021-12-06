using System.Collections;
public static class Day4{
    public static void Run(){
        Console.WriteLine($"Part1 Sample: {(Part1.Result(File.ReadAllText("day4/sample.txt")))}");
        Console.WriteLine($"Part1 Result: {(Part1.Result(File.ReadAllText("day4/input.txt")))}");
        Console.WriteLine($"Part2 Sample: {(Part2.Result(File.ReadAllText("day4/sample.txt")))}");
        Console.WriteLine($"Part2 Result: {(Part2.Result(File.ReadAllText("day4/input.txt")))}");
    }

    public static List<BingoBoard> Bingo(string lines){
        var boardsParts = lines.Split("\n\n");
        int[] sequence = boardsParts[0].Split(',').Select(int.Parse).ToArray();
        var boards = new List<BingoBoard>();
        foreach( var boardString in boardsParts.Skip(1) ){
            boards.Add(new BingoBoard(boardString.Split('\n').ToArray()));
        }
        for(int i = 0; i < sequence.Length; i++){
            foreach( var board in boards ){
                board.Mark(sequence[i],i);
            }
        }
        return boards;
    }
    public static IEnumerable<BingoBoard> OrderedBingos(List<BingoBoard> boards){
        return boards.Where(e => e.Result != null).OrderBy(e => e.Result.index);
    }
    public static class Part1{
        public static int Result(string lines){
            var bingo = OrderedBingos(Bingo(lines)).First();
            return bingo.SumUnmarked() * bingo.Result.value;
        }
    }
    public static class Part2{
        public static int Result(string lines){
            var bingo = OrderedBingos(Bingo(lines)).Last();
            return bingo.SumUnmarked() * bingo.Result.value;
        }
    }
    public class BingoBoard{
        public BingoResult Result = null;
        public class BingoResult{
            public int index;
            public int value;

        }
        public int[] Board;
        public BingoBoard(string[] lines){
            var parts = lines.SelectMany(e => e.Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries)).ToArray();
            Board = parts.Select(int.Parse).ToArray();
            if(Board.Length != 25){
                throw new Exception("Wrong board size");
            }
        }
        public override string ToString()
        {
            return 
            $"Bingo board: bingo({Result?.index}), bingoVal({Result?.value}), sumUnmarked({SumUnmarked()})\n" +
            string.Join("\n",Enumerable.Range(0,5).Select(
                i => string.Join(" ",Enumerable.Range(0,5).Select(j =>{
                    var board = Board[i*5+j];
                    return $"{board:00}";
                }))
            )) + "\n";
        }
        public int SumUnmarked(){
            return Board.Where(e => e != -1).Sum();
        }
        public bool IsBingo(int startPos = -1){
            bool CheckRow(int row){
                return Board.Skip(row*5).Take(5).Sum() == -5;
            }
            bool CheckCol(int col){
                return Enumerable.Range(0,5).Select(i => Board[i*5 + col]).Sum() == -5;
            }
            if(startPos != -1){
                if(CheckRow(startPos /5)) return true;
                if(CheckCol(startPos %5)) return true;
            } else{
                for(int i = 0; i < 5; i++){
                    if(CheckRow(i)) return true;
                    if(CheckCol(i)) return true;
                }
            }
            return false;
        }
        public void Mark(int val, int index){
            if(this.Result != null) return;
            for(int i = 0 ; i < Board.Length; i++){
                if(Board[i] == val){
                    Board[i] = -1;
                    if(IsBingo(i)){
                        this.Result = new BingoResult(){
                            index = index,
                            value = val
                        };
                        return;
                    }
                    break;
                }
            }
        }
    }
}
