namespace Cart.Orders;

internal class PrintOrderToConsole : IPrintOrder
{
    /// <summary>
    /// Вывести в консоль информацию заказе.
    /// </summary>
    public void Print(Order order)
    {
        uint index = 0;

        foreach (KeyValuePair<Product, uint> orderItem in order.Products)
        {
            Console.WriteLine($"{index += 1})");
            Console.WriteLine(orderItem.Key.ToString());
            Console.WriteLine($"Количество - {orderItem.Value}.");
            Console.WriteLine();
        }
        Console.WriteLine($"Итоговая стоимость - {order.Products.Sum(product => product.Key.Price * product.Value)}");
        Console.WriteLine($"Общее количество товаров - {order.Products.Sum(product => product.Value)}");
        Console.WriteLine($"Итоговый вес - {order.Products.Sum(product => product.Key.Weight * product.Value)}");
        Console.WriteLine($"Дата готовности заказа - {order.TimeOfDeparture}");
    }
}
