class Program
{   

    static (string sequencesFilePath, string commandsFilePath) GetFilesDirectories()
    {
        string sequencesFilePath = "";
        string commandsFilePath = "";

        string contentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\content");
        string[] sequencesfiles = Directory.GetFiles(contentPath, "sequences*.txt");

        if (sequencesfiles.Length > 0)
        {
            Console.WriteLine("Choose sequences file:");
            for (int i = 0; i < sequencesfiles.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + Path.GetFileNameWithoutExtension(sequencesfiles[i]));
            }

            int fileChoise;
            while (true)
            {
                Console.Write(">>> ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out fileChoise) &&
                    fileChoise >= 1 && fileChoise <= sequencesfiles.Length)
                {
                    break;
                }

                Console.WriteLine("Wrong input!");
            }

            sequencesFilePath = sequencesfiles[fileChoise - 1];
            string filesID = Path.GetFileName(sequencesFilePath).Substring("sequences".Length);
            commandsFilePath = Path.Combine(contentPath, "commands" + filesID);

            if (!File.Exists(commandsFilePath))
            {
                Console.WriteLine("No commands file for this sequences file!");
                commandsFilePath = "";
            }

        }
        else
        {
            Console.WriteLine("Not found any sequences files");
        }

        return (sequencesFilePath, commandsFilePath);
    }

    static void Main(string[] args)
    {
        var filesPaths = GetFilesDirectories();
    }
}