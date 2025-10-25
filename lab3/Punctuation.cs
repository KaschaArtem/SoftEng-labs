namespace lab3
{
    class Punctuation
    {
        private string value { get; }
        private int positionIndex { get; }

        public string Value 
        { 
            get { return value; }
        }

        public int PositionIndex
        {
            get { return positionIndex; }
        }

        public Punctuation(string value, int positionIndex)
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
