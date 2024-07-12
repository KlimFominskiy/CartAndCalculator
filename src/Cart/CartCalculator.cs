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

        Order newOrder = new();
        order.CopyTo(newOrder);
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

        Order orderC = new();
        orderA.CopyTo(orderC);
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
    /// Удалить единицу товара из корзины.
    /// </summary>
    /// <param name="order">Корзина, из которой удаляется единица товара.</param>
    /// <param name="product">Удаляемый товар.</param>
    /// <returns>Карточка с удалённой единицей указанного товара.</returns>
    public Order Subtract(Order order, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Order newOrder = new();
        order.CopyTo(newOrder);
        Dictionary<Product, uint> products = newOrder.Products.ToDictionary();
        if (products.ContainsKey(product))
        {
            if ((products[product] -= 1) == 0)
            {
                products.Remove(product);
            }
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
        
        Order orderC = new();
        orderA.CopyTo(orderC);
        Dictionary<Product, uint> products = orderC.Products.ToDictionary();
        foreach (Product product in orderB.Products.ToDictionary().Keys)
        {
            if(products.ContainsKey(product))
            {
                products.Remove(product);
            }
            else
            {
                throw new Exception($"Продукт {product.Name} с Id = {product.Id} не найден в первой корзине.");
            }
        }
        orderC.Products = products.ToList();

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

        Order newOrder = new();
        order.CopyTo(newOrder);
        foreach (KeyValuePair<Product, uint> orderItem in newOrder.Products)
        {
            if (orderItem.Key.GetType() == product.GetType())
            {
                newOrder.Products.Remove(orderItem);
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
        foreach (KeyValuePair<Product, uint> orderItem in newOrder.Products)
        {
            KeyValuePair<Product, uint> newOrderItem = new(orderItem.Key, orderItem.Value / number);
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
