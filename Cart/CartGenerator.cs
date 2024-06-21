using Cart.Products;

namespace Cart;

public static class CartGenerator
{
    static ulong productId = 0;

    public static Cart GenerateOrder()
    {
        Cart cart = new();
        Random random = new();

        for (int i = 0; i < random.Next(0, 3); i++)
        {
            Corvalol corvarol = new(
                id: productId += 1,
                name: "Корвалол-" + productId.ToString(),
                weight: random.NextDouble() * (30 - 15) + 15,
                price: new decimal(random.NextDouble() * (1000 - 500) + 500),
                timeOfArrival: DateTime.Now.AddDays(random.Next(0, 10))
                );
            cart.Products.Add(corvarol, 1);
        }
        for (int i = 0; i < random.Next(0, 3); i++)
        {
            WashingMachine washingMachine = new(
                id: productId += 1,
                name: "Стиральная машина-" + productId.ToString(),
                weight: random.NextDouble() * (30 - 15) + 15,
                price: new decimal(random.NextDouble() * (1000 - 500) + 500),
                timeOfArrival: DateTime.Now.AddDays(random.Next(0, 10)),
                isDryerIncluded: random.Next(0, 2) == 0 ? false : true
                );
            cart.Products.Add(washingMachine, 1);
        }
        for (int i = 0; i < random.Next(0, 3); i++)
        {
            Chips chips = new(
                id: productId += 1,
                name: "Чипсы-" + productId.ToString(),
                weight: random.NextDouble() * (30 - 15) + 15,
                price: new decimal(random.NextDouble() * (1000 - 500) + 500),
                timeOfArrival: DateTime.Now.AddDays(random.Next(0, 10))
                );
            cart.Products.Add(chips, 1);
        }

        return cart;
    }
}
