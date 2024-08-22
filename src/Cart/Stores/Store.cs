using Cart.Orders;
using Cart.Products;
using Cart.Readers;
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
    public static List<Product> GenerateProducts(string title = "")
    {
        Console.Write(title);

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
                isDryerIncluded: random.Next(0, 2) != 0
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

        File.WriteAllText(ConsoleReader.ReadFullFileNameFromConsole(ProgramSettings.ProductsFileNameDefault),
            JsonSerializer.Serialize(Products, ProgramSettings.JsonSerializerOptions));
        
        Console.WriteLine("Товары в магазине сгенерированы.");
        
        return Products;
    }

    /// <summary>
    /// Считать из файла список товаров магазина.
    /// </summary>
    public static void ReadProductsFromFile(string title = "")
    {
        Console.Write(title);
        
        Products = JsonSerializer.Deserialize<List<Product>>(
            FileReader.ReadDataFromFile(ConsoleReader.ReadFullFileNameFromConsole(ProgramSettings.ProductsFileNameDefault)), 
            ProgramSettings.JsonSerializerOptions
            ) ?? throw new ArgumentNullException();
        Console.WriteLine("Товары считаны.");
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
    public static void PrintProductsInfo(string title = "")
    {
        Console.Write(title);

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
    public static void PrintProductsTypes(string title = "")
    {
        Console.Write(title);
        uint index = 0;
        foreach (Type productType in ProductsTypes)
        {
            Console.WriteLine($"{index += 1}) {productType.Name}");
        }
    }
}
