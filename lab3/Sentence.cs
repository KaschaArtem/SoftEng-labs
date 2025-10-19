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
    }
}
