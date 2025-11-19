public class PassengerAircraft : Aircraft
{
    public int PassengerCapacity { get; set; }

    public PassengerAircraft() { }

    public PassengerAircraft(string model, string aircraftType, int flightRange, float fuelConsumption, int passengerCapacity)
        : base(model, aircraftType, flightRange, fuelConsumption)
    {
        PassengerCapacity = passengerCapacity;
    }

    public override string ToString()
    {
        return base.ToString() + $", Passengers: {PassengerCapacity}";
    }
}