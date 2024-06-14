using System.Reflection;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        WashingMachine BoshWMA = new(
            id: 1,
            name: nameof(BoshWMA),
            weight: 100,
            price: 2000,
            timeOfArrival: DateTime.Now,
            isDryerIncluded: true
            );
        WashingMachine BoshWMB = new(
            id: 2,
            name: nameof(BoshWMB),
            weight: 100,
            price: 2000,
            timeOfArrival: DateTime.Now,
            isDryerIncluded: false
            );
        WashingMachine BoshWMC = new(
            id: 3,
            name: nameof(BoshWMC),
            weight: 100,
            price: 2000,
            timeOfArrival: DateTime.Now,
            isDryerIncluded: false
            );

        Cart cart = new(products: [BoshWMB, BoshWMA, BoshWMC]);

        foreach (var item in cart.GetCartInfo())
        {
            foreach(var itemInfo in  item.Value)
            {
                Console.WriteLine($"{itemInfo.Key} = {itemInfo.Value}");
            }
            Console.WriteLine();
        }
    }
}