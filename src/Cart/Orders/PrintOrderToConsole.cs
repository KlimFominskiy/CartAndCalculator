namespace Cart.Orders;

internal class PrintOrderToConsole : IPrintOrder
{
    /// <summary>
    /// Вывести в консоль информацию заказе.
    /// </summary>
    public virtual void Print(Order order, string title = "")
    {
        Console.Write(title);
        
        uint index = 0;
        foreach (KeyValuePair<Product, uint> orderItem in order.Products)
        {
            Console.WriteLine($"{index += 1})\n" +
                $"{orderItem.Key.ToString()}\n" +
                $"Количество - {orderItem.Value}.\n"
                );
        }
        Console.WriteLine($"Итоговая стоимость - {order.Products.Sum(product => product.Key.Price * product.Value)}.\n" +
            $"Общее количество товаров - {order.Products.Sum(product => product.Value)}.\n" +
            $"Итоговый вес - {order.Products.Sum(product => product.Key.Weight * product.Value)}.\n" +
            $"Дата отправления заказа - {order.TimeOfDeparture}.\n"
            );
    }
}