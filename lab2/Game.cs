namespace lab2
{   
    enum GameState
    {
        Start,
        End
    }

    class Game
    {
        public int size;
        public Player cat;
        public Player mouse;
        public GameState state;

        public Game(int size)
        {
            this.size = size;
            cat = new Player("Cat");
            mouse = new Player("Mouse");
            state = GameState.Start;
        }

        public void Run()
        {
            while (state != GameState.End)
            {

            }
        }

        private void DoMoveCommand()
        {

        }

        private int GetDistance()
        {
            return 0;
        }
    }
}
