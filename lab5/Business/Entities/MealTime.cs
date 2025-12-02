using Business.Base;
using Business.Rules;

namespace Business.Entities;

public class MealTime : BusinessObject
{
    public string Name { get; set; }
    public List<Product> Meal { get; set; }

    public MealTime(string name)
    {
        Name = name;
        Meal = new List<Product>();

        AddRule(new NameRule(nameof(Name)));
    }

    public double GetCalories()
    {
        double calories = 0;
        foreach (Product product in Meal)
            calories += product.Calories;
        return calories;
    }
}
