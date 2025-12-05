using Business.Entities;
using Service;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Presentation.Views;

public partial class MainWindow : Window
{
    private readonly IService service = new Service.Service();

    private User user = new User();
    private DailyRation ration;

    private Product? selectedProduct;
    private MealTime? selectedMealTime;

    public MainWindow()
    {
        InitializeComponent();
        ration = service.GetRation();

        UpdateUserInfo();
        LoadCategories();
        LoadMealTimes();
        HookEvents();
    }

    private void UpdateUserInfo()
    {
        UserWeightBox.Text = user.Weight.ToString();
        UserHeightBox.Text = user.Height.ToString();
        UserAgeBox.Text = user.Age.ToString();

        UpdateBmrBox();

        switch (user.Activity)
        {
            case ActivityType.Low: RadioLow.IsChecked = true; break;
            case ActivityType.Normal: RadioNormal.IsChecked = true; break;
            case ActivityType.Average: RadionAverage.IsChecked = true; break;
            case ActivityType.High: RadioHigh.IsChecked = true; break;
        }

        UpdateArmBox();

        UpdateDailyNorm();
    }

    private void LoadCategories()
    {
        foreach (Category category in service.GetCategories())
        {
            var categoryNode = new TreeViewItem { Header = category.Name };

            var products = service.GetProductsByCategory(category.Name);
            foreach (Product product in products)
            {
                var productNode = new TreeViewItem { Header = product.Name };

                ((IList)categoryNode.Items).Add(productNode);
            }

            ((IList)ProductCategoryTree.Items).Add(categoryNode);
        }
    }

    private void LoadMealTimes()
    {
        foreach (string mealTime in ration.MealTimes.Keys)
        {
            var mealTimeNode = new TreeViewItem { Header = mealTime };

            ((IList)ProductMealTimeTree.Items).Add(mealTimeNode);
        }
    }

    private void HookEvents()
    {
        UserWeightBox.TextChanged += OnUserWeightChanged;
        UserHeightBox.TextChanged += OnUserHeightChanged;
        UserAgeBox.TextChanged += OnUserAgeChanged;

        RadioLow.IsCheckedChanged += OnRadioChanged;
        RadioNormal.IsCheckedChanged += OnRadioChanged;
        RadionAverage.IsCheckedChanged += OnRadioChanged;
        RadioHigh.IsCheckedChanged += OnRadioChanged;
    }

    private void OnUserWeightChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            string text = tb.Text ?? "";

            if (double.TryParse(text, out double weight))
                user.Weight = weight;
            else
                user.Weight = 0;
            UpdateBmrBox();
        }
    }

    private void OnUserHeightChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            string text = tb.Text ?? "";

            if (double.TryParse(text, out double height))
                user.Height = height;
            else
                user.Height = 0;
            UpdateBmrBox();
        }
    }

    private void OnUserAgeChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            string text = tb.Text ?? "";

            if (int.TryParse(text, out int age))
                user.Age = age;
            else
                user.Age = 0;
            UpdateBmrBox();
        }
    }

    private void UpdateBmrBox()
    {
        double? bmr = user.GetBMR();
        if (bmr == null)
            BmrBox.Text = "";
        else
            BmrBox.Text = ((int)bmr).ToString();
        UpdateDailyNorm();
    }

    private void OnRadioChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb && rb.IsChecked == true)
        {
            user.Activity = rb.Content!.ToString() switch
            {
                "Сидячий образ жизни" => ActivityType.Low,
                "Умеренная активность" => ActivityType.Normal,
                "Средняя активность" => ActivityType.Average,
                "Высокая активность" => ActivityType.High,
                _ => user.Activity
            };
            UpdateArmBox();
        }
    }

    private void UpdateArmBox()
    {
        double? arm = user.GetARM();
        if (arm == null)
            ArmBox.Text = "";
        else
            ArmBox.Text = Math.Round(arm.Value, 3).ToString();
        UpdateDailyNorm();
    }

    private void UpdateDailyNorm()
    {
        int? calories = user.GetDailyCalories();
        if (calories == null)
        {
            NormalCaloriesBox.Text = "";
            NormalProteinBox.Text = "";
            NormalFatsBox.Text = "";
            NormalCarbsBox.Text = "";
        }
        else
        {
            NormalCaloriesBox.Text = calories.ToString();
            NormalProteinBox.Text = user.GetDailyProtein(calories.Value).ToString();
            NormalFatsBox.Text = user.GetDailyFats(calories.Value).ToString();
            NormalCarbsBox.Text = user.GetDailyCarbs(calories.Value).ToString();
        }
    }
}