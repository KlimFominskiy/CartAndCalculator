using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cart;

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
    private static JsonSerializerSettings jsonSerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Include
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
                    KeyValuePair<Product, uint> orderItem = new KeyValuePair<Product, uint>(product, Convert.ToUInt32(random.Next(1, 4)));
                    order.Products.Add(orderItem);
                    order.TimeOfDeparture = DateTime.Now;
                }
            }
            orders.Add(order);
        }

        File.AppendAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders, JsonConvert.SerializeObject(orders, jsonSerializerSettings));
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ.
    /// </summary>
    /// <returns>Заказ.</returns>
    public static Order GenerateRandomOrder()
    {
        int orderNumber = random.Next(0, Orders.Count - 1);

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
    public static Order GenerateOrderByCount(uint maxCount)
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
        foreach(Order order in Orders)
        {
            Console.WriteLine($"Заказ №{index += 1}");
            Console.WriteLine($"Название\t\tЦена\t\tКоличествоо\tСтоимость");
            foreach(KeyValuePair<Product, uint> products in order.Products)
            {
                Console.WriteLine($"{products.Key.Name}\t\t{products.Key.Price}\t{products.Value}\t{products.Key.Price * products.Value}");
            }
            Console.WriteLine($"Сумма заказа = {order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value)}.");
            Console.WriteLine($"Общее количество товаров в заказе = {order.Products.Sum(orderItem => orderItem.Value)}.");
            Console.WriteLine($"Общий вес заказа = ${order.Products.Sum(orderItem => orderItem.Key.Weight)}.");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Прочитать заказы из файла.
    /// </summary>
    public static void ReadOrdersFromFile()
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        List<Order> orders = new();
        orders = JsonConvert.DeserializeObject<List<Order>>(jsonOrdersList, jsonSerializerSettings);
    }
}
