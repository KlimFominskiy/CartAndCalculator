using Cart.Products;
using System;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cart;

public static class Store
{
    public static List<Product> GenerateProducts()
    {
        List<Product> Products = new();

        ulong productId = 0;
        Random random = new();

        for (int i = 0; i < 5; i++)
        {
            Corvalol corvarol = new(
                id: productId += 1,
                name: "Корвалол-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId),
                timeOfArrival: GetTimeOfArrival(productId)
                );
            WashingMachine washingMachine = new(
                id: productId += 1,
                name: "Стиральная машина-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId),
                timeOfArrival: GetTimeOfArrival(productId),
                isDryerIncluded: random.Next(0, 2) == 0 ? false : true
                );
            Chips chips = new(
                id: productId += 1,
                name: "Чипсы-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId),
                timeOfArrival: GetTimeOfArrival(productId)
                );

            Products.Add(corvarol);
            Products.Add(washingMachine);
            Products.Add(chips);
        }

        while(true)
        { 
            Console.WriteLine("Сохранить список в файл? Введите y или n.");
            switch(Console.ReadLine())
            {
                case "y":
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    };
                    string JSONString = JsonSerializer.Serialize(Products, options);
                    string fileName = "Products.json";
                    string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    File.AppendAllText(filePath + Path.DirectorySeparatorChar + fileName, JSONString);
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("Такой команды нет.");
                    continue;
            }

            Console.WriteLine("Вывести список на экран? Введите y или n.");
            switch (Console.ReadLine())
            {
                case "y":
                    foreach (Product product in Products)
                    {
                        PropertyInfo[] propertyInfo = product.GetType().GetProperties();
                        foreach (PropertyInfo property in propertyInfo)
                        {
                            Console.WriteLine($"{property.Name}, {property.GetValue(product)?.ToString()}");
                        }
                        Console.WriteLine();
                    }
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("Такой команды нет.");
                    continue;
            }
            break;
        }

        return Products;
    }

    private static double GetWeight(ulong ProductId)
    {
        Random random = new();
        return Double.Round(random.NextDouble() * (30 - 15) + 15, 2);
    }

    private static decimal GetPrice(ulong ProductId)
    {
        Random random = new();
        
        return Decimal.Round((decimal)(random.NextDouble() * (1000 - 500) + 500), 2);
    }

    private static DateTime GetTimeOfArrival(ulong ProductId)
    {
        Random random = new();
        return DateTime.Now.AddDays(random.Next(0, 10));
    }
}
