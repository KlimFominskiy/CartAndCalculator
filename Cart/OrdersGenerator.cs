using System.Collections.Generic;
using System.Text.Json;

namespace Cart;

public class OrdersGenerator
{
    /// <summary>
    /// Список товаров магазина.
    /// </summary>
    public List<Product> StoreProductsList { get; set; }
    /// <summary>
    /// Путь к директории проекта.
    /// </summary>
    private string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    /// <summary>
    /// Файл со списком продуктов в магазине.
    /// </summary>
    private string fileNameProducts = "Products.json";
    /// <summary>
    /// Файл со списком сгенерированных заказов.
    /// </summary>
    private string fileNameOrders = "Orders.json";
    /// <summary>
    /// Список продуктов в магазине в формате строки JSON.
    /// </summary>
    private string jsonProductsList;
    /// <summary>
    /// Список заказов. TKey - id продукта, TValue - количество продукта.
    /// </summary>
    private List<Dictionary<ulong, ulong>> ordersDictionariesList;
    private Random random = new();
    private Cart cart = new();
    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public OrdersGenerator()
    {
        jsonProductsList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameProducts);
        StoreProductsList = JsonSerializer.Deserialize<List<Product>>(jsonProductsList);
    }

    /// <summary>
    /// Создать и записать 5 случайных заказов (наборов товаров) из списка товаров магазина.
    /// </summary>
    public void GenerateRandomOrders()
    {
        ordersDictionariesList = new();
        for (int i = 0; i < 5; i++)
        {
            Dictionary<ulong, ulong> order = new();
            foreach (Product product in StoreProductsList)
            {
                if (random.Next(0, 2) > 0)
                {
                    order.Add(product.Id, Convert.ToUInt64(random.Next(1, 4)));
                }
            }
            ordersDictionariesList.Add(order);
        }
        File.AppendAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders, JsonSerializer.Serialize<List<Dictionary<ulong, ulong>>>(ordersDictionariesList, options));
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ из сгенерированных заказов.
    /// </summary>
    /// <returns>Заказ.</returns>
    public Cart GenerateRandomOrder()
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        ordersDictionariesList = JsonSerializer.Deserialize<List<Dictionary<ulong, ulong>>>(jsonOrdersList);
        int orderNumber = random.Next(0, ordersDictionariesList.Count - 1);
        foreach (KeyValuePair<ulong, ulong> orderItem in ordersDictionariesList[orderNumber])
        {
            Product? product = StoreProductsList.Where(productItem => productItem.Id == orderItem.Key).FirstOrDefault();
            if (product != null)
            {
                cart.Products.Add(product, orderItem.Value);
            }
            else
            {
                Console.WriteLine($"Продукт с id = {orderItem.Key} не найден в списке продуктов магазина.");
                break;
            }
        }

        return cart;
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ, сумма которого меньше указанной.
    /// </summary>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public Cart GenerateOrderBySum(decimal maxSum)
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        ordersDictionariesList = JsonSerializer.Deserialize<List<Dictionary<ulong, ulong>>>(jsonOrdersList);

        List<int> validOrdersList = new();

        for (int i = 0; i < ordersDictionariesList.Count; i++)
        {
            decimal totalSum = 0;
            foreach (KeyValuePair<ulong, ulong> orderItem in ordersDictionariesList[i])
            {
                decimal orderItemPrice = StoreProductsList.Where(productItem => productItem.Id == orderItem.Key).FirstOrDefault().Price;
                totalSum += orderItemPrice;
            }
            if(totalSum < maxSum)
            {
                validOrdersList.Add(i);
            }
        }

        int orderNumber = random.Next(0, validOrdersList.Count);
        foreach(KeyValuePair<ulong, ulong> orderItem in ordersDictionariesList[validOrdersList[orderNumber]])
        {
            Product? product = StoreProductsList.Where(productItem => productItem.Id == orderItem.Key).FirstOrDefault();
            if (product != null)
            {
                cart.Products.Add(product, orderItem.Value);
            }
            else
            {
                Console.WriteLine($"Продукт с id = {orderItem.Key} не найден в списке продуктов магазина.");
            }
        }

        return cart;
    }

    /// <summary>
    /// Сформировать (выбрать) заказ, сумма которого находится в указанных границах.
    /// </summary>
    /// <param name="minSum">Минимальная сумма заказа.</param>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public Cart GenerateOrderBySum(decimal minSum, decimal maxSum)
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        ordersDictionariesList = JsonSerializer.Deserialize<List<Dictionary<ulong, ulong>>>(jsonOrdersList);

        List<int> validOrdersList = new();

        for (int i = 0; i < ordersDictionariesList.Count; i++)
        {
            decimal totalSum = 0;
            foreach (KeyValuePair<ulong, ulong> orderItem in ordersDictionariesList[i])
            {
                decimal orderItemPrice = StoreProductsList.Where(productItem => productItem.Id == orderItem.Key).FirstOrDefault().Price;
                totalSum += orderItemPrice + orderItem.Value;
            }
            if (totalSum > minSum && totalSum < maxSum)
            {
                validOrdersList.Add(i);
            }
        }

        int orderNumber = random.Next(0, validOrdersList.Count);
        foreach (KeyValuePair<ulong, ulong> orderItem in ordersDictionariesList[validOrdersList[orderNumber]])
        {
            Product? product = StoreProductsList.Where(productItem => productItem.Id == orderItem.Key).FirstOrDefault();
            if (product != null)
            {
                cart.Products.Add(product, orderItem.Value);
            }
            else
            {
                Console.WriteLine($"Продукт с id = {orderItem.Key} не найден в списке продуктов магазина.");
                break;
            }
        }

        return cart;
    }

    /// <summary>
    /// Сформировать (выбрать) заказ, общее количество товаров в котором не превышает заданного значения.
    /// </summary>
    /// <param name="maxCount">Максимальное общее количество товаров в заказе.</param>
    /// <returns>Заказ, удовлетворяющий параметрам.</returns>
    public Cart GenerateOrderByCount(ulong maxCount)
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        ordersDictionariesList = JsonSerializer.Deserialize<List<Dictionary<ulong, ulong>>>(jsonOrdersList);

        Random random = new();
        Cart cart = new();

        List<int> validOrdersList = new();

        for (int i = 0; i < ordersDictionariesList.Count; i++)
        {
            decimal totalCount = 0;
            foreach (KeyValuePair<ulong, ulong> orderItem in ordersDictionariesList[i])
            {
                totalCount += orderItem.Value;
            }
            if (totalCount < maxCount)
            {
                validOrdersList.Add(i);
            }
        }

        return cart;
    }

    public List<Dictionary<Product, ulong>> GetOrdersInfo()
    {
        string jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        ordersDictionariesList = JsonSerializer.Deserialize<List<Dictionary<ulong, ulong>>>(jsonOrdersList);
        List<Dictionary<Product, ulong>> ordersDictionary = new();

        List<decimal> ordersSumList = new();
        List<ulong> ordersProductNumberList = new();
        uint index = 0;
        foreach(Dictionary<ulong, ulong> order in ordersDictionariesList)
        {
            Cart cart = new();
            //Console.WriteLine($"Заказ №{++index}");
            //Console.WriteLine($"Название\t\tЦена\t\tКоличествоо\tСтоимость");
            foreach (KeyValuePair<ulong, ulong> orderItem in order)
            {
                Product? product = StoreProductsList.Where(productItem => productItem.Id == orderItem.Key).FirstOrDefault();
                cart.Products.Add(product, orderItem.Value);
                //Console.WriteLine($"{product.Name}\t\t{product.Price}\t{orderItem.Value}\t{product.Price * orderItem.Value}");
            }
            ordersDictionary.Add(cart.Products);
            //Console.WriteLine($"Сумма заказа = {cart.GetTotalPrice()}.");
            //Console.WriteLine($"Общее количество товаров в заказе = {cart.GetTotalProductsNumber()}.");
            //Console.WriteLine();
        }

        return ordersDictionary;
    }
}
