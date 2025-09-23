using System.Text;

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

    static void ExecutionOfCommands((List<Sequence> sequences, List<Command> commands) pairOfSequencesCommands)
    {
        foreach (var command in pairOfSequencesCommands.commands)
        {
            switch (command.commandName)
            {
                case "search":

                    var matchesForProteinSequence = pairOfSequencesCommands.sequences
                        .Where(sequence => sequence.proteinSequence.Contains(command.commandParameter1));
                    
                    if (matchesForProteinSequence.Any())
                    {
                        foreach(var sequence in matchesForProteinSequence)
                        {
                            Console.WriteLine(command.commandName + " | " + command.commandParameter1 + " | " + sequence.proteinSequence);
                        }
                    } 
                    else
                    {
                        Console.WriteLine(command.commandName + " | " + command.commandParameter1 + " | NOT FOUND");
                    }

                    break;
                case "diff":

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

                        Console.WriteLine(command.commandName + " | " + replaceAmount);
                    }
                    else
                    {
                        Console.WriteLine(command.commandName + " | NOT FOUND");
                    }

                    break;
                case "mode":

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

                        Console.WriteLine(command.commandName + " | " + matchForProteinName.proteinName + " | " + result.Key + " | " + result.Count);
                    }
                    else
                    {
                        Console.WriteLine(command.commandName + " | MISSING");
                    }

                    break;
            }
        }
    }

    static void Main(string[] args)
    {
        var filesPaths = GetFilesDirectories();
        var pairOfSequencesCommands = GetFilesContent(filesPaths);

        ExecutionOfCommands(pairOfSequencesCommands);
    }
}