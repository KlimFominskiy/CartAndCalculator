using Calculator;

namespace Cart;

/// <summary>
/// Калькулятора для корзины в Интернет магазине.
/// </summary>

public class CartCalculator : Calculator.Calculator
{
    public CartCalculator(Logger? logger) : base(logger)
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
    /// <returns>Карточка с добавленным товаром.</returns>
    public Order Add(Order order, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        KeyValuePair<Product, uint> orderItem = order.Products.FirstOrDefault(orderItem => orderItem.Key == product);
        if (!orderItem.Equals(default(KeyValuePair<Product, uint>)))
        {
            int orderItemIndex = order.Products.IndexOf(orderItem);
            order.Products[orderItemIndex] = new KeyValuePair<Product, uint> (product, order.Products[orderItemIndex].Value + 1);
        }
        else
        {
            order.Products.Add(new KeyValuePair<Product, uint>(product, 1));
        }

        return order;
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

        Order orderC = orderA;
        foreach (KeyValuePair<Product, uint> productB in orderB.Products)
        {
            for (uint i = 0; i < productB.Value; i++)
            {
                Add(orderC, productB.Key);
            }
        }

        return orderC;
    }

    /// <summary>
    /// Удалить единицу товара из корзины.
    /// </summary>
    /// <param name="order">Корзина, из которой удаляется единица товара.</param>
    /// <param name="product">Удаляемый товар.</param>
    /// <returns>Карточка с удалённой единицей указанного товара.</returns>
    public Order Subtract(Order order, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        KeyValuePair<Product, uint> orderItem = order.Products.FirstOrDefault(orderItem => orderItem.Key == product);
        if (!orderItem.Equals(default(KeyValuePair<Product, uint>)))
        {
            int orderItemIndex = order.Products.IndexOf(orderItem);
            if (order.Products[orderItemIndex].Value == 1)
            {
                order.Products.RemoveAt(orderItemIndex);
            }
            else
            {
                order.Products[orderItemIndex] = new KeyValuePair<Product, uint>(product, order.Products[orderItemIndex].Value - 1);
            }
        }

        return order;
    }

    /// <summary>
    /// Удалить из первой корзины товары, которые есть во второй корзине.
    /// </summary>
    /// <param name="orderA">Исходная корзина.</param>
    /// <param name="orderB">Вторая корзина.</param>
    /// <returns>Новая исходная корзина без удалённых товаров.</returns>
    /// <exception cref="Exception">Исключение, если во второй корзине есть товары, которых нет в первой (исходной) корзине.</exception>
    public Order Substract(Order orderA, Order orderB)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);
        
        Order orderC = orderA;
        foreach (KeyValuePair<Product, uint> product in orderC.Products)
        {
            KeyValuePair<Product, uint> orderItem = orderC.Products.FirstOrDefault(orderItem => orderItem.Key == product.Key);
            if (!orderItem.Equals(default(KeyValuePair<Product, uint>)))
            {
                throw new Exception($"Продукт {product.Key.Name} не найден в первой корзине.");
            }
        }

        return orderC;
    }

    /// <summary>
    /// Удалить из корзины товары указанного типа.
    /// </summary>
    /// <param name="order">Исходная корзина.</param>
    /// <param name="product">Товар.</param>
    /// <returns>Новая исходная корзина без товаров указанного типа.</returns>
    public Order Divide(Order order, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = order;
        foreach (KeyValuePair<Product, uint> keyValuePair in newOrder.Products)
        {
            KeyValuePair<Product, uint> orderItem = newOrder.Products.FirstOrDefault(orderItem => orderItem.Key == product);
            if (keyValuePair.Key.GetType() == product.GetType())
            {
                int orderItemIndex = newOrder.Products.IndexOf(orderItem);
                order.Products.RemoveAt(orderItemIndex);
            }
        }

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
            int orderItemIndex = order.Products.IndexOf(orderItem);
            KeyValuePair<Product, uint> newOrderItem = new KeyValuePair<Product, uint>(orderItem.Key, orderItem.Value / number);
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
            int orderItemIndex = order.Products.IndexOf(orderItem);
            KeyValuePair<Product, uint> newOrderItem = new KeyValuePair<Product, uint>(orderItem.Key, orderItem.Value * number);
            newOrder.Products.Add(newOrderItem);
        }

        return newOrder;
    }
}
