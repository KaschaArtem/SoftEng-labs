using System.Text;

class Program
{
    public static string contentPath = "";
    public static string filesID = "";
    public static int commandID = 1;
    public static StringBuilder fileOutput = new StringBuilder();

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

        contentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\content");
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
            filesID = Path.GetFileName(sequencesFilePath).Substring("sequences".Length);
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

    static string DecodeRLESequence(string proteinSequence)
    {
        var result = new StringBuilder();
        for (int i = 0; i < proteinSequence.Length; i++)
        {
            if (char.IsDigit(proteinSequence[i]))
            {
                int num = proteinSequence[i] - '0';

                result.Append(proteinSequence[i + 1], (num - 1));
            } 
            else
            {
                result.Append(proteinSequence[i]);
            }
        }

        return result.ToString();
    }

    static (List<Sequence> sequences, List<Command> commands) GetFilesContent((string sequencesFilePath, string commandsFilePath) filesPaths)
    {
        List<Sequence> sequences = new List<Sequence>();
        foreach (string line in File.ReadLines(filesPaths.sequencesFilePath))
        {
            string[] tokens = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            var sequence = new Sequence
            {
                proteinName = tokens[0],
                organismName = tokens[1],
                proteinSequence = DecodeRLESequence(tokens[2])
            };
            sequences.Add(sequence);
        }

        List<Command> commands = new List<Command>();
        foreach (string line in File.ReadLines(filesPaths.commandsFilePath))
        {
            string[] tokens = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            var command = new Command();

            command.commandName = tokens[0];
            switch (command.commandName)
            {
                case "search":
                    command.commandParameter1 = DecodeRLESequence(tokens[1]);
                    break;
                case "diff": 
                    command.commandParameter1 = tokens[1];
                    command.commandParameter2 = tokens[2];
                    break;
                case "mode": 
                    command.commandParameter1 = tokens[1];
                    break;
            }
            commands.Add(command);
        }

        return (sequences, commands);
    }

    static void AddCommandID(Command command)
    {
        switch(command.commandName)
        {
            case "search": fileOutput.Append($"{commandID:D3}   {command.commandName}   {command.commandParameter1}\n"); break;
            case "diff": fileOutput.Append($"{commandID:D3}   {command.commandName}   {command.commandParameter1}   {command.commandParameter2}\n"); break;
            case "mode": fileOutput.Append($"{commandID:D3}   {command.commandName}   {command.commandParameter1}\n"); break;
        }
        commandID++;
    }

    static void ExecutionOfCommands((List<Sequence> sequences, List<Command> commands) pairOfSequencesCommands)
    {
        foreach (var command in pairOfSequencesCommands.commands)
        {
            AddCommandID(command);
            switch (command.commandName)
            {
                case "search":

                    var matchesForProteinSequence = pairOfSequencesCommands.sequences
                        .Where(sequence => sequence.proteinSequence.Contains(command.commandParameter1));

                    if (matchesForProteinSequence.Any())
                    {
                        foreach (var sequence in matchesForProteinSequence)
                        {
                            fileOutput.Append($"organism\t\t\t\tprotein\n{sequence.organismName}\t\t{sequence.proteinName}\n");
                        }
                    }
                    else
                    {
                        fileOutput.Append($"organism\t\t\t\tprotein\nNOT FOUND\n");
                    }

                    break;
                case "diff":

                    fileOutput.Append("amino-acids difference:\n");

                    var matchForFirstProteinName = pairOfSequencesCommands.sequences
                        .FirstOrDefault(sequence => sequence.proteinName.Contains(command.commandParameter1));

                    var matchForSecondProteinName = pairOfSequencesCommands.sequences
                        .FirstOrDefault(sequence => sequence.proteinName.Contains(command.commandParameter2));

                    if (matchForFirstProteinName.proteinName != null && matchForSecondProteinName.proteinName != null)
                    {
                        var longerSequence = matchForFirstProteinName;
                        var shorterSequence = matchForSecondProteinName;
                        if (longerSequence.proteinSequence.Length < matchForSecondProteinName.proteinSequence.Length)
                        {
                            longerSequence = matchForSecondProteinName;
                            shorterSequence = matchForFirstProteinName;
                        }

                        int replaceAmount = 0;
                        for (int i = 0; i < shorterSequence.proteinSequence.Length; i++)
                        {
                            if (shorterSequence.proteinSequence[i] != longerSequence.proteinSequence[i])
                            {
                                replaceAmount++;
                            }
                        }

                        replaceAmount += (longerSequence.proteinSequence.Length - shorterSequence.proteinSequence.Length);

                        fileOutput.Append($"{replaceAmount}\n");
                    }
                    else
                    {
                        if (matchForFirstProteinName.proteinName == null)
                        {
                            fileOutput.Append($"MISSING:\t{matchForFirstProteinName.proteinName}\n");
                        }
                        else if (matchForSecondProteinName.proteinName == null)
                        {
                            fileOutput.Append($"MISSING:\t{matchForSecondProteinName.proteinName}\n");
                        }
                        else
                        {
                            fileOutput.Append($"MISSING:\t{matchForFirstProteinName.proteinName}\t{matchForSecondProteinName.proteinName}\n");
                        }
                    }

                    break;
                case "mode":

                    fileOutput.Append("amino-acid occurs:\n");

                    var matchForProteinName = pairOfSequencesCommands.sequences
                        .FirstOrDefault(sequence => sequence.proteinName.Contains(command.commandParameter1));

                    if (matchForProteinName.proteinName != null)
                    {
                        var result = matchForProteinName.proteinSequence
                            .GroupBy(character => character)
                            .Select(group => new { Key = group.Key, Count = group.Count() })
                            .OrderByDescending(pair => pair.Count)
                            .ThenBy(pair => pair.Key)
                            .First();

                        fileOutput.Append($"{result.Key}          {result.Count}\n");
                    }
                    else
                    {
                        fileOutput.Append($"MISSING: {command.commandParameter1}");
                    }

                    break;
            }
            fileOutput.Append('-', 74);
            fileOutput.Append('\n');
        }
    }

    static void Main(string[] args)
    {
        var filesPaths = GetFilesDirectories();
        var pairOfSequencesCommands = GetFilesContent(filesPaths);

        fileOutput.Append("Kascha Artem\n" + "Genetic Searching\n" + new string('-', 74) + "\n");

        ExecutionOfCommands(pairOfSequencesCommands);

        File.WriteAllText(Path.Combine(contentPath, $"genedata{filesID}"), fileOutput.ToString());
        Console.WriteLine("File Saved!");
    }
}