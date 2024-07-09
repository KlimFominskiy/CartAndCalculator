using Newtonsoft.Json;
using System.Reflection;

namespace Cart;

/// <summary>
/// Корзина (заказ) Интернет-магазина.
/// </summary>
public class Order
{
    /// <summary>
    /// Товары, добавленные в заказ. TKey - товар. TValue - количество товара.
    /// </summary>
    public List<KeyValuePair<Product, uint>> Products = new();

    /// <summary>
    /// Дата отправления заказа.
    /// </summary>
    public DateTime? TimeOfDeparture = null;

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
    /// Вывести в консоль информацию о товарах в корзине.
    /// </summary>
    public void PrintOrderInfo()
    {
        foreach (KeyValuePair<Product, uint> product in Products)
        {
            foreach (PropertyInfo propertyInfo in product.GetType().GetProperties())
            {
                Console.WriteLine($"{propertyInfo.Name} - {propertyInfo.GetValue(product)?.ToString()}");
            }
        }
    }

    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    public void ReadOrderFromConsole()
    {
        Console.WriteLine("Введите через пробел номер товара, количество товара, требование к цене.");
        Console.WriteLine("Возможные значени требований: 1 - самое низкое значение, 2 - самое высокое значение, 3 - любое значение.");
        Console.WriteLine("Каждый товар указывайте с новой строки.");
        Console.WriteLine("При окончании ввода введите end.");

        Order orderFromConsole = new();

        List<int[]> ordersFromConsole = new();
        while (true)
        {
            string consoleReadLine = Console.ReadLine();
            if (consoleReadLine == "end")
            {
                break;
            }
            else
            {
                ordersFromConsole.Add(Array.ConvertAll(consoleReadLine.Split(" "), int.Parse));
                int productTypeNumber = ordersFromConsole.Last()[0];
                int productNumber = ordersFromConsole.Last()[1];
                int priceRequirement = ordersFromConsole.Last()[2];

                Type productType = Store.ProductsTypes[productTypeNumber - 1];
                List<Product> validProducts = Store.Products.Where(product => product.GetType() == productType).ToList();
                Product validProduct;
                if (priceRequirement == 1)
                {
                    validProduct = validProducts.MinBy(product => product.Price);
                }
                else if (priceRequirement == 2)
                {
                    validProduct = validProducts.MaxBy(product => product.Price);
                }
                else
                {
                    Random random = new Random();
                    validProduct = validProducts[random.Next(0, validProducts.Count)];
                }

                Products.Add(new KeyValuePair<Product, uint> (validProduct, Convert.ToUInt32(productNumber)));
            }
        }
    }

    /// <summary>
    /// Записать заказ в файл Order.json.
    /// </summary>
    public void WriteOrderToFile()
    {
        string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string fileName = "Order.json";
        string jsonOrder = JsonConvert.SerializeObject(this, jsonSerializerSettings);
        File.AppendAllText(projectPath + Path.DirectorySeparatorChar + fileName, jsonOrder);
    }

    /// <summary>
    /// Считать заказ из файла.
    /// </summary>
    /// <param name="fileName">Путь к файлу.</param>
    public Order ReadOrderFromFile()
    {
        Console.WriteLine("Введите полный путь к файлу. Нажмите enter для использования файла по умолчанию.");
        string fileName = Console.ReadLine();
        if (string.IsNullOrEmpty(fileName))
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            fileName = (projectPath + Path.DirectorySeparatorChar + "Order.json");
        }
        string jsonOrder = File.ReadAllText(fileName);

        return JsonConvert.DeserializeObject<Order>(jsonOrder, jsonSerializerSettings);
    }
}