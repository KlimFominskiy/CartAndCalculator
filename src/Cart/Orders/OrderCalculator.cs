using Calculator;

namespace Cart.Orders;

/// <summary>
/// Калькулятора для корзины в Интернет магазине.
/// </summary>

public class OrderCalculator : Calculator.Calculator
{
    OrderHandlers orderHandlers = new();

    public OrderCalculator(Logger? logger) : base(logger)
    {

    }

    /// <summary>
    /// Создать корзину с двумя указанными товарами.
    /// </summary>
    /// <param name="productA">Первый товар.</param>
    /// <param name="productB">Второй товар.</param>
    /// <returns>Карточка, с созданными товарами.</returns>
    public Order Add(Product productA, Product productB)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order order = new();
        if (productA == productB)
        {
            order.Products.Add(new KeyValuePair<Product, uint>(productA, 2));
        }
        else
        {
            order.Products.Add(new KeyValuePair<Product, uint>(productA, 1));
            order.Products.Add(new KeyValuePair<Product, uint>(productB, 1));
        }

        return order;
    }

    /// <summary>
    /// Добавить товар в корзину.
    /// </summary>
    /// <param name="order">Корзина.</param>
    /// <param name="product">Добавляемый товар.</param>
    /// <returns>Корзина с добавленным товаром.</returns>
    public Order Add(Order order, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = orderHandlers.CopyFrom(order);

        KeyValuePair<Product, uint> orderItem = newOrder.Products.FirstOrDefault(orderItem => orderItem.Key == product);
        Dictionary<Product, uint> products = newOrder.Products.ToDictionary();
        if (products.ContainsKey(product))
        {
            products[product] += 1;
            newOrder.Products = products.ToList();
        }
        else
        {
            newOrder.Products.Add(new KeyValuePair<Product, uint>(product, 1));
        }

        return newOrder;
    }

    /// <summary>
    /// Добавить товар в корзину.
    /// </summary>
    /// <param name="order">Корзина.</param>
    /// <param name="product">Добавляемый товар.</param>
    /// <returns>Карточка с добавленным товаром.</returns>
    public Order Add(Product product, Order order)
    {
        return Add(order, product);
    }

    /// <summary>
    /// Объединить корзины. 
    /// </summary>
    /// <param name="orderA">Первая корзина.</param>
    /// <param name="orderB">Вторая корзина.</param>
    /// <returns>Новая объединённая корзина.</returns>
    public Order Add(Order orderA, Order orderB)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order orderC = orderHandlers.CopyFrom(orderA);

        foreach (KeyValuePair<Product, uint> productB in orderB.Products)
        {
            for (uint i = 0; i < productB.Value; i++)
            {
                orderC = Add(orderC, productB.Key);
            }
        }

        return orderC;
    }

    /// <summary>
    /// Удалить из корзины единицу указанного товара.
    /// </summary>
    /// <param name="order">Корзина, из которой удаляется единица товара.</param>
    /// <param name="product">Удаляемый товар.</param>
    /// <returns>Заказ с удалённой единицей указанного товара.</returns>
    public Order Subtract(Order order, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = orderHandlers.CopyFrom(order);

        Dictionary<Product, uint> products = newOrder.Products.ToDictionary();
        if (products.ContainsKey(product))
        {
            if ((products[product] -= 1) == 0)
            {
                products.Remove(product);
            }
        }
        else
        {
            throw new Exception($"Продукт {product.Name} с Id = {product.Id} не найден в корзине.");
        }
        newOrder.Products = products.ToList();

        return newOrder;
    }

    /// <summary>
    /// Удалить из первой корзины товары, которые есть во второй корзине.
    /// </summary>
    /// <param name="orderA">Исходная корзина.</param>
    /// <param name="orderB">Вторая корзина.</param>
    /// <returns>Новая исходная корзина без удалённых товаров.</returns>
    /// <exception cref="Exception">Исключение, если во второй корзине есть товары, которых нет в первой (исходной) корзине.</exception>
    public Order Subtract(Order orderA, Order orderB)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order orderC = orderHandlers.CopyFrom(orderA);

        orderC.Products = orderC.Products.Where(orderItem => orderC.Products.Contains(orderItem) is false).ToList();

        return orderC;
    }

    /// <summary>
    /// Удалить из корзины товары указанного типа.
    /// </summary>
    /// <param name="order">Исходная корзина.</param>
    /// <param name="productType">Тип (класс) товара.</param>
    /// <returns>Новая исходная корзина без товаров указанного типа.</returns>
    public Order Divide(Order order, Type productType)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = orderHandlers.CopyFrom(order);

        newOrder.Products = newOrder.Products.Where(orderItem => orderItem.Key.GetType() != productType).ToList();

        return newOrder;
    }

    /// <summary>
    /// Уменьшить в карточке каждое количество товара в указанное число раз.
    /// </summary>
    /// <param name="order">Исходная карточка.</param>
    /// <param name="number">Число, указывающее во сколько раз мы уменьшаем количество товара.</param>
    /// <returns>Карточка с уменьшенным количеством каждого товара.</returns>
    public Order Divide(Order order, uint number)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = new();
        foreach (KeyValuePair<Product, uint> orderItem in order.Products)
        {
            KeyValuePair<Product, uint> newOrderItem = newOrderItem = new(orderItem.Key, orderItem.Value / number);
            newOrder.Products.Add(newOrderItem);
        }

        return newOrder;
    }

    /// <summary>
    /// Увеличить в карточке каждое количество товара в указанное число раз.
    /// </summary>
    /// <param name="order">Исходная карточка.</param>
    /// <param name="number">Число, указывающее во сколько раз мы увеличиваем количество товара.</param>
    /// <returns>Карточка с увеличенным количеством каждого товара.</returns>
    public Order Multiply(Order order, uint number)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = new();
        foreach (KeyValuePair<Product, uint> orderItem in order.Products)
        {
            KeyValuePair<Product, uint> newOrderItem = new KeyValuePair<Product, uint>(orderItem.Key, orderItem.Value * number);
            newOrder.Products.Add(newOrderItem);
        }

        return newOrder;
    }
}