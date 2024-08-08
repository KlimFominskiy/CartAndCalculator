using Cart.Settings;
using Cart.Stores;
using System.Text.Json;

namespace Cart.Orders;

internal class OrderHandlers
{
    private Order order;
    public OrderHandlers(Order order)
    {
        this.order = order; 
    }

    /// <summary>
    /// Вывести в консоль информацию заказе.
    /// </summary>
    public void PrintOrderInfo()
    {
        uint index = 0;
        foreach (KeyValuePair<Product, uint> orderItem in order.Products)
        {
            Console.WriteLine($"{index += 1})");
            Console.WriteLine(orderItem.Key.ToString());
            Console.WriteLine($"Количество - {orderItem.Value}.");
            Console.WriteLine();
        }
        Console.WriteLine($"Итоговая стоимость - {order.Products.Sum(product => product.Key.Price * product.Value)}");
        Console.WriteLine($"Общее количество товаров - {order.Products.Sum(product => product.Value)}");
        Console.WriteLine($"Итоговый вес - {order.Products.Sum(product => product.Key.Weight * product.Value)}");
        Console.WriteLine($"Дата готовности заказа - {order.TimeOfDeparture}");
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
                order.Products.Add(new KeyValuePair<Product, uint>(validProduct, orderItemSettings.ProductQuantity));
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
        string jsonOrder = JsonSerializer.Serialize(this, ProgramSettings.JsonSerializerOptions);
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
            return JsonSerializer.Deserialize<Order>(jsonOrder, ProgramSettings.JsonSerializerOptions);
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

        Console.WriteLine("Введите количество нового продукта");
        uint productQuantity = ReadTypesFromConsole.ReadUintFromConsole();

        order.Products.Remove(order.Products[(int)productInOrderNumber - 1]);
        order.Products.Add(new KeyValuePair<Product, uint>(Store.Products[(int)productInStoreNumber - 1], productQuantity));
    }

    /// <summary>
    /// Скопировать значения полей заказа.
    /// </summary>
    /// <param name="other">Заказ, в который производится копирование.</param>
    public void CopyTo(Order other)
    {
        other.Products.AddRange(order.Products);
        other.TimeOfDeparture = order.TimeOfDeparture;
    }

    /// <summary>
    /// Сортировка списка товаров в алфавитном порядке.
    /// </summary>
    public void SortProductsByAlphabet()
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
    }
}
