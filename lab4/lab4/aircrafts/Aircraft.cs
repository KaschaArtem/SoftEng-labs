public abstract class Aircraft
{
    public string Model { get; set; } = string.Empty;
    public int FlightRange { get; set; }
    public float FuelConsumption { get; set; }

    public Aircraft() { }

    public Aircraft(string model, int flightRange, float fuelConsumption)
    {
        Model = model;
        FlightRange = flightRange;
        FuelConsumption = fuelConsumption;
    }

    public override string ToString()
    {
        return $"Model: {Model}, Range: {FlightRange}, Fuel: {FuelConsumption}";
    }
}