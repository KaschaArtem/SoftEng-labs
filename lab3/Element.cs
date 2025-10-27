namespace lab3
{
    class Element
    {
        public int Amount { get; set; }
        public HashSet<int> Indexes { get; set; } = new HashSet<int>();

        public Element() { }
    }
}
