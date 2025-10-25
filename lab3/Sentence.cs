using System.Diagnostics;

namespace lab3
{
    enum Type
    {
        Declarative,
        Interrogative,
        Exclamatory
    }
    class Sentence
    {
        private List<Word> words { get; }
        private List<Punctuation> punctuations { get; }
        private Type sentenceType { get; }

        public List<Word> Words {
            get { return words; }
        }

        public List<Punctuation> Punctuations
        {
            get { return punctuations; }
        }

        public Type SententenceType 
        { 
            get { return sentenceType; } 
        }

        public Sentence(List<Word> words, List<Punctuation> punctuations, Type sentenceType)
        {
            this.words = words;
            this.punctuations = punctuations;
            this.sentenceType = sentenceType;
        }

        public void Print()
        {
            int word = 0;
            int punctuation = 0;
            int maxIndex = Math.Max(
                words.Count > 0 ? words[^1].PositionIndex : 0,
                punctuations.Count > 0 ? punctuations[^1].PositionIndex : 0
            );

            bool isPrinted = false;
            for (int i = 0; i <= maxIndex; i++)
            {
                if (word < words.Count && words[word].PositionIndex == i)
                {
                    if (isPrinted) Console.Write(" ");
                    else isPrinted = true;
                        words[word].Print();
                    word++;
                }

                if (punctuation < punctuations.Count && punctuations[punctuation].PositionIndex == i)
                {
                    if (isPrinted) Console.Write(" ");
                    else isPrinted = true;
                    punctuations[punctuation].Print();
                    punctuation++;
                }
            }

            Console.Write(" ");
        }

        public int GetLength()
        {
            int symbols = 0;

            foreach (Word word in words)
            {
                symbols += word.Value.Length;
            }

            foreach (Punctuation punctuation in punctuations)
            {
                symbols += punctuation.Value.Length;
            }

            if (words.Count > 1)
            {
                symbols += words.Count - 1;
            }

            return symbols;
        }
    }
}
