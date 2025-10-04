namespace lab2
{
    enum State
    {
        Winner,
        Looser,
        Playing,
        NotInGame
    }

    class Player
    {
        public string name;
        public int location;
        public State state = State.NotInGame;
        public int distanceTraveled = 0;

        public Player(string name) 
        {
            this.name = name;
            this.location = 0;
        }

        public void Move(int steps)
        {
            location += steps;
            if (location < 0) location += Game.size;
            if (location > Game.size) location -= Game.size;
            distanceTraveled += Math.Abs(steps);
        }
    }
}
