using System.Xml.Serialization;

namespace lab3
{
    public enum Type
    {
        Declarative,
        Interrogative,
        Exclamatory
    }
    [Serializable]
    public class Sentence
    {
        [XmlIgnore]
        public List<Word> Words { get; set; } = new List<Word>();

        [XmlIgnore]
        public List<Punctuation> Punctuations { get; set; } = new List<Punctuation>();

        [XmlIgnore]
        public Type SententenceType { get; set; }

        [XmlElement("word", typeof(Word))]
        [XmlElement("punctuation", typeof(Punctuation))]
        public List<object> ItemsForXml
        {
            get
            {
                var items = new List<object>();
                items.AddRange(Words.Cast<object>());
                items.AddRange(Punctuations.Cast<object>());
                return items.OrderBy(item =>
                {
                    if (item is Word word) return word.PositionIndex;
                    else if (item is Punctuation punctuation) return punctuation.PositionIndex;
                    return 0;
                }).ToList();
            }
        }

        public Sentence() { }

        public Sentence(List<Word> words, List<Punctuation> punctuations, Type sentenceType)
        {
            Words = words;
            Punctuations = punctuations;
            SententenceType = sentenceType;
        }

        public void Print() 
        { 
            int word = 0; 
            int punctuation = 0; 
            int maxIndex = Math.Max(Words.Count > 0 ? Words[^1].PositionIndex : 0,
                                    Punctuations.Count > 0 ? Punctuations[^1].PositionIndex : 0);
            bool isPrinted = false; 
            for (int i = 0; i <= maxIndex; i++) 
            { 
                if (word < Words.Count && Words[word].PositionIndex == i) 
                { 
                    if (isPrinted) Console.Write(" ");
                    else isPrinted = true; 
                    Words[word].Print();
                    word++; 
                } 
                if (punctuation < Punctuations.Count && Punctuations[punctuation].PositionIndex == i) 
                { 
                    if (isPrinted) Console.Write(" "); 
                    else isPrinted = true; 
                    Punctuations[punctuation].Print(); 
                    punctuation++; 
                } 
            } 
            Console.Write(" "); 
        }

        public int GetLength()
        {
            int symbols = 0;

            foreach (Word word in Words)
            {
                symbols += word.Value.Length;
            }

            foreach (Punctuation punctuation in Punctuations)
            {
                symbols += punctuation.Value.Length;
            }

            if (Words.Count > 1)
            {
                symbols += Words.Count - 1;
            }

            return symbols;
        }
    }
}
