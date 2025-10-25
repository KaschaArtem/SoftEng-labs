namespace lab3
{
    class Word
    {
        private string value { get; }
        private int positionIndex { get; set; }

        public string Value 
        { 
            get { return value; }
        }

        public int PositionIndex
        {
            get { return positionIndex; }
            set { positionIndex = value; }
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
