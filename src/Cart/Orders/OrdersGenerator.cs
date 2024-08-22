using System.Text.Json;
using Cart.Readers;
using Cart.Settings;
using Cart.Stores;

namespace Cart.Orders;

/// <summary>
/// Класс генератора тестовых заказов.
/// </summary>
public static class OrdersGenerator
{
    /// <summary>
    /// Список сгенерированных заказов.
    /// </summary>
    public static List<Order> Orders = new();

    /// <summary>
    /// 
    /// Генератор случайного числа.
    /// </summary>
    private static Random random = new();

    /// <summary>
    /// Создать и записать в файл 5 случайных заказов (наборов товаров) из списка товаров магазина.
    /// </summary>
    public static List<Order> GenerateRandomOrders(string title = "")
    {
        Console.Write(title);

        List<Order> orders = new();
        for (int i = 0; i < 5; i++)
        {
            Order order = new();
            foreach (Product product in Store.Products)
            {
                if (random.Next(0, 2) > 0)
                {
                    order.Products.Add(new KeyValuePair<Product, uint>(product, Convert.ToUInt32(random.Next(1, 4))));
                    order.TimeOfDeparture = DateTime.Now.AddDays(random.Next(0, 46));
                }
            }
            orders.Add(order);
        }

        File.WriteAllText(ProgramSettings.ProjectPath + Path.DirectorySeparatorChar + ProgramSettings.OrdersFileNameDefault, JsonSerializer.Serialize(orders, ProgramSettings.JsonSerializerOptions));
        
        Console.WriteLine("Заказы сгенерированы.");

        return orders;
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ.
    /// </summary>
    /// <returns>Заказ.</returns>
    public static Order GenerateRandomOrder(string title = "")
    {
        Console.Write(title);

        return Orders[random.Next(0, Orders.Count - 1)];
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ, сумма которого меньше указанной.
    /// </summary>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public static Order GenerateOrderBySum(decimal maxSum)
    {
        List<Order> validOrdersList = Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) <= maxSum).ToList();

        return validOrdersList[random.Next(0, validOrdersList.Count - 1)];
    }

    /// <summary>
    /// Сформировать (выбрать) заказ, сумма которого находится в указанных границах.
    /// </summary>
    /// <param name="minSum">Минимальная сумма заказа.</param>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public static Order GenerateOrderBySum(decimal minSum, decimal maxSum)
    {
        List<Order> validOrdersList = Orders.Where
            (order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) <= maxSum
        && order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) >= minSum).ToList();

        return validOrdersList[random.Next(0, validOrdersList.Count - 1)];
    }

    /// <summary>
    /// Сформировать (выбрать) заказ, общее количество товаров в котором не превышает заданного значения.
    /// </summary>
    /// <param name="maxQuantityOfProducts">Максимальное общее количество товаров в заказе.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public static Order GenerateOrderByMaxQuantity(uint maxQuantityOfProducts, string title = "")
    {
        Console.Write(title);

        List<Order> validOrdersList = Orders.Where(order => order.Products.Sum(orderItem => orderItem.Value) <= maxQuantityOfProducts).ToList();

        return validOrdersList[random.Next(0, validOrdersList.Count - 1)];
    }

    /// <summary>
    /// Прочитать заказы из файла.
    /// </summary>
    public static void ReadOrdersFromFile(string title = "")
    {
        Console.Write(title);

        string fullPathToFile = ConsoleReader.ReadFullFileNameFromConsole(ProgramSettings.OrdersFileNameDefault);
        string ordersJson = FileReader.ReadDataFromFile(fullPathToFile);
        Orders = JsonSerializer.Deserialize<List<Order>>(ordersJson, ProgramSettings.JsonSerializerOptions) ?? throw new ArgumentNullException();
        Console.WriteLine("Заказы считаны.");
    }
}
