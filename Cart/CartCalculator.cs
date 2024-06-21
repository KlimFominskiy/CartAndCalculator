using Calculator;

namespace Cart;

public class CartCalculator : Calculator.Calculator
{
    public CartCalculator(Logger? logger) : base(logger)
    {

    }

    public Cart Add(Product productOne, Product productTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        Cart cart = new();
        cart.Products.Add(productOne, cart.Products.GetValueOrDefault(productOne) + 1);

        return cart;
    }

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

    public Cart Add(Product product, Cart cart)
    {
        return Add(cart, product);
    }

    public Cart Add(Cart cartOne, Cart cartTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach(KeyValuePair<Product, ulong> productTwo in cartTwo.Products)
        {
            for (ulong i = 0; i < productTwo.Value; i++)
            {
                Add(cartOne, productTwo.Key);
            }
        }

        return cartOne;
    }

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

    public Cart Substract(Cart cartOne, Cart cartTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach(KeyValuePair<Product, ulong> product in cartTwo.Products)
        {
            if (!cartOne.Products.Remove(product.Key))
            {
                throw new Exception($"Продукт {product.Key.Name} не найден в первой корзине.");
            }
        }

        return cartOne;
    }

    public Cart Divide(Cart cart, Product product)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach(KeyValuePair<Product, ulong> keyValuePair in cart.Products)
        {
            if(keyValuePair.Key.GetType() == product.GetType())
            {
                cart.Products.Remove(keyValuePair.Key);
            }
        }

        return cart;
    }

    public Cart Divide(Cart cart, uint number)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach (KeyValuePair<Product, ulong> keyValuePair in cart.Products)
        {
            if ((cart.Products[keyValuePair.Key] /= number) <= 0)
            {
                cart.Products.Remove(keyValuePair.Key);
            }
        }

        return cart;
    }

    public Cart Multiply(Cart cart, uint number)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        foreach (KeyValuePair<Product, ulong> keyValuePair in cart.Products)
        {
            cart.Products[keyValuePair.Key] *= number;
        }

        return cart;
    }
}
