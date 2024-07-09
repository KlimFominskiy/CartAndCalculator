using System.Text;

namespace Cart;

internal class Program
{
    internal static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        //Store.GenerateProducts();
        Store.ReadProductsFromFile();
        //OrdersGenerator.GenerateRandomOrders();
        OrdersGenerator.ReadOrdersFromFile();

        // Задание 1.
        // Вывод ассортимента. Параметры типа товара - цена, вес.
        // Значения параметров - самый низкий, самый высокий, любой (случайный).
        Store.PrintProductsTypesInfo();
        Order orderFromConsole = new Order();
        orderFromConsole.ReadOrderFromConsole();

        return;

        CartCalculator cartCalculator = new(new Calculator.Logger());
        Order orderA = new();
        orderA = cartCalculator.Add(Store.Products[0], Store.Products[1]);
        orderA = cartCalculator.Add(Store.Products[0], Store.Products[0]);
        orderA = cartCalculator.Add(orderA, Store.Products[1]);
        orderA = cartCalculator.Add(orderA, Store.Products[1]);
        Order orderB = new();
        orderB = cartCalculator.Add(orderB, Store.Products[1]);
        Order orderC = cartCalculator.Add(orderA, orderB);
        orderC = cartCalculator.Multiply(orderC, 2);

        // Работа с LINQ (задание 5).
        List<Order> validOrders = new();
        // Заказы, дешевле заданной суммы (10000.00).
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < 10000.00M).ToList();
        // Заказы, дороже заданной суммы (10000.00).
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > 10000.00M).ToList();
        // Заказы, имеющие в составе товары определённого типа.
        Type productType = Store.Products.First().GetType();
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Any(orderItem => orderItem.Key.GetType() == productType)).ToList();
        // Заказы, отсортированные по весу в порядке возрастания.
        validOrders = OrdersGenerator.Orders.OrderBy(order => order.Products.Sum(orderItem => orderItem.Key.Weight)).ToList();
        // Заказы, с уникальными названиями.
        //validOrders = orders.Select(order => order.DistinctBy(orderItem => orderItem.Key.Name).ToDictionary()).ToList();
        validOrders = OrdersGenerator.Orders.Where(order => order.Products.All(orderItem => orderItem.Value == 1)).ToList();
        // Заказы, отправленные до указанной даты.
        DateTime maxDepartureDateTime = DateTime.Now.AddDays(1);
        validOrders = OrdersGenerator.Orders.Where(order => order.TimeOfDeparture <= maxDepartureDateTime).ToList();
    }
}