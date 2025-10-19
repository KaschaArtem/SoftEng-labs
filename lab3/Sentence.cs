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

        public Sentence(List<Word> words, List<Punctuation> punctuations, char endingPunctuation)
        {
            this.words = words;
            this.punctuations = punctuations;
            sentenceType = SetSentenceType(endingPunctuation);
        }

        private Type SetSentenceType(char endingPunctuation) {
            switch (endingPunctuation)
            {
                case '.':
                    return Type.Declarative;
                case '?':
                    return Type.Interrogative;
                case '!':
                    return Type.Exclamatory;
            }
            return Type.Declarative;
        }

        public void Print()
        {
            int currentWordIndex = 0;
            int currentPunctuationIndex = 0;
            for (int i = 0; i < words.Count + 1; i++)
            {
                bool isPrinted = false;
                if (currentWordIndex < words.Count &&
                    i == words[currentWordIndex].PositionIndex)
                {
                    words[currentWordIndex].Print();
                    currentWordIndex++;
                    isPrinted = true;
                }
                if (currentPunctuationIndex < punctuations.Count &&
                    i == punctuations[currentPunctuationIndex].PositionIndex)
                {
                    punctuations[currentPunctuationIndex].Print();
                    currentPunctuationIndex++;
                    isPrinted = true;
                }
                if (isPrinted) { Console.Write(" "); }
            }
        }
    }
}
