public class PassengerAircraft : Aircraft
{
    public int PassengerCapacity { get; set; }

    public PassengerAircraft() { }

    public PassengerAircraft(string model, string aircraftType, int flightRange, float fuelConsumption, int passengerCapacity)
        : base(model, aircraftType, flightRange, fuelConsumption)
    {
        PassengerCapacity = passengerCapacity;
    }

    public override Aircraft Create()
    {
        ReadBaseFields();

        Console.Write("Enter passenger capacity: ");
        PassengerCapacity = int.Parse(Console.ReadLine()!);

        return this;
    }

    public override string ToString()
    {
        return base.ToString() + $", Passengers: {PassengerCapacity}";
    }
}