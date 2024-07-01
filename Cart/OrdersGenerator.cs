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
    private string fileNameProducts = "Products.json";
    private string fileNameOrders = "Orders.json";
    private string jsonProductsList;
    private string productsList;
    private string jsonOrdersList;
    private List<Dictionary<ulong, ulong>> ordersList;
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
        jsonOrdersList = File.ReadAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders);
        ordersList = JsonSerializer.Deserialize<List<Dictionary<ulong, ulong>>>(jsonOrdersList);

    }

    /// <summary>
    /// Создать 5 случайных заказов (наборов товаров) из списка товаров магазина.
    /// </summary>
    public void GenerateRandomOrders()
    {

        List<Dictionary<ulong, ulong>> ordersList = new();
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
            ordersList.Add(order);
        }
        File.AppendAllText(projectPath + Path.DirectorySeparatorChar + fileNameOrders, JsonSerializer.Serialize<List<Dictionary<ulong, ulong>>>(ordersList, options));
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ из сгенерированных заказов.
    /// </summary>
    /// <returns></returns>
    public Cart GenerateRandomOrder()
    {
        int orderNumber = random.Next(0, ordersList.Count - 1);
        foreach (KeyValuePair<ulong, ulong> orderItem in ordersList[orderNumber])
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
    /// <returns>Заказ.</returns>
    public Cart GenerateOrderBySum(decimal maxSum)
    {
        Random random = new();
        Cart cart = new();

        List<int> validOrdersList = new();

        for (int i = 0; i < ordersList.Count; i++)
        {
            decimal totalSum = 0;
            foreach (KeyValuePair<ulong, ulong> orderItem in ordersList[i])
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
        foreach(KeyValuePair<ulong, ulong> orderItem in ordersList[validOrdersList[orderNumber]])
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
    /// Сформировать (выбрать) заказ, сумма которого находится в указанных границах.
    /// </summary>
    /// <param name="minSum">Минимальная сумма заказа.</param>
    /// <param name="maxSum">Максимальная сумма заказа.</param>
    /// <returns>Заказ.</returns>
    public Cart GenerateOrderBySum(decimal minSum, decimal maxSum)
    {
        Random random = new();
        Cart cart = new();

        List<int> validOrdersList = new();

        for (int i = 0; i < ordersList.Count; i++)
        {
            decimal totalSum = 0;
            foreach (KeyValuePair<ulong, ulong> orderItem in ordersList[i])
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
        foreach (KeyValuePair<ulong, ulong> orderItem in ordersList[validOrdersList[orderNumber]])
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
    /// <returns>Заказ.</returns>
    public Cart GenerateOrderByCount(ulong maxCount)
    {
        Random random = new();
        Cart cart = new();

        List<int> validOrdersList = new();

        for (int i = 0; i < ordersList.Count; i++)
        {
            decimal totalCount = 0;
            foreach (KeyValuePair<ulong, ulong> orderItem in ordersList[i])
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
}
