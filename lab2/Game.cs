using System.Text;

namespace lab2
{   
    enum GameState
    {
        Start,
        End
    }

    class Game
    {
        public static int size;
        public Player cat;
        public Player mouse;
        public GameState state;

        public static string? InputFile;
        public static string? OutputFile;

        public Game(int size)
        {
            cat = new Player("Cat");
            mouse = new Player("Mouse");
            state = GameState.Start;
        }

        public void Run()
        {
            cat.state = State.Playing;
            mouse.state = State.Playing;

            StringBuilder OutputContent = new StringBuilder();
            OutputContent.Append("Cat and Mouse\n\nCat Mouse  Distance\n-------------------\n");

            foreach (string line in File.ReadLines(InputFile!))
            {


                if (cat.location == mouse.location)
                {
                    cat.state = State.Winner;
                    mouse.state = State.Looser;
                    break;
                }
            }

            if (cat.state == State.Playing) 
            {
                cat.state = State.Looser;
                cat.state = State.Winner;
            }

            OutputContent.Append($"-------------------\n\n\nDistance traveled:   Mouse    Cat\n");
            OutputContent.AppendFormat("{0, -21}{1, 5}{2, 7}\n\n", "", mouse.distanceTraveled, cat.distanceTraveled);

            if (cat.state == State.Winner) { OutputContent.Append($"Mouse caught at: {cat.location}"); }
            else { OutputContent.Append("Mouse evaded Cat"); }

            File.WriteAllText(OutputFile!, OutputContent.ToString());

            cat.state = State.NotInGame;
            mouse.state = State.NotInGame;
        }

        private void DoMoveCommand(Player player, int steps)
        {
            player.Move(steps);
        }

        private int GetDistance()
        {
            return Math.Abs(cat.location - mouse.location);
        }
    }
}
