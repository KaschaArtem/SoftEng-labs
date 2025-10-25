namespace lab3
{
    class Word
    {
        private string value { get; }
        private int positionIndex { get; }

        public int PositionIndex
        {
            get { return positionIndex; }
        }

        public Word(string value, int positionIndex) 
        {
            this.value = value;
            this.positionIndex = positionIndex;
        }

        public void Print()
        {
            Console.Write(value);
        }
    }
}
