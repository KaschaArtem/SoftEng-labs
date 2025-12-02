using Business.Base;

namespace Business.Entities;

public class DailyRation : BusinessObject
{
    public int MealAmount { get; set; }
    public Dictionary<string, MealTime> MealTimes { get; set; }

    public DailyRation(int n = 3)
    {
        MealAmount = n;
        MealTimes = new Dictionary<string, MealTime>();
        MealTimes["Завтрак"] = new MealTime("Завтрак");
        MealTimes["Обед"] = new MealTime("Обед");
        MealTimes["Ужин"] = new MealTime("Ужин");
    }

    public double GetCalories()
    {
        double calories = 0;
        foreach (string s in MealTimes.Keys)
            foreach (Product p in MealTimes[s].Meal)
                calories += p.Calories;
        return calories;
    }
}
