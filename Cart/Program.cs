using Calculator;
using Cart.Products;
using System.Reflection;
using System.Text.Json;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        Store store = new();
        store.GenerateProducts();
        store.Products.Sort();

        Cart cart = new();
        Logger logger = new Logger();
        CartCalculator cartCalculator = new(logger);

        cart.Products.Add(store.Products.First(), 1);

        Cart cartTwo = new();
        cartTwo.Products.Add(store.Products.First(), 1);

        cartCalculator.Add(cart, cartTwo);
        cartCalculator.Subtract(cart, store.Products.First());
        cartCalculator.Subtract(cart, store.Products.First());

        //Console.WriteLine("Введите предпочтения к продуктам в формате Товар - Количество - Предпочтение. Например, Стиральная машина - 25 - Самая низкая цена");
        //List<string> Preference = new(["Самая низкая цена, самая высокая цена, самая низкий вес, самый высокий вес"]);
        //Console.WriteLine("Виды предпочтений");

    }
}