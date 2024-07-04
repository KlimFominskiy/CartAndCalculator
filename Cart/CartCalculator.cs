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
    /// <param name="productOne">Первый товар.</param>
    /// <param name="productTwo">Второй товар.</param>
    /// <returns>Карточка, с созданными товарами.</returns>
    public Cart Add(Product productOne, Product productTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Cart cart = new();
        cart.Products.Add(productOne, cart.Products.GetValueOrDefault(productOne) + 1);

        return cart;
    }

    /// <summary>
    /// Добавить товар в корзину.
    /// </summary>
    /// <param name="cart">Корзина.</param>
    /// <param name="product">Добавляемый товар.</param>
    /// <returns>Карточка с добавленным товаром.</returns>
    public Cart Add(Cart cart, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        if (cart.Products.TryGetValue(product, out _))
        {
            cart.Products[product] += 1;
        }
        else
        {
            cart.Products.Add(product, 1);
        }

        return cart;
    }

    /// <summary>
    /// Добавить товар в корзину.
    /// </summary>
    /// <param name="cart">Корзина.</param>
    /// <param name="product">Добавляемый товар.</param>
    /// <returns>Карточка с добавленным товаром.</returns>
    public Cart Add(Product product, Cart cart)
    {
        return Add(cart, product);
    }

    /// <summary>
    /// Объединить корзины. 
    /// </summary>
    /// <param name="cartOne">Первая корзина.</param>
    /// <param name="cartTwo">Вторая корзина.</param>
    /// <returns>Объединённая корзина.</returns>
    public Cart Add(Cart cartOne, Cart cartTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach (KeyValuePair<Product, uint> productTwo in cartTwo.Products)
        {
            for (uint i = 0; i < productTwo.Value; i++)
            {
                Add(cartOne, productTwo.Key);
            }
        }

        return cartOne;
    }

    /// <summary>
    /// Удалить единицу товара из корзины.
    /// </summary>
    /// <param name="cart">Корзина, из которой удаляется единица товара.</param>
    /// <param name="product">Удаляемый товар.</param>
    /// <returns>Карточка с удалённой единицей указанного товара.</returns>
    public Cart Subtract(Cart cart, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        if (cart.Products.TryGetValue(product, out _))
        {
            if (--cart.Products[product] == 0)
            {
                cart.Products.Remove(product);
            }
        }

        return cart;
    }

    /// <summary>
    /// Удалить из первой корзины товары, которые есть во второй корзине.
    /// </summary>
    /// <param name="cartOne">Исходная корзина.</param>
    /// <param name="cartTwo">Вторая корзина.</param>
    /// <returns>исходная корзина без удалённых товаров.</returns>
    /// <exception cref="Exception">Исключение, если во второй корзине есть товары, которых нет в первой (исходной) корзине.</exception>
    public Cart Substract(Cart cartOne, Cart cartTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach(KeyValuePair<Product, uint> product in cartTwo.Products)
        {
            if (!cartOne.Products.Remove(product.Key))
            {
                throw new Exception($"Продукт {product.Key.Name} не найден в первой корзине.");
            }
        }

        return cartOne;
    }

    /// <summary>
    /// Удалить из корзины товары указанного типа.
    /// </summary>
    /// <param name="cart">Исходная корзина.</param>
    /// <param name="product">Товар.</param>
    /// <returns>Исходная корзина без товаров указанного типа.</returns>
    public Cart Divide(Cart cart, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach(KeyValuePair<Product, uint> keyValuePair in cart.Products)
        {
            if(keyValuePair.Key.GetType() == product.GetType())
            {
                cart.Products.Remove(keyValuePair.Key);
            }
        }

        return cart;
    }

    /// <summary>
    /// Уменьшить в карточке каждое количество товара в указанное число раз.
    /// </summary>
    /// <param name="cart">Исходная карточка.</param>
    /// <param name="number">Число, указывающее во сколько раз мы уменьшаем количество товара.</param>
    /// <returns>Карточка с уменьшенным количеством каждого товара.</returns>
    public Cart Divide(Cart cart, uint number)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach (KeyValuePair<Product, uint> keyValuePair in cart.Products)
        {
            if ((cart.Products[keyValuePair.Key] /= number) <= 0)
            {
                cart.Products.Remove(keyValuePair.Key);
            }
        }

        return cart;
    }

    /// <summary>
    /// Увеличить в карточке каждое количество товара в указанное число раз.
    /// </summary>
    /// <param name="cart">Исходная карточка.</param>
    /// <param name="number">Число, указывающее во сколько раз мы увеличиваем количество товара.</param>
    /// <returns>Карточка с увеличенным количеством каждого товара.</returns>
    public Cart Multiply(Cart cart, uint number)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach (KeyValuePair<Product, uint> keyValuePair in cart.Products)
        {
            cart.Products[keyValuePair.Key] *= number;
        }

        return cart;
    }
}
