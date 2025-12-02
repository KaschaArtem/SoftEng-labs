using Business.Base;

namespace Business.Entities;

public enum ActivityType
{
    Low,
    Normal,
    Average,
    High
}

public class User : BusinessObject
{
    public double Weight { get; set; }
    public double Height { get; set; }
    public int Age { get; set; }
    public ActivityType Activity { get; set; }

    public User(double weight = 75, double height = 170, int age = 30, ActivityType activity = ActivityType.Normal)
    {
        Weight = weight;
        Height = height;
        Age = age;
        Activity = activity;
    }

    public double GetBMR()
    {
        return 447.593 + 9.247 * Weight + 3.098 * Height - 4.330 * Age;
    }
    
    public double GetARM()
    {
        switch (Activity)
        {
            case ActivityType.Low: return 1.2; 
            case ActivityType.Normal: return 1.375;
            case ActivityType.Average: return 1.55;
            case ActivityType.High: return 1.725;
            default: return 1.375;
        }
    }

    public int GetDailyCalories()
    {
        return (int)(GetBMR() * GetARM());
    }
}
