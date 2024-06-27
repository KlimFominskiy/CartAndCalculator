using Calculator;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        //Store.Products = Store.GenerateProducts();

        Cart cartOne = new();
        Cart cartTwo = new();

        CartCalculator cartCalculator = new(new Logger());

        OrdersGenerator ordersGenerator = new();
        ordersGenerator.GenerateRandomOrder();

        //Console.WriteLine("Введите предпочтения к продуктам в формате Товар - Количество - Предпочтение. Например, Стиральная машина - 25 - Самая низкая цена");
        //List<string> Preference = new(["Самая низкая цена, самая высокая цена, самая низкий вес, самый высокий вес"]);
        //Console.WriteLine("Виды предпочтений");
    }
}