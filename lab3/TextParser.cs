using System.Text.RegularExpressions;

namespace lab3
{
    class TextParser
    {
        public Text ParseTextFile(string inputFilePath)
        {
            List<Sentence> sentences = new List<Sentence>();

            List<Word> words = new List<Word>();
            List<Punctuation> punctuations = new List<Punctuation>();

            foreach (string paragraph in File.ReadLines(inputFilePath))
            {
                string[] tokens = paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int currentWordIndex = -1;

                foreach (string token in tokens)
                {
                    currentWordIndex++;

                    Match match = Regex.Match(token, @"^(\w+)?([^\w]*)$");
                    string word = match.Groups[1].Value;
                    string punctuation = match.Groups[2].Value;

                    if (!string.IsNullOrEmpty(word))
                    {
                        words.Add(new Word(word, currentWordIndex));
                    }

                    if (!string.IsNullOrEmpty(punctuation))
                    {
                        punctuations.Add(new Punctuation(punctuation, currentWordIndex));

                        if (punctuation[0] == '.' ||
                            punctuation[0] == '?' ||
                            punctuation[0] == '!')
                        {
                            sentences.Add(new Sentence(words, punctuations, punctuation[0]));
                            words = new List<Word>();
                            punctuations = new List<Punctuation>();
                            currentWordIndex = -1;
                        }
                    }
                }
            }

            return new Text(sentences);
        }
    }
}
