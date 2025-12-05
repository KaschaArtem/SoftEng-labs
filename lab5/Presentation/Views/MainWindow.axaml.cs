using Business.Entities;
using Service;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Presentation.Views;

public partial class MainWindow : Window
{
    private readonly IService service = new Service.Service();

    private User user = new User();
    private DailyRation ration;

    private Product? selectedProduct;
    private MealTime? selectedMealTime;

    private Dictionary<string, TreeViewItem> mealTimeTrees = new Dictionary<string, TreeViewItem>();

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
                categoryNode.Items.Add(productNode);
            }

            ProductCategoryTree.Items.Add(categoryNode);
        }
    }

    private void LoadMealTimes()
    {
        foreach (var kvp in ration.MealTimes)
        {
            var mealTimeNode = new TreeViewItem { Header = kvp.Key };
            mealTimeTrees.Add(kvp.Key, mealTimeNode);

            foreach (var product in kvp.Value.Meal)
            {
                var productNode = new TreeViewItem { Header = product.Name };
                mealTimeNode.Items.Add(productNode);
            }

            ProductMealTimeTree.Items.Add(mealTimeNode);
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

        ProductCategorySearch.TextChanged += OnProductCategorySearchChanged;
        ProductMealTimeSearch.TextChanged += OnProductMealTimeSearchChanged;

        ProductCategoryTree.PointerReleased += OnProductTreeRightClick;
        ProductMealTimeTree.PointerReleased += OnMealProductTreeRightClick;
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

    private void OnProductCategorySearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox searchBox)
        {
            string searchText = searchBox.Text ?? "";

            foreach (TreeViewItem categoryNode in ProductCategoryTree.Items!)
            {
                categoryNode.IsVisible = true;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (TreeViewItem productNode in categoryNode.Items!)
                    {
                        productNode.IsVisible = true;
                    }
                    categoryNode.IsExpanded = false;
                    continue;
                }

                searchText = searchText.ToLower();
                bool hasVisibleProducts = false;

                foreach (TreeViewItem productNode in categoryNode.Items!)
                {
                    bool isVisible = false;

                    if (productNode.Header!.ToString()!.ToLower().Contains(searchText))
                        isVisible = true;

                    productNode.IsVisible = isVisible;

                    if (isVisible)
                        hasVisibleProducts = true;
                }

                categoryNode.IsVisible = hasVisibleProducts;

                categoryNode.IsExpanded = hasVisibleProducts;
            }
        }
    }

    private void OnProductMealTimeSearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox searchBox)
        {
            string searchText = searchBox.Text ?? "";

            foreach (TreeViewItem mealTimeNode in ProductMealTimeTree.Items!)
            {
                mealTimeNode.IsVisible = true;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (TreeViewItem productNode in mealTimeNode.Items!)
                    {
                        productNode.IsVisible = true;
                    }
                    continue;
                }

                searchText = searchText.ToLower();
                bool hasVisibleProducts = false;

                foreach (TreeViewItem productNode in mealTimeNode.Items!)
                {
                    bool isVisible = false;

                    if (productNode.Header!.ToString()!.ToLower().Contains(searchText))
                        isVisible = true;

                    productNode.IsVisible = isVisible;

                    if (isVisible)
                        hasVisibleProducts = true;
                }

                mealTimeNode.IsVisible = hasVisibleProducts;

                mealTimeNode.IsExpanded = hasVisibleProducts;
            }
        }
    }

    private void OnProductTreeRightClick(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton != MouseButton.Right)
            return;

        if (ProductCategoryTree.SelectedItem is not TreeViewItem item)
            return;

        if (item.Items.Count > 0)
            return;

        string productName = item.Header!.ToString()!;

        Product product = service.GetProduct(productName);

        var menu = new ContextMenu();

        foreach (var kvp in ration.MealTimes)
        {
            var menuItem = new MenuItem { Header = $"Добавить в {kvp.Key}" };
            menuItem.Click += (_, __) =>
            {
                kvp.Value.AddProduct(product);
                AddProductToMealTimeInfo(kvp.Key, productName);
            };
            menu.Items.Add(menuItem);
        }

        menu.Open(item);
    }

    private void OnMealProductTreeRightClick(object? sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton != MouseButton.Right)
            return;

        if (ProductMealTimeTree.SelectedItem is not TreeViewItem item)
            return;

        if (item.Parent is not TreeViewItem mealItem)
            return;

        string productName = item.Header!.ToString()!;
        string currentMeal = mealItem.Header!.ToString()!;

        Product product = service.GetProduct(productName);

        var menu = new ContextMenu();

        var moveMenu = new MenuItem { Header = "Переместить в..." };

        foreach (var kvp in ration.MealTimes)
        {
            if (kvp.Key == currentMeal)
                continue;

            var menuItem = new MenuItem { Header = kvp.Key };
            menuItem.Click += (_, __) =>
            {
                ration.MealTimes[currentMeal].RemoveProduct(product);
                kvp.Value.AddProduct(product);
                RemoveProductFromMealTimeInfo(currentMeal, productName);
                AddProductToMealTimeInfo(kvp.Key, productName);

            };
            moveMenu.Items.Add(menuItem);
        }

        var delete = new MenuItem { Header = "Удалить" };
        delete.Click += (_, __) =>
        {
            ration.MealTimes[currentMeal].RemoveProduct(product);
            RemoveProductFromMealTimeInfo(currentMeal, productName);
        };

        menu.Items.Add(moveMenu);
        menu.Items.Add(delete);

        menu.Open(item);
    }

    private void AddProductToMealTimeInfo(string key, string productName)
    {
        if (mealTimeTrees.TryGetValue(key, out var mealTimeNode))
        {
            var productNode = new TreeViewItem { Header = productName };
            mealTimeNode.Items.Add(productNode);
        }
    }

    private void RemoveProductFromMealTimeInfo(string key, string productName)
    {
        if (mealTimeTrees.TryGetValue(key, out var mealTimeNode))
        {
            TreeViewItem? nodeToRemove = null;
            foreach (TreeViewItem item in mealTimeNode.Items!)
            {
                if (item.Header?.ToString() == productName)
                {
                    nodeToRemove = item;
                    break;
                }
            }

            if (nodeToRemove != null)
            {
                mealTimeNode.Items.Remove(nodeToRemove);
            }
        }
    }
}