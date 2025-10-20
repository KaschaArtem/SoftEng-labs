using System.Text.RegularExpressions;

namespace lab3
{
    class TextParser
    {
        public Text ParseTextFile(string filePath)
        {
            string content = File.ReadAllText(filePath);
            content = Regex.Replace(content, @"\s+", " ").Trim();

            MatchCollection sentenceMatches = Regex.Matches(content, @"[^.!?]+[.!?]+");

            List<Sentence> sentences = new List<Sentence>();

            foreach (Match match in sentenceMatches)
            {
                string sentenceText = match.Value.Trim();

                List<Word> words = new List<Word>();
                List<Punctuation> punctuations = new List<Punctuation>();

                var tokens = Regex.Matches(sentenceText, @"([\p{L}\p{Nd}\-]+|[^\p{L}\p{Nd}\s]+)");

                string lastToken = tokens[tokens.Count - 1].Value;
                char firstChar = lastToken[0];
                Type type = firstChar switch
                {
                    '?' => Type.Interrogative,
                    '!' => Type.Exclamatory,
                    _ => Type.Declarative
                };

                int index = 0;
                foreach (Match token in tokens)
                {
                    string value = token.Value;

                    if (Regex.IsMatch(value, @"[\p{L}\p{Nd}]"))
                    {
                        words.Add(new Word(value, index));
                    }
                    else
                    {
                        punctuations.Add(new Punctuation(value, index));
                    }

                    index++;
                }

                sentences.Add(new Sentence(words, punctuations, type));
            }

            return new Text(sentences);
        }
    }
}
