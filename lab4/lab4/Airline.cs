using System.Xml.Serialization;

public class Airline
{
    public List<Aircraft> Aircrafts { get; set; } = new();

    public Airline() { }

    public Airline(List<Aircraft> aircrafts)
    {
        Aircrafts = aircrafts;
    }

    public void AddAircraft(Aircraft aircraft)
    {
        Aircrafts.Add(aircraft);
    }

    public void RemoveAircraft(Aircraft aircraft)
    {
        Aircrafts.Remove(aircraft);
    }

    public int GetTotalLoadCapacity()
    {
        return Aircrafts.OfType<CargoAircraft>().Sum(c => c.LoadCapacity);
    }

    public int GetTotalPassengersCapacity()
    {
        return Aircrafts.OfType<PassengerAircraft>().Sum(c => c.PassengerCapacity);
    }

    public void SortByFlightRange()
    {
        Aircrafts = Aircrafts.OrderBy(r => r.FlightRange).ToList();
    }

    public List<Aircraft> GetByFuelConsumptionRange(float min, float max)
    {
        return Aircrafts.Where(c => c.FuelConsumption >= min && c.FuelConsumption <= max).ToList();
    }

    public void Serialize(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Airline));

        using FileStream fileStream = new FileStream(filePath, FileMode.Create);
        serializer.Serialize(fileStream, this);
    }

    public static Airline Deserialize(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Airline));

        using FileStream fileStream = new FileStream(filePath, FileMode.Open);
        return (Airline)serializer.Deserialize(fileStream)!;
    }
}