using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


}
