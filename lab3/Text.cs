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

        private void PrintSorted(IEnumerable<Sentence> sorted)
        {
            foreach (var sentence in sorted)
            {
                sentence.Print();
            }
        }

        public void PrintByWordsIncreasing()
        {
            var sorted = sentences.OrderBy(sentence => sentence.Words.Count);
            PrintSorted(sorted);
        }

        public void PrintByLengthIncreasing()
        {
            var sorted = sentences.OrderBy(sentence => sentence.GetLength());
            PrintSorted(sorted);
        }
    }
}
