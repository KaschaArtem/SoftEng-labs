using System.Reflection.Metadata.Ecma335;
using Business.Base;
using Business.Rules;

namespace Business.Entities;

public class Product : BusinessObject
{
    public string Name { get; set; } = String.Empty;
    private double calories100;
    public double Calories100
    {
        get { return calories100; }
        set
        {
            if (value < 0)
                calories100 = 0;
            else
                calories100 = value;
        }
    }
    private double weight;
    public double Weight
    {
        get { return weight; }
        set
        {
            if (value < 0)
                weight = 0;
            else
                weight = value;
        }
    }
    private double protein;
    public double Protein
    {
        get { return protein; }
        set
        {
            if (value < 0)
                protein = 0;
            else
                protein = value;
        }
    }
    private double fats;
    public double Fats
    {
        get { return fats; }
        set
        {
            if (value < 0)
                fats = 0;
            else
                fats = value;
        }
    }
    private double carbs;
    public double Carbs
    {
        get { return carbs; }
        set
        {
            if (value < 0)
                carbs = 0;
            else
                carbs = value;
        }
    }
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
