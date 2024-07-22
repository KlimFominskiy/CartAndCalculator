using System.Reflection;
using System.Text.Json;
using Cart.Enums;
using Cart.Products;
using Cart.Stores;

namespace Cart.Orders;

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
    private JsonSerializerOptions jsonSerializerOptions = new()
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
        Console.WriteLine();

        uint index = 0;
        foreach (KeyValuePair<Product, uint> orderItem in Products)
        {
            Console.WriteLine($"{index += 1})");
            foreach (PropertyInfo orderItemInfo in orderItem.Key.GetType().GetProperties())
            {
                //foreach (PropertyInfo productInfo in orderItemInfo)
                //{
                //    Console.WriteLine($"{productInfo.Name}, {productInfo.GetValue(productInfo)?.ToString()}");
                //}
            Console.WriteLine($"{orderItemInfo.Name} - {orderItemInfo.GetValue(orderItem.Key)?.ToString()}.");
            }
            Console.WriteLine($"Количество - {orderItem.Value}.");
            Console.WriteLine();
        }
        Console.WriteLine($"Итоговая стоимость - {Products.Sum(product => product.Key.Price * product.Value)}");
        Console.WriteLine($"Общее количество товаров - {Products.Sum(product => product.Value)}");
        Console.WriteLine($"Итоговый вес - {Products.Sum(product => product.Key.Weight * product.Value)}");
    }

    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    public void ReadOrderFromConsole()
    {
        while (true)
        {
            OrderItemSettings orderItemSettings = new();

            orderItemSettings.ProductTypeNumber = ReadProductTypeNumberFromConsole();

            Console.WriteLine("Введите количество товара.");
            orderItemSettings.ProductQuantity = ReadProductQiantityFromConsole();

            Console.WriteLine("Введите требование к цене.");
            Console.WriteLine("Возможные значения требования: ");
            Console.WriteLine($"{(int)PriceRequirementSettings.TheLowestValue} - самое низкое значение,");
            Console.WriteLine($"{(int)PriceRequirementSettings.TheHighestValuem} - самое высокое значение,");
            Console.WriteLine($"{(int)PriceRequirementSettings.RandomValue} - любое значение.");

            orderItemSettings.PriceRequirement = ReadPriceRequirementFromConsole();
            Type productType = Store.ProductsTypes[Convert.ToInt32(orderItemSettings.ProductTypeNumber - 1)];
            
            List<Product> validProducts = new();
            foreach(Product product in Store.Products.Where(product => product.GetType() == productType).ToList())
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
                case PriceRequirementSettings.TheHighestValuem:
                    validProduct = validProducts.MaxBy(product => product.Price);
                    break;
                case PriceRequirementSettings.RandomValue:
                    Random random = new Random();
                    validProduct = validProducts[random.Next(0, validProducts.Count)];
                    break;
            }

            if (validProduct is not null)
            {
                Products.Add(new KeyValuePair<Product, uint>(validProduct, orderItemSettings.ProductQuantity));
            }

            Console.WriteLine("Введите end, чтобы закончить ввод. Для продолжения введите любой символ.");
            if (Console.ReadLine() == "end")
            {
                break;
            }
        }
    }

    /// <summary>
    /// Записать заказ в файл Order.json.
    /// </summary>
    public void WriteOrderToFile()
    {
        Console.WriteLine();
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
        Console.WriteLine("Введите полный путь к файлу формата JSON. Нажмите enter для использования файла по умолчанию.");
        while (true)
        {
            string fileName = Console.ReadLine();
            if (string.IsNullOrEmpty(fileName))
            {
                string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                fileName = projectPath + Path.DirectorySeparatorChar + "Order.json";
            }
            else
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"Считан путь:  {fileName}. Файл не найден. Повторите ввод.");
                    continue;
                }

            }
            string jsonOrder = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<Order>(jsonOrder, jsonSerializerOptions);
        }
    }

    /// <summary>
    /// Обновить продукт в заказе.
    /// </summary>
    /// <param name="productId">Идентификационный номер товара.</param>
    /// <param name="product">Данные о товаре.</param>
    /// <param name="quantity">Количество товара.</param>
    public void UpdateProduct(uint productId, Product product, uint quantity)
    {
        int productIndexInOrder = Products.FindIndex(orderItem => orderItem.Key.Id == productId);
        int newProductIndexInOrder = Products.FindIndex(orderItem => orderItem.Key.Id == product.Id);
        KeyValuePair<Product, uint> newProduct;
        // Если товара с таким id нет в заказе.
        if (newProductIndexInOrder == -1)
        {
            newProduct = new(product, quantity);
        }
        else
        {
            newProduct = new(product, quantity + Products[newProductIndexInOrder].Value);
            Products.RemoveAt(newProductIndexInOrder);
        }

        Products.Add(newProduct);
    }

    public void CopyTo(Order other)
    {
        other.Products.AddRange(Products);
        other.TimeOfDeparture = TimeOfDeparture;
    }

    private uint ReadProductTypeNumberFromConsole()
    {
        while (true)
        {
            string orderItemSetting = Console.ReadLine();
            if (uint.TryParse(orderItemSetting, out uint setting))
            {
                if (setting > Store.ProductsTypes.Count)
                {
                    Console.WriteLine($"Введён тип {setting}. Такого типа товара нет в списке. Повторите ввод.");
                    continue;
                }
                return setting;
            }
            else
            {
                Console.WriteLine($"Введено {orderItemSetting}. Неправильный формат. Повторите ввод.");
                continue;
            }
        }
    }

    private uint ReadProductQiantityFromConsole()
    {
        while (true)
        {
            string orderItemSetting = Console.ReadLine();
            if (uint.TryParse(orderItemSetting, out uint setting))
            {
                return setting;
            }
            else
            {
                Console.WriteLine($"Введено {orderItemSetting}. Неправильный формат. Повторите ввод.");
                continue;
            }
        }
    }

    private PriceRequirementSettings ReadPriceRequirementFromConsole()
    {
        while (true)
        {
            string orderItemSetting = Console.ReadLine();
            if (Enum.TryParse(orderItemSetting, out PriceRequirementSettings setting))
            {
                if (Enum.IsDefined(typeof(PriceRequirementSettings), setting))
                {
                    return setting;
                }
                else
                {
                    Console.WriteLine($"Введено {setting}. Такой настройки требования нет. Повторите ввод.");
                    continue;
                }
            }
            else
            {
                Console.WriteLine($"Введено {orderItemSetting}. Неправильный формат. Повторите ввод.");
            }
        }
    }
}