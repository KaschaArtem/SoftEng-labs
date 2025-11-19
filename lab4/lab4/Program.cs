public class Program
{
    public static void Main(string[] args)
    {
        DataParser parser = new DataParser();
        var aircraftsFromFile = parser.ParseAircraftsFromFile(Path.Combine("..", "..", "..", "data", "aircrafts.txt"));

        Airline airline = new Airline(aircraftsFromFile);

        do
        {
            Console.Write("Choose option:\n" +
                          "1. Output all aircrafts\n" +
                          "2. Output total load capacity\n" +
                          "3. Output total passengers capacity\n" +
                          "4. Sort by flight range\n" +
                          "5. Output by fuel consumption range\n" +
                          "a. Add aircraft\n" +
                          "r. Remove aircraft\n" +
                          "s. Serialize airline\n" +
                          "d. Deserialize airline\n" +
                          "e. Exit\n" +
                          ">>> ");

            char input = Console.ReadKey().KeyChar;
            if (input == 'e') { return; }
            else { Console.Write("\n\n\n"); }

            switch (input)
            {
                case '1':
                    foreach (var aircraft in airline.Aircrafts)
                        Console.WriteLine(aircraft);
                    break;

                case '2':
                    Console.WriteLine(airline.GetTotalLoadCapacity());
                    break;

                case '3':
                    Console.WriteLine(airline.GetTotalPassengersCapacity());
                    break;

                case '4':
                    airline.Sort(new SortByFlightRange());
                    Console.WriteLine("Sorted!");
                    break;

                case '5':
                    Console.Write("Enter min and max: ");
                    string[] parts = Console.ReadLine()!.Split();
                    if (parts.Length >= 2 &&
                        int.TryParse(parts[0], out int min) &&
                        int.TryParse(parts[1], out int max))
                    {
                        foreach (var aircraft in airline.GetByFuelConsumptionRange(min, max))
                            Console.WriteLine(aircraft);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!");
                    }
                    break;

                case 'a':
                    break;

                case 'r':
                    break;

                case 's':
                    airline.Serialize(Path.Combine("..", "..", "..", "data", "aircrafts.xml"));
                    Console.WriteLine("Saved!");
                    break;

                case 'd':
                    Airline loaded = Airline.Deserialize(Path.Combine("..", "..", "..", "data", "aircrafts.xml"));
                    Console.Write("Load xml?:\n" +
                                  "y. Yes\n" +
                                  "!y. No\n" +
                                  ">>> ");

                    char inputLoad = Console.ReadKey().KeyChar;
                    if (inputLoad == 'y') { airline = loaded; }
                    break;
            }

            Console.Write("\n\n");
        } while (true);
    }
}