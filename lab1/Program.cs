class Program
{
    struct Sequence
    {
        public string proteinName;
        public string organismName;
        public string proteinSequence;
    }

    struct Command
    {
        public string commandName;
        public string commandParameter1;
        public string commandParameter2;
    }

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

    static (List<Sequence> sequences, List<Command> commands) GetFilesContent((string sequencesFilePath, string commandsFilePath) filesPaths)
    {
        List<Sequence> sequences = new List<Sequence>();
        foreach (string line in File.ReadLines(filesPaths.Item1))
        {
            string[] tokens = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            var sequence = new Sequence
            {
                proteinName = tokens[0],
                organismName = tokens[1],
                proteinSequence = tokens[2]
            };
            sequences.Add(sequence);
        }

        List<Command> commands = new List<Command>();
        foreach (string line in File.ReadLines(filesPaths.Item2))
        {
            string[] tokens = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            var command = new Command();
            if (tokens[0] == "search" || tokens[0] == "mode") {
                command.commandName = tokens[0];
                command.commandParameter1 = tokens[1];
            } 
            else if (tokens[0] == "diff")
            {
                command.commandName = tokens[0];
                command.commandParameter1 = tokens[1];
                command.commandParameter2 = tokens[2];
            }
            commands.Add(command);
        }

        return (sequences, commands);
    }

    static void Main(string[] args)
    {
        var filesPaths = GetFilesDirectories();
        var pairOfSequencesCommands = GetFilesContent(filesPaths);

        foreach (Sequence sequence in pairOfSequencesCommands.Item1)
        {
            Console.WriteLine(sequence.proteinName + " | " + sequence.organismName + " | " + sequence.proteinSequence);
        }

        foreach (Command command in pairOfSequencesCommands.Item2)
        {
            Console.WriteLine(command.commandName + " | " + command.commandParameter1 + " | " + command.commandParameter2);
        }
    }
}