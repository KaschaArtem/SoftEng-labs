public class CargoAircraft : Aircraft
{
    public int LoadCapacity { get; set; }

    public CargoAircraft() { }

    public CargoAircraft(string model, string aircraftType, int flightRange, float fuelConsumption, int loadCapacity)
        : base(model, aircraftType, flightRange, fuelConsumption)
    {
        LoadCapacity = loadCapacity;
    }

    public override string ToString()
    {
        return base.ToString() + $", Load: {LoadCapacity}";
    }
}