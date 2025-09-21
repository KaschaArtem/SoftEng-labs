class Program
{   
    struct Sequance
    {
        string proteinName;
        string organismName;
        string proteinSequance;
    }

    static string GetFileDirectory()
    {

        return "";
    }

    static Sequance GetSequencesFileContent(string filePath)
    {
        Sequance sequance = new Sequance();

        return sequance;
    }

    static void Main(string[] args)
    {
        string contentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\content");
        string[] files = Directory.GetFiles(contentPath, "*.txt");

        foreach (string file in files)
        {
            Console.WriteLine(file);
        }

        Console.ReadLine();
    }
}