using Calculator;
using Cart.Products;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        //Store.GenerateProducts();
        OrdersGenerator ordersGenerator = new();
        //ordersGenerator.GenerateRandomOrders();
        List<Dictionary<Product, ulong>> ordersDictionary = new();
        ordersDictionary = ordersGenerator.GetOrdersInfo();
        //Заказы дешевле дешевле указанной стоимости.
        List<Dictionary<Product, ulong>> validOrders = new();
        validOrders = ordersDictionary.Where(order => order.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < 5000).ToList();


        //Console.WriteLine("Введите предпочтения к продуктам в формате Товар - Количество - Предпочтение. Например, Стиральная машина - 25 - Самая низкая цена");
        //List<string> Preference = new(["Самая низкая цена, самая высокая цена, самая низкий вес, самый высокий вес"]);
        //Console.WriteLine("Виды предпочтений");
    }
}