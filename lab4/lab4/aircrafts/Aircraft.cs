using System.Xml.Serialization;

[XmlInclude(typeof(CargoAircraft))]
[XmlInclude(typeof(PassengerAircraft))]
public abstract class Aircraft
{
    public enum Type
    {
        Cargo,
        Passenger
    }

    public string Model { get; set; } = string.Empty;
    public Type AircraftType { get; set; }
    public int FlightRange { get; set; }
    public float FuelConsumption { get; set; }

    public Aircraft() { }

    public Aircraft(string model, string aircraftType, int flightRange, float fuelConsumption)
    {
        Model = model;
        AircraftType = aircraftType switch
        {
            "cargo" => Type.Cargo,
            "passenger" => Type.Passenger,
            _ => throw new Exception("Unknown aircraft type")
        };
        FlightRange = flightRange;
        FuelConsumption = fuelConsumption;
    }

    protected void ReadBaseFields()
    {
        Console.Write("Enter model: ");
        Model = Console.ReadLine()!;

        Console.Write("Enter flight range: ");
        FlightRange = int.Parse(Console.ReadLine()!);

        Console.Write("Enter fuel consumption: ");
        FuelConsumption = float.Parse(Console.ReadLine()!);
    }

    public abstract Aircraft Create();

    public override string ToString()
    {
        return $"Model: {Model}, Type: {AircraftType}, Range: {FlightRange}, Fuel: {FuelConsumption}";
    }
}