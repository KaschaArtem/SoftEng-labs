public class CargoAircraft : Aircraft
{
    public int LoadCapacity { get; set; }

    public CargoAircraft() { }

    public CargoAircraft(string model, int flightRange, float fuelConsumption, int loadCapacity)
        : base(model, flightRange, fuelConsumption)
    {
        LoadCapacity = loadCapacity;
    }

    public override string ToString()
    {
        return base.ToString() + $", Load: {LoadCapacity}";
    }
}