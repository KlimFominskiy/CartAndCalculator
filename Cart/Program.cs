using Calculator;
using Cart.Products;
using System.Text.Json;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        //Store.GenerateProducts();
        OrdersGenerator ordersGenerator = new();
        //ordersGenerator.GenerateRandomOrders();
        //ordersGenerator.GenerateRandomOrder();
        ordersGenerator.GenerateOrderBySum(5000);

        //Console.WriteLine("Введите предпочтения к продуктам в формате Товар - Количество - Предпочтение. Например, Стиральная машина - 25 - Самая низкая цена");
        //List<string> Preference = new(["Самая низкая цена, самая высокая цена, самая низкий вес, самый высокий вес"]);
        //Console.WriteLine("Виды предпочтений");
    }
}