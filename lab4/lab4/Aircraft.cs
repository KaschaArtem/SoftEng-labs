using System.Reflection;

public abstract class Aircraft
{
    public string Model { get; set; } = string.Empty;
    public int FlightRange { get; set; }
    public float FuelConsumption { get; set; }

    public override string ToString()
    {
        return $"Model: {Model}, Range: {FlightRange}, Fuel: {FuelConsumption}";
    }
}