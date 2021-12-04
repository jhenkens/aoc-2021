class Day2{
    public class Part1{
        private class Step {
            public int depth;
            public int aim;
            public int down;
            public int forward;
            public int horizontal;
        }
        public static (int,int) Position(params string[] input ){
            return input.Select(ParseRow).Aggregate((0,0), (a,b) => (a.Item1 + b.down, a.Item1 + b.forward));
        }

        private static Step ParseRow(string input){
            var parts = input.Split(' ');
            var offset = int.Parse(parts[1]);
            switch(parts[0]){
                case "forward":
                    return new Step{down=0,forward=offset};
                case "up":
                    offset = -offset;
                    break;
            }
            return new Step{down=offset,forward=0};
        }
        public static int PositionPart2(params string[] input ){
            int depth = 0;
            int horizontal = 0;
            int aim = 0;
            foreach( var step in input.Select(ParseRow)){
                aim += step.down;
                horizontal += step.forward;
                depth += aim*step.forward;
            }
            return (depth*horizontal);
        }
    }
}