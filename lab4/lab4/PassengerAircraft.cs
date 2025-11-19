public class PassengerAircraft : Aircraft
{
    public int PassengerCapacity { get; set; }

    public override string ToString()
    {
        return base.ToString() + $", Passengers: {PassengerCapacity}";
    }
}