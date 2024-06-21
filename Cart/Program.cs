using Calculator;
using Cart.Products;
using System.Reflection;
using System.Text.Json;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        Store.Products = Store.GenerateProducts();

        Cart cartOne = new();
        Cart cartTwo = new();

        CartCalculator cartCalculator = new(new Logger());

        cartCalculator.Add(cartOne, Store.Products[0]);
        //cartCalculator.Add(cartOne, store.Products[0]);
        //cartCalculator.Add(cartOne, store.Products[0]);
        //cartCalculator.Add(cartOne, store.Products[1]);
        //cartCalculator.Add(cartOne, store.Products[1]);
        //cartCalculator.Divide(cartOne, 2);

        //CartGenerator.GenerateOrder();

        //Console.WriteLine("Введите предпочтения к продуктам в формате Товар - Количество - Предпочтение. Например, Стиральная машина - 25 - Самая низкая цена");
        //List<string> Preference = new(["Самая низкая цена, самая высокая цена, самая низкий вес, самый высокий вес"]);
        //Console.WriteLine("Виды предпочтений");
    }
}