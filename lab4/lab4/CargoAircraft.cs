public class CargoAircraft : Aircraft
{
    public int LoadCapacity { get; set; }

    public override string ToString()
    {
        return base.ToString() + $", Load: {LoadCapacity}";
    }
}