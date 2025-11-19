public class DataParser
{
    public List<Aircraft> ParseAircraftsFromFile(string path)
    {
        var result = new List<Aircraft>();
        foreach (var line in File.ReadAllLines(path))
        {
            var tokens = line.Split(';');

            string model = tokens[0];
            string type = tokens[1].ToLower();
            int range = int.Parse(tokens[2]);
            float fuel = float.Parse(tokens[3]);
            int capacity = int.Parse(tokens[4]);

            switch (type)
            {
                case "cargo":
                    result.Add(new CargoAircraft(model, range, fuel, capacity));
                    break;
                case "passenger":
                    result.Add(new PassengerAircraft(model, range, fuel, capacity));
                    break;
            }
        }
        return result;
    }
}