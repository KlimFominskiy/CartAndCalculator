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
    /// Получить информацию о товарах в корзине.
    /// </summary>
    /// <returns>Информацию о товарах в корзине.</returns>
    public Dictionary<uint, Dictionary<object, string?>> GetOrderInfo()
    {
        //TKey - номер товара в корзине, TValue - словарь свойств товара (полей класса товара).
        //TKey вложенного словаря - наименование свойства. TValue вложенное словаря - значение свойства.
        Dictionary<uint, Dictionary<object, string?>> cartInfo = new(); 
        uint productNumber = 0;
        foreach (KeyValuePair<Product, uint> product in Products)
        {
            productNumber += 1;
            //TKey - наименование свойства, значение свойства.
            Dictionary<object, string?> propertiesInfo = new();
            foreach (PropertyInfo propertyInfo in product.GetType().GetProperties())
            {
                propertiesInfo.Add(propertyInfo.Name, propertyInfo.GetValue(product)?.ToString());
            }

            cartInfo.Add(productNumber, propertiesInfo);
        }
         
        return cartInfo;
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
}