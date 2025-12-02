using Business.Base;
using Business.Rules;

namespace Business.Entities;

public class Product : BusinessObject
{
    public string Name { get; set; } = String.Empty;
    public double Calories100 { get; set; }
    public double Weight { get; set; }
    public double Protein { get; set; }
    public double Fats { get; set; }
    public double Carbs { get; set; }
    public Category Category { get; set; } = new Category();

    public double Calories
    {
        get
        {
            if (Weight != 100.0) 
                return (Weight * Calories100) / 100;
            return Calories100;
        }
    }

    public Product() { }

    public Product(Product product)
    {
        Name = product.Name;
        Weight = product.Weight;
        Protein = product.Protein;
        Fats = product.Fats;
        Carbs = product.Carbs;
        Calories100 = product.Calories100;
        Category = product.Category;

        AddRule(new NameRule(nameof(Name)));
    }

    public override string ToString()
    {
        return $"{Name} {Protein} {Fats} {Carbs} {Calories}";
    }
}
