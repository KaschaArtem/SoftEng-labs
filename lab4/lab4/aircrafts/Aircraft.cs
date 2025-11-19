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
            _ => throw new NotImplementedException()
        };
        FlightRange = flightRange;
        FuelConsumption = fuelConsumption;
    }

    public override string ToString()
    {
        return $"Model: {Model}, Type: {AircraftType}, Range: {FlightRange}, Fuel: {FuelConsumption}";
    }
}