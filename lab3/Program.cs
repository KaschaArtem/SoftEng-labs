namespace lab3
{
    class Program
    {
        static string? GetTextFilePath()
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
            if (inputFilePath == null) { return; }

            TextParser textParser = new TextParser();
            Text text = textParser.ParseTextFile(inputFilePath);

            text.Print();
            Console.WriteLine("\n\n\n");
            text.ReplaceWordsByLength(0, 5, "asdasdasd");
        }
    }
}