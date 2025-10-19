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

        public Sentence(List<Word> words, List<Punctuation> punctuations, Type sentenceType)
        {
            this.words = words;
            this.punctuations = punctuations;
            this.sentenceType = sentenceType;
        }
    }
}
