using System.Reflection;
using System.Text.Json;

namespace Cart;

/// <summary>
/// Корзина (заказ) Интернет-магазина.
/// </summary>
[Serializable]
public class Order
{
    /// <summary>
    /// Товары в заказе. TKey - товар. TValue - количество товара.
    /// </summary>
    public List<KeyValuePair<Product, uint>> Products = new();

    /// <summary>
    /// Дата отправления заказа.
    /// </summary>
    public DateTime? TimeOfDeparture = null;

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
        Console.WriteLine($"Итоговая стоимость - {Products.Sum(product => product.Key.Price * product.Value)}");
        Console.WriteLine($"Итоговый вес - {Products.Sum(product => product.Key.Weight * product.Value)}");
    }

    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    public void ReadOrderFromConsole()
    {
        Console.WriteLine("Введите через пробел номер товара, количество товара, требование к цене.");
        Console.WriteLine("Возможные значения требования: 1 - самое низкое значение, 2 - самое высокое значение, 3 - любое значение.");
        Console.WriteLine("Каждый товар указывайте с новой строки.");
        Console.WriteLine("При окончании ввода введите end.");

        Order orderFromConsole = new();

        List<uint[]> ordersFromConsole = new();
        while (true)
        {
            string consoleReadLine = Console.ReadLine();
            if (consoleReadLine == "end")
            {
                break;
            }
            else
            {
                ordersFromConsole.Add(Array.ConvertAll(consoleReadLine.Split(" "), uint.Parse));
                uint productTypeNumber = ordersFromConsole.Last()[0];
                uint productNumber = ordersFromConsole.Last()[1];
                uint priceRequirement = ordersFromConsole.Last()[2];

                Type productType = Store.ProductsTypes[Convert.ToInt32(productTypeNumber - 1)];
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

                Products.Add(new KeyValuePair<Product, uint> (validProduct, productNumber));
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
        string jsonOrder = JsonSerializer.Serialize(this, jsonSerializerOptions);
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

        return JsonSerializer.Deserialize<Order>(jsonOrder, jsonSerializerOptions);
    }

    /// <summary>
    /// Изменить товар в заказе.
    /// </summary>
    /// <param name="product"></param>
    /// <param name="number"></param>
    public void UpdateProduct(uint productId, Product product, uint number)
    {
        int productIndexInOrder = this.Products.FindIndex(orderItem => orderItem.Key.Id == productId);
        int newProductIndexInOrder = this.Products.FindIndex(orderItem => orderItem.Key.Id == product.Id);
        KeyValuePair<Product, uint> newProduct;
        // Если продукта с таким id нет в заказе.
        if (newProductIndexInOrder == -1)
        {
            newProduct = new(product, number);
        }
        else
        {
            newProduct = new(product, number + this.Products[newProductIndexInOrder].Value);
            this.Products.RemoveAt(newProductIndexInOrder);
        }

        this.Products.Add(newProduct);
    }
}