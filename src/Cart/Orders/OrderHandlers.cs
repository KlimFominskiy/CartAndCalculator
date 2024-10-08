﻿using Cart.Menus;
using Cart.Readers;
using Cart.Settings;
using Cart.Stores;
using System.Text.Json;

namespace Cart.Orders;

public class OrderHandlers
{
    private readonly IPrintOrder printOrderToAPI = new PrintOrderToAPI();

    private readonly IPrintOrder printOrderToConsole = new PrintOrderToConsole();

    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    public Order ReadOrderFromConsole(string title = "")
    {
        Console.Write(title);

        Order order = new();
        while (true)
        {
            OrderItemSettings orderItemSettings = new();

            Store.PrintProductsTypes("Типы товаров в магазине.\n");
            orderItemSettings.ProductTypeNumber = ConsoleReader.ReadProductTypeNumberFromConsole("Введите тип товара.\n");

            orderItemSettings.QuantityOfProduct = ConsoleReader.ReadUintFromConsole("Введите количество товара.\n");
            
            Menu.PrintOrderItemPriceSettings("Введите требование к цене.\n");
            orderItemSettings.PriceRequirement = ConsoleReader.ReadPriceRequirementFromConsole();

            Type productType = Store.ProductsTypes[Convert.ToInt32(orderItemSettings.ProductTypeNumber - 1)];
            List<Product> validProducts = new();
            foreach (Product product in Store.Products.Where(product => product.GetType() == productType).ToList())
            {
                Product newProduct = product with { };
                validProducts.Add(newProduct);
            }
            
            Product? validProduct = null;
            switch (orderItemSettings.PriceRequirement)
            {
                case PriceRequirementSettings.TheLowestValue:
                    validProduct = validProducts.MinBy(product => product.Price);
                    break;
                case PriceRequirementSettings.TheHighestValue:
                    validProduct = validProducts.MaxBy(product => product.Price);
                    break;
                case PriceRequirementSettings.RandomValue:
                    Random random = new();
                    validProduct = validProducts[random.Next(0, validProducts.Count - 1)];
                    break;
            }

            if (validProduct is not null)
            {
                order.Products.Add(new KeyValuePair<Product, uint>(validProduct, orderItemSettings.QuantityOfProduct));
            }

            Console.WriteLine("Введите end, чтобы закончить ввод. Для продолжения введите любой символ.");
            if (Console.ReadLine() == "end")
            {
                Console.WriteLine("Считывание заказа из консоли завершено.");
                return order;
            }
            else
            {
                continue;
            }
        }
    }

    /// <summary>
    /// Записать заказ в файл Order.json.
    /// </summary>
    public void WriteOrderToFile(Order order, string title ="")
    {
        Console.Write(title);

        string fullPathToFile = ConsoleReader.ReadFullFileNameFromConsole(ProgramSettings.OrderFileNameDefault);
        string jsonOrder = JsonSerializer.Serialize(order, ProgramSettings.JsonSerializerOptions);
        File.WriteAllText(fullPathToFile, jsonOrder);
        Console.WriteLine("Запись заказа в файл окончена.");
    }

    /// <summary>
    /// Считать заказ из файла.
    /// </summary>
    /// <param name="fileName">Путь к файлу.</param>
    public Order ReadOrderFromFile(string title = "")
    {
        Console.Write(title);

        string orderJson = FileReader.ReadDataFromFile(ConsoleReader.ReadFullFileNameFromConsole(ProgramSettings.OrderFileNameDefault));
        Console.WriteLine("Считывание заказа из файла завершено.");

        return JsonSerializer.Deserialize<Order>(orderJson, ProgramSettings.JsonSerializerOptions) ?? throw new ArgumentNullException();
    }

    /// <summary>
    /// Обновить продукт в заказе.
    /// </summary>
    public Order UpdateProduct(Order order, string title = "")
    {
        Console.Write(title);

        if (order.Products.Count == 0)
        {
            Console.WriteLine("Заказ пуст.");
            return order;
        }

        Console.WriteLine("Состав заказа.");
        printOrderToConsole.Print(order);

        Console.WriteLine("Выберите товар из заказа.");
        uint productInOrderNumber;
        while (true)
        {
            productInOrderNumber = ConsoleReader.ReadUintFromConsole();
            if (productInOrderNumber < order.Products.Count)
            {
                break;
            }
            else
            {
                Console.WriteLine($"Введено {productInOrderNumber}. Нет такого номера. Повторите ввод.");
                continue;
            }
        }

        Store.PrintProductsInfo("Список товаров магазина.\n");
        Console.WriteLine("Введите номер продукта из списка товаров магазина.");
        uint productInStoreNumber;
        while (true)
        {
            productInStoreNumber = ConsoleReader.ReadUintFromConsole();
            if (productInOrderNumber < Store.Products.Count)
            {
                break;
            }
            else
            {
                Console.WriteLine($"Введено {productInStoreNumber}. Нет такого номера. Повторите ввод.");
                continue;
            }
        }

        Console.WriteLine("Введите количество нового продукта");
        uint productQuantity = ConsoleReader.ReadUintFromConsole();

        order.Products[(int)productInOrderNumber - 1] = new KeyValuePair<Product, uint>(Store.Products[(int)productInStoreNumber - 1], productQuantity);

        return order;
    }

    /// <summary>
    /// Скопировать значения полей заказа.
    /// </summary>
    /// <param name="other">Заказ, в который производится копирование.</param>
    public Order CopyFrom(Order other)
    {
        Order order = new();
        order.Products.AddRange(other.Products);
        order.TimeOfDeparture = other.TimeOfDeparture;

        return order;
    }

    /// <summary>
    /// Сортировка списка товаров в алфавитном порядке.
    /// </summary>
    public Order SortProductsByAlphabet(Order order, string title = "")
    {
        Console.Write(title);

        int n = order.Products.Count;
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 0; i < n - 1; i++)
            {
                if (string.Compare(order.Products[i].Key.Name, order.Products[i + 1].Key.Name) > 0)
                {
                    (order.Products[i + 1], order.Products[i]) = (order.Products[i], order.Products[i + 1]);
                    swapped = true;
                }
            }
            n--;
        } while (swapped);

        return order;
    }
}
