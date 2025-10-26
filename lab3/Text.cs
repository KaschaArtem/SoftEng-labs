using System.Globalization;
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

        private void PrintRedacted(List<Sentence> redacted)
        {
            foreach (var sentence in redacted)
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

        public void GetUnicWordsByLength(int wordLength) 
        {
            var unicWords = new HashSet<string>();

            foreach (var sentence in sentences)
            {
                if (sentence.SententenceType != Type.Interrogative) { continue; }
                foreach (var word in sentence.Words)
                {   
                    if (word.Value.Length == wordLength) { unicWords.Add(word.Value.ToLower()); }
                }
            }

            Console.WriteLine($"Found {unicWords.Count} words:");
            foreach (var word in unicWords)
            {
                Console.Write($"{word} ");
            }
        }

        public void DeleteWordsByLength(int wordLength)
        {
            string russianConsonants = "бвгджзйклмнпрстфхцчшщ";
            string englishConsonants = "bcdfghjklmnpqrstvwxyz";

            List<Sentence> redacted = new List<Sentence>();
            foreach (var sentence in sentences)
            {
                List<Word> redactedWords = new List<Word>();
                foreach (var word in sentence.Words)
                {
                    var firstChar = word.Value.ToLower()[0];
                    if (word.Value.Length == wordLength)
                    {
                        if (russianConsonants.Contains(firstChar) || englishConsonants.Contains(firstChar)) 
                        { 
                            continue; 
                        }
                    }
                    redactedWords.Add(word);
                }
                List<Punctuation> punctuations = sentence.Punctuations;
                Type sentenceType = sentence.SententenceType;
                redacted.Add(new Sentence(redactedWords, punctuations, sentenceType));
            }
            PrintRedacted(redacted);
        }

        public void PrintSentences()
        {
            for (int i = 0; i < sentences.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                sentences[i].Print();
                Console.WriteLine();
            }
        }

        public void ReplaceWordsByLength(int sentenceIndex, int wordLength, string substring)
        {
            List<Sentence> redacted = sentences;
            foreach (var word in redacted[sentenceIndex].Words)
            {
                if (word.Value.Length == wordLength)
                {
                    word.Value = substring;
                }
            }
            PrintRedacted(redacted);
        }
    }
}
