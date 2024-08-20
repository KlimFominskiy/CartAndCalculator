using Cart.Orders;
using Cart.Products;
using Cart.Settings;
using System.Text.Json;

namespace Cart.Stores;

/// <summary>
/// Класс магазина. Может генерировать продукты или считывать список продуктов из файла. Хранит список продуктов. 
/// </summary>
public static class Store
{
    /// <summary>
    /// Список продуктов в магазине.
    /// </summary>
    public static List<Product> Products = new();

    /// <summary>
    /// Типы товаров магазина.
    /// </summary>
    public static List<Type> ProductsTypes = typeof(Product).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Product))).ToList();

    /// <summary>
    /// Сгенерировать продукты.
    /// </summary>
    public static List<Product> GenerateProducts()
    {
        uint productId = 0;
        Random random = new();

        for (uint i = 0; i < 5; i++)
        {
            Corvalol corvarol = new(
                id: productId += 1,
                name: "Корвалол-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId)
                );
            WashingMachine washingMachine = new(
                id: productId += 1,
                name: "Стиральная машина-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId),
                isDryerIncluded: random.Next(0, 2) == 0 ? false : true
                );
            Chips chips = new(
                id: productId += 1,
                name: "Чипсы-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId)
                );

            Products.Add(corvarol);
            Products.Add(washingMachine);
            Products.Add(chips);
        }

        while (true)
        {
            Console.WriteLine("Сохранить список в файл? Введите y или n.");
            switch (Console.ReadLine())
            {
                case "y":
                    string jsonString = JsonSerializer.Serialize(Products, ProgramSettings.JsonSerializerOptions);
                    string fileName = "Products.json";
                    string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                    File.WriteAllText(filePath + Path.DirectorySeparatorChar + fileName, jsonString);
                    break;
                case "n":
                    break;
                default:
                    Console.WriteLine("Такой команды нет.");
                    continue;
            }

            Console.WriteLine("Вывести список товаров на экран? Введите y или n.");
            switch (Console.ReadLine())
            {
                case "y":
                    PrintProductsInfo();
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

    /// <summary>
    /// Считать из файла список товаров магазина.
    /// </summary>
    public static void ReadProductsFromFile()
    {
        string fullPathToFile;
        while (true)
        {
            fullPathToFile = ReadTypesFromConsole.ReadFullFileNameFromConsole(ProgramSettings.productsFileNameDefault);
            if (!File.Exists(fullPathToFile))
            {
                Console.WriteLine($"Считан путь: {fullPathToFile}.\n" +
                    $"Файл не найден. Повторите ввод.");

                continue;
            }

            break;
        }

        string jsonProducts = File.ReadAllText(fullPathToFile);
        Products = JsonSerializer.Deserialize<List<Product>>(jsonProducts, ProgramSettings.JsonSerializerOptions);
    }

    /// <summary>
    /// Получить вес товара по его id.
    /// </summary>
    /// <param name="ProductId">Id товара.</param>
    /// <returns>Вес товара.</returns>
    private static double GetWeight(uint ProductId)
    {
        Random random = new();

        return double.Round(random.NextDouble() * (30 - 15) + 15, 2);
    }

    /// <summary>
    /// Получить цену товара по его id.
    /// </summary>
    /// <param name="ProductId">Id товара.</param>
    /// <returns>Цена товара.</returns>
    private static decimal GetPrice(uint ProductId)
    {
        Random random = new();

        return decimal.Round((decimal)(random.NextDouble() * (1000 - 500) + 500), 2);
    }

    /// <summary>
    /// Вывод товаров магазина.
    /// </summary>
    public static void PrintProductsInfo()
    {
        uint index = 0;
        foreach (Product product in Products)
        {
            Console.WriteLine($"{index += 1})");
            Console.WriteLine(product.ToString());
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Вывод типов товаров магазина.
    /// </summary>
    public static void PrintProductsTypes()
    {
        uint index = 0;
        foreach (Type productType in ProductsTypes)
        {
            Console.WriteLine($"{index += 1}) {productType.Name.ToString()}");
        }
    }
}
