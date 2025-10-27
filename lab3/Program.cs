using System.Xml.Serialization;

namespace lab3
{
    class Program
    {
        private static string? GetTextFilePath()
        {
            string textsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Texts");
            string[] textFiles = Directory.GetFiles(textsFolderPath, "*.txt");

            if (!textFiles.Any())
            {
                Console.WriteLine("No text files found!");
                return null;
            }

            Console.WriteLine("Choose text file:");
            for (int i = 0; i < textFiles.Length; i++)
                Console.WriteLine($"{i + 1}) {Path.GetFileNameWithoutExtension(textFiles[i])}");

            int fileChoice;
            while (!int.TryParse(Console.ReadLine(), out fileChoice) || fileChoice < 1 || fileChoice > textFiles.Length)
                Console.WriteLine("Wrong input!");

            return textFiles[fileChoice - 1];
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string? inputFilePath = GetTextFilePath();
            if (inputFilePath == null) return;

            TextParser textParser = new TextParser();
            Text text = textParser.ParseTextFile(inputFilePath);

            int optionchoise;
            while (true)
            {
                Console.Write("\n\nChoose option:\n" +
                    "0. Exit\n" +
                    "1. Output sentences by increasing of words amount\n" +
                    "2. Output sentences by increasing of length\n" +
                    "3. Find unic words in interrogative sentences by input length\n" +
                    "4. Delete all words with first char is consonant by input length\n" +
                    "5. Replace words with input substring by input length in choosen sentence\n" +
                    "6. Delete all stop words in text\n" +
                    "7. Get concordance of text\n" +
                    "8. Save text as XML\n"
                );
                while (!int.TryParse(Console.ReadLine(), out optionchoise) || optionchoise < 0 || optionchoise > 7)
                    Console.WriteLine("Wrong input!");
                Console.WriteLine();

                int sentenceChoise;
                int wordLength;
                string? substring;
                switch (optionchoise)
                {
                    case 0:
                        return;
                    case 1:
                        text.PrintByWordsIncreasing();
                        break;
                    case 2:
                        text.PrintByLengthIncreasing();
                        break;
                    case 3:
                        Console.Write("Input word length: ");
                        while (!int.TryParse(Console.ReadLine(), out wordLength) || wordLength < 1)
                            Console.WriteLine("Wrong input!");

                        Console.WriteLine();
                        text.GetUnicWordsByLength(wordLength);
                        break;
                    case 4:
                        Console.Write("Input word length: ");
                        while (!int.TryParse(Console.ReadLine(), out wordLength) || wordLength < 1)
                            Console.WriteLine("Wrong input!");

                        text.DeleteWordsByLength(wordLength);
                        break;
                    case 5:
                        text.PrintSentences();
                        Console.WriteLine();

                        Console.Write("Choose sentence: ");
                        while (!int.TryParse(Console.ReadLine(), out sentenceChoise) || sentenceChoise < 1 || sentenceChoise > text.Sentences.Count)
                            Console.WriteLine("Wrong input!");

                        Console.Write("Input word length: ");
                        while (!int.TryParse(Console.ReadLine(), out wordLength) || wordLength < 1)
                            Console.WriteLine("Wrong input!");

                        Console.Write("Input substring: ");
                        substring = Console.ReadLine();
                        if (substring == null) substring = "";

                        Console.WriteLine();
                        text.ReplaceWordsByLength(sentenceChoise - 1, wordLength, substring);
                        break;
                    case 6:
                        text.DeleteStopWords();
                        break;
                    case 7:
                        text.GetConcordance();
                        break;
                    case 8:
                        text.SaveAsXML();
                        Console.WriteLine("Saved at Texts folder!");
                        break;
                }
            }
        }
    }
}