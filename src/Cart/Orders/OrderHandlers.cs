using Cart.Menus;
using Cart.Settings;
using Cart.Stores;
using System.Text.Json;

namespace Cart.Orders;

public class OrderHandlers
{
    private IPrintOrder printOrderToAPI = new PrintOrderToAPI();

    private IPrintOrder printOrderToConsole = new PrintOrderToConsole();

    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    public Order ReadOrderFromConsole()
    {
        Order order = new();
        while (true)
        {
            OrderItemSettings orderItemSettings = new();

            Console.WriteLine("Типы товаров в магазине.");
            Store.PrintProductsTypes();
            Console.WriteLine("Введите тип товара.");
            orderItemSettings.ProductTypeNumber = ReadTypesFromConsole.ReadProductTypeNumberFromConsole();

            Console.WriteLine("Введите количество товара.");
            orderItemSettings.QuantityOfProduct = ReadTypesFromConsole.ReadUintFromConsole();
            
            Console.WriteLine("Введите требование к цене.");
            Menu.PrintOrderItemPriceSettings();
            orderItemSettings.PriceRequirement = ReadTypesFromConsole.ReadPriceRequirementFromConsole();

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
                    Random random = new Random();
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
    public void WriteOrderToFile(Order order)
    {
        string fullPathToFile = ReadTypesFromConsole.ReadFullFileNameFromConsole(ProgramSettings.OrderFileNameDefault);
        string jsonOrder = JsonSerializer.Serialize(order, ProgramSettings.JsonSerializerOptions);
        File.WriteAllText(fullPathToFile, jsonOrder);
    }

    /// <summary>
    /// Считать заказ из файла.
    /// </summary>
    /// <param name="fileName">Путь к файлу.</param>
    public Order ReadOrderFromFile()
    {
        string fullPathToFile = ReadTypesFromConsole.ReadFullFileNameFromConsole(ProgramSettings.OrderFileNameDefault);
        string orderJson = FileReader.ReadDataFromFile(fullPathToFile);

        return JsonSerializer.Deserialize<Order>(orderJson, ProgramSettings.JsonSerializerOptions);
    }

    /// <summary>
    /// Обновить продукт в заказе.
    /// </summary>
    public Order UpdateProduct(Order order)
    {
        Console.WriteLine("Состав заказа.");
        printOrderToConsole.Print(order);

        Console.WriteLine("Выберите товар из заказа.");
        uint productInOrderNumber;
        while (true)
        {
            productInOrderNumber = ReadTypesFromConsole.ReadUintFromConsole();
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

        Console.WriteLine("Введите номер продукта из списка товаров магазина.");
        Console.WriteLine("Список товаров магазина.");
        Store.PrintProductsInfo();
        uint productInStoreNumber;
        while (true)
        {
            productInStoreNumber = ReadTypesFromConsole.ReadUintFromConsole();
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
        uint productQuantity = ReadTypesFromConsole.ReadUintFromConsole();

        order.Products[(int)productInOrderNumber - 1] = new KeyValuePair<Product, uint>(Store.Products[(int)productInStoreNumber - 1], productQuantity);

        return order;
    }

    /// <summary>
    /// Скопировать значения полей заказа.
    /// </summary>
    /// <param name="other">Заказ, в который производится копирование.</param>
    public Order CopyFrom(Order other)
    {
        Order order = new Order();
        order.Products.AddRange(other.Products);
        order.TimeOfDeparture = other.TimeOfDeparture;

        return order;
    }

    /// <summary>
    /// Сортировка списка товаров в алфавитном порядке.
    /// </summary>
    public Order SortProductsByAlphabet(Order order)
    {
        int n = order.Products.Count;
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 0; i < n - 1; i++)
            {
                if (string.Compare(order.Products[i].Key.Name, order.Products[i + 1].Key.Name) > 0)
                {
                    var temp = order.Products[i];
                    order.Products[i] = order.Products[i + 1];
                    order.Products[i + 1] = temp;
                    swapped = true;
                }
            }
            n--;
        } while (swapped);

        return order;
    }
}
