using Cart.Products;
using System;
using System.Reflection;
using System.Text.Json;

namespace Cart;

public class Store
{
    public List<Product> Products { get; set; }

    public Store()
    {
        Products = new();
    }

    public List<Product> GenerateProducts()
    {
        ulong productId = 0;
        Random random = new();

        for (int i = 0; i < 3; i++)
        {
            Corvalol corvarol = new(
                id: productId += 1,
                name: "Корвалол-" + productId.ToString(),
                weight: random.NextDouble() * (30 - 15) + 15,
                price: new decimal(random.NextDouble() * (1000 - 500) + 500),
                timeOfArrival: DateTime.Now.AddDays(random.Next(0, 10))
                );
            WashingMachine washingMachine = new(
                id: productId += 1,
                name: "Стиральная машина-" + productId.ToString(),
                weight: random.NextDouble() * (30 - 15) + 15,
                price: new decimal(random.NextDouble() * (1000 - 500) + 500),
                timeOfArrival: DateTime.Now.AddDays(random.Next(0, 10)),
                isDryerIncluded: random.Next(0, 2) == 0 ? false : true
                );
            Chips chips = new(
                id: productId += 1,
                name: "Чипсы-" + productId.ToString(),
                weight: random.NextDouble() * (30 - 15) + 15,
                price: new decimal(random.NextDouble() * (1000 - 500) + 500),
                timeOfArrival: DateTime.Now.AddDays(random.Next(0, 10))
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
                    string JSONString = JsonSerializer.Serialize(Products);
                    string fileName = "Products list.json";
                    File.WriteAllText(fileName, JSONString);
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
}
