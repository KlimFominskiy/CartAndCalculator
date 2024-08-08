using Cart.Enums;
using Cart.Orders;
using Cart.Products;
using Cart.Stores;
using System.Text;

namespace Cart;

internal static class Program
{
    internal static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Menu.Start();
        Menu.PrintProgramModes();
        Menu.ChooseMode();
    }
}