using Cart;
using Cart.Products;

namespace C_sharp_course.UnitTests;

internal class BuildProducts
{
    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        uint productId = 0;
        Random random = new();

        for (uint i = 0; i < 5; i++)
        {
            Corvalol corvarol = new(
                id: productId += 1,
                name: "Корвалол-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId)
                );
            WashingMachine washingMachine = new(
                id: productId += 1,
                name: "Стиральная машина-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId),
                isDryerIncluded: random.Next(0, 2) == 0 ? false : true
                );
            Chips chips = new(
                id: productId += 1,
                name: "Чипсы-" + productId.ToString(),
                weight: GetWeight(productId),
                price: GetPrice(productId)
                );

            products.Add(corvarol);
            products.Add(washingMachine);
            products.Add(chips);
        }

        return products;
    }

    private double GetWeight(uint ProductId)
    {
        Random random = new();
        return Double.Round(random.NextDouble() * (30 - 15) + 15, 2);
    }

    private decimal GetPrice(uint ProductId)
    {
        Random random = new();

        return Decimal.Round((decimal)(random.NextDouble() * (1000 - 500) + 500), 2);
    }
}
