using System.Reflection;
using System.Text.Json;
using Cart.Enums;
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
    /// Дата отправки заказа заказа.
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
    /// Вывести в консоль информацию заказе.
    /// </summary>
    public void PrintOrderInfo()
    {
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
        Console.WriteLine($"Дата готовности заказа - {this.TimeOfDeparture}");
    }

    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    public void ReadOrderFromConsole()
    {
        while (true)
        {
            OrderItemSettings orderItemSettings = new();

            orderItemSettings.ProductTypeNumber = ReadTypesFromConsole.ReadProductTypeNumberFromConsole();

            Console.WriteLine("Введите количество товара.");
            orderItemSettings.ProductQuantity = ReadTypesFromConsole.ReadUintFromConsole();

            Console.WriteLine("Введите требование к цене.");
            Console.WriteLine("Возможные значения требования: ");
            Console.WriteLine($"{(int)PriceRequirementSettings.TheLowestValue} - самое низкое значение,");
            Console.WriteLine($"{(int)PriceRequirementSettings.TheHighestValuem} - самое высокое значение,");
            Console.WriteLine($"{(int)PriceRequirementSettings.RandomValue} - любое значение.");

            orderItemSettings.PriceRequirement = ReadTypesFromConsole.ReadPriceRequirementFromConsole();
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
    public void UpdateProduct()
    {
        Console.WriteLine("Состав заказа.");
        this.PrintOrderInfo();

        Console.WriteLine("Выберите товар из заказа.");
        uint productInOrderNumber;
        while(true)
        {
            productInOrderNumber = ReadTypesFromConsole.ReadUintFromConsole();
            if (productInOrderNumber < this.Products.Count)
            {
                break;
            }
            else
            {
                Console.WriteLine($"Введено {productInOrderNumber}. Нет такого номера. Повторите ввод.");
                continue;
            }
        }

        Console.WriteLine("Введите номер продукта из списка продуктов магазина.");
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

        //Считать номер продукта из списка продуктоа магазина.
        Console.WriteLine("Введите количество нового продукта");
        uint productQuantity = ReadTypesFromConsole.ReadUintFromConsole();
        
        this.Products.Remove(this.Products[(int)productInOrderNumber - 1]);
        this.Products.Add(new KeyValuePair<Product, uint>(Store.Products[(int)productInStoreNumber - 1], productQuantity));
    }

    /// <summary>
    /// Скопировать значения полей заказа.
    /// </summary>
    /// <param name="other">Заказ, в который производится копирование.</param>
    public void CopyTo(Order other)
    {
        other.Products.AddRange(Products);
        other.TimeOfDeparture = TimeOfDeparture;
    }
}