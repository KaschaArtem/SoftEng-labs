using System.Dynamic;

namespace lab2 
{
    class Program
    {
        static string? GetInputFilePath()
        {
            string contentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\content");
            string[] dataFiles = Directory.GetFiles(contentPath, "*.ChaseData.txt");

            if (!dataFiles.Any())
            {
                Console.WriteLine("No data files found!");
                return null;
            }

            Console.WriteLine("Choose data file:");
            for (int i = 0; i < dataFiles.Length; i++)
                Console.WriteLine($"{i + 1}) {Path.GetFileNameWithoutExtension(dataFiles[i])}");

            int fileChoice;
            while (!int.TryParse(Console.ReadLine(), out fileChoice) || fileChoice < 1 || fileChoice > dataFiles.Length)
                Console.WriteLine("Wrong input!");

            return dataFiles[fileChoice - 1];
        }

        static string SetOutputFilePath(string InputFile)
        {
            return Path.Combine(
                   Path.GetDirectoryName(InputFile)!,
                   Path.GetFileName(InputFile).Split('.')[0] + ".PursuitLog.txt"
                   );
        }

        static void Main(string[] args)
        {
            var InputFile = GetInputFilePath();
            if (InputFile == null) return;
            var OutputFile = SetOutputFilePath(InputFile);
        }
    }
}