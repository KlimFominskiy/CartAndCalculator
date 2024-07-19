using Cart;
using Cart.Orders;
using System;

namespace C_sharp_course.UnitTests.Data.Cart;

internal class BuildOrders
{
    public Order GetOrder(uint productsNumber)
    {
        Random random = new Random();
        Order order = new();
        BuildProducts buildProducts = new BuildProducts();
        List<Product> products = buildProducts.GetProducts();
        for (int i = 0; i < productsNumber; i++)
        {
            int randomProductNumber = random.Next(0, products.Count);
            order.Products.Add(new KeyValuePair<Product, uint>(products[randomProductNumber], Convert.ToUInt32(random.Next(1, 4))));
            products.RemoveAt(randomProductNumber);
        }

        return order;
    }
}
