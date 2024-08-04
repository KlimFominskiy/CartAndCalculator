using System.Text.Json;
using Cart.Stores;

namespace Cart.Orders;

/// <summary>
/// Класс генератора тестовых заказов.
/// </summary>
public static class OrdersGenerator
{
    /// <summary>
    /// Путь к директории проекта.
    /// </summary>
    private static string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

    /// <summary>
    /// Файл со списком сгенерированных заказов.
    /// </summary>
    private static string fileNameOrders = "Orders.json";

    /// <summary>
    /// Список сгенерированных заказов.
    /// </summary>
    public static List<Order> Orders = new();

    /// <summary>
    /// Генератор случайного числа.
    /// </summary>
    private static Random random = new Random();

    /// <summary>
    /// Сгенерированный (выбранный) заказ.
    /// </summary>
    private static Order cart = new();

    /// <summary>
    /// Настройка сеариализации.
    /// </summary>
    private static JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        IncludeFields = true,
    };

    /// <summary>
    /// Создать и записать в файл 5 случайных заказов (наборов товаров) из списка товаров магазина.
    /// </summary>
    public static void GenerateRandomOrders()
    {
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

        File.WriteAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders, JsonSerializer.Serialize(orders, jsonSerializerOptions));
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ.
    /// </summary>
    /// <returns>Заказ.</returns>
    public static Order GenerateRandomOrder()
    {
        return Orders[random.Next(0, Orders.Count - 1)];
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ, сумма которого меньше указанной.
    /// </summary>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public static Order GenerateOrderBySum(decimal maxSum)
    {
        List<Order> validOrdersList = Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < maxSum).ToList();

        return Orders[random.Next(0, validOrdersList.Count)];
    }

    /// <summary>
    /// Сформировать (выбрать) заказ, сумма которого находится в указанных границах.
    /// </summary>
    /// <param name="minSum">Минимальная сумма заказа.</param>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public static Order GenerateOrderBySum(decimal minSum, decimal maxSum)
    {
        List<Order> validOrders = Orders.Where
            (order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < maxSum
        && order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > minSum).ToList();

        return Orders[random.Next(0, validOrders.Count)];
    }

    /// <summary>
    /// Сформировать (выбрать) заказ, общее количество товаров в котором не превышает заданного значения.
    /// </summary>
    /// <param name="maxCount">Максимальное общее количество товаров в заказе.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public static Order GenerateOrderByMaxQuantity(uint maxCount)
    {
        List<Order> validOrders = Orders.Where(order => order.Products.Sum(orderItem => orderItem.Value) < maxCount).ToList();

        return Orders[random.Next(0, validOrders.Count)];
    }

    /// <summary>
    /// Вывести в консоль информацию о наборе заказов.
    /// </summary>
    /// <returns>Информация о заказах.</returns>
    public static void PrintOrdersInfo()
    {
        uint index = 0;
        foreach (Order order in Orders)
        {
            Console.WriteLine($"Заказ №{index += 1}");
            order.PrintOrderInfo();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Прочитать заказы из файла.
    /// </summary>
    public static void ReadOrdersFromFile()
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        Orders = JsonSerializer.Deserialize<List<Order>>(jsonOrdersList, jsonSerializerOptions);
    }
}
