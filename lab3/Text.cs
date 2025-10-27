using System.Xml.Serialization;

namespace lab3
{
    [Serializable]
    [XmlRoot("text")]
    public class Text
    {
        [XmlElement("sentence")]
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();

        public Text() { }

        public Text(List<Sentence> sentences)
        {
            Sentences = sentences;
        }

        public void Print()
        {
            foreach (var sentence in Sentences)
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
            var sorted = Sentences.OrderBy(sentence => sentence.Words.Count);
            PrintSorted(sorted);
        }

        public void PrintByLengthIncreasing()
        {
            var sorted = Sentences.OrderBy(sentence => sentence.GetLength());
            PrintSorted(sorted);
        }

        public void GetUnicWordsByLength(int wordLength) 
        {
            var unicWords = new HashSet<string>();

            foreach (var sentence in Sentences)
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
            foreach (var sentence in Sentences)
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
            for (int i = 0; i < Sentences.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                Sentences[i].Print();
                Console.WriteLine();
            }
        }

        public void ReplaceWordsByLength(int sentenceIndex, int wordLength, string substring)
        {
            List<Sentence> redacted = Sentences;
            foreach (var word in redacted[sentenceIndex].Words)
            {
                if (word.Value.Length == wordLength)
                {
                    word.Value = substring;
                }
            }
            PrintRedacted(redacted);
        }

        public void DeleteStopWords()
        {
            string dataFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data");
            string[] stopWordsFiles = Directory.GetFiles(dataFolderPath, "stopwords_*.txt");

            HashSet<string> stopWords = new HashSet<string>();
            foreach (var stopWordsFile in stopWordsFiles)
            {
                string[] fileContent = File.ReadAllLines(stopWordsFile);
                foreach (string line in fileContent)
                {
                    stopWords.Add(line);
                }
            }

            List<Sentence> redacted = new List<Sentence>();
            foreach (var sentence in Sentences)
            {
                List<Word> redactedWords = new List<Word>();
                foreach (var word in sentence.Words)
                {
                    if (!stopWords.Contains(word.Value)) {
                        redactedWords.Add(word);
                    }
                }
                List<Punctuation> punctuations = sentence.Punctuations;
                Type sentenceType = sentence.SententenceType;
                redacted.Add(new Sentence(redactedWords, punctuations, sentenceType));
            }
            PrintRedacted(redacted);
        }

        public void GetConcordance() 
        { 
            Dictionary<string, Element> wordStats = new();

            for (int i = 0; i < Sentences.Count; i++)
            {
                foreach (var word in Sentences[i].Words)
                {
                    string w = word.Value.ToLower();
                    if (!wordStats.ContainsKey(w))
                        wordStats[w] = new Element();

                    wordStats[w].Amount++;
                    wordStats[w].Indexes.Add(i + 1);
                }
            }

            foreach (var kvp in wordStats)
            {
                Console.Write($"{kvp.Key} -> {kvp.Value.Amount}: {string.Join(", ", kvp.Value.Indexes)}");
            }
        }

        public void SaveAsXML()
        {
            string textsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Texts\output.xml");

            var serializer = new XmlSerializer(typeof(Text));
            using (var writer = new StreamWriter(textsFolderPath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
