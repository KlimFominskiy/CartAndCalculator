using Cart.Products;
using System.Reflection;
using System.Text.Json;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        // Генерация товаров в магазине.
        Store store = new();
        store.GenerateProducts();

        Cart cart = new();
        cart.Products.AddRange(store.Products);
    }
}