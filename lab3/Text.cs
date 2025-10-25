using static System.Net.Mime.MediaTypeNames;

namespace lab3
{
    class Text
    {
        private List<Sentence> sentences { get; }

        public Text(List<Sentence> sentences)
        {
            this.sentences = sentences;
        }

        public void Print()
        {
            foreach (var sentence in sentences)
            {
                sentence.Print();
            }
        }

        public void PrintByWordsIncreasing()
        {
            var sorted = sentences.OrderBy(s => s.Words.Count);

            foreach (var sentence in sorted)
            {
                sentence.Print();
            }
        }
    }
}
