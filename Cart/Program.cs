using System.Text;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        //Store.GenerateProducts();
        Store.ReadProductsFromFile();
        OrdersGenerator.GenerateRandomOrders();
        //OrdersGenerator.ReadOrdersFromFile();

        return;

        Cart cart = new Cart();
        CartCalculator cartCalculator = new(new Calculator.Logger());

        List<Cart> orders = OrdersGenerator.Orders;
        //Заказы дешевле дешевле указанной стоимости.
        List<Cart> validOrders = new();
        // Заказы, дешевле заданной суммы (10000.00).
        validOrders = orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < 10000.00M).ToList();
        // Заказы, дороже заданной суммы (10000.00).
        validOrders = orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > 10000.00M).ToList();
        // Заказы, имеющие в составе товары определённого типа.
        Type productType = Store.Products.First().GetType();
        validOrders = orders.Where(order => order.Products.Any(orderItem => orderItem.Key.GetType() == productType)).ToList();
        // Заказы, отсортированные по весу в порядке возрастания.
        validOrders = orders.OrderBy(order => order.Products.Sum(orderItem => orderItem.Key.Weight)).ToList();
        // Заказы, с уникальными названиями.
        //validOrders = orders.Select(order => order.DistinctBy(orderItem => orderItem.Key.Name).ToDictionary()).ToList();
        validOrders = orders.Where(order => order.Products.All(orderItem => orderItem.Value == 1)).ToList();
        // Заказы, отправленные до указанной даты.
        DateTime maxDepartureDateTime = DateTime.Now.AddDays(1);
        validOrders = orders.Where(order => order.TimeOfDeparture <= maxDepartureDateTime).ToList();
    }
}