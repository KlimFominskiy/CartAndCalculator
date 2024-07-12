using C_sharp_course.UnitTests;
using C_sharp_course.UnitTests.Data.Cart;

namespace Cart.UnitTests;

public class Tests
{
    private CartCalculator cartCalculator = new(null);
    private Order emptyOrder;
    private Order orderWithOneProduct;
    private Order orderWithThreeProducts;

    [SetUp]
    public void Setup()
    {
        BuildProducts buildProducts = new();
        BuildOrders buildOrders = new();
        emptyOrder = buildOrders.GetOrder(0);
        orderWithOneProduct = buildOrders.GetOrder(1);
        orderWithThreeProducts = buildOrders.GetOrder(3);
    }

    [Test(Description = "Проверка успешного объединения заказов.")]
    public void Add_ValidOrders_NewMergedOrder()
    {
        Order newOrder = cartCalculator.Add(orderWithOneProduct, orderWithThreeProducts);
        List<Product> productsList = newOrder.Products.ToDictionary().Keys.ToList();
        foreach (Product product in orderWithThreeProducts.Products.ToDictionary().Keys)
        {
            Assert.Contains(product, productsList);
        }
        foreach (Product product in orderWithOneProduct.Products.ToDictionary().Keys)
        {
            Assert.Contains(product, productsList);
        }
    }

    [Test(Description = "Проверка успешного объединения двух продуктов в заказ.")]
    public void Add_ValidProducts_NewOrder()
    {
        Product productA = orderWithThreeProducts.Products[0].Key;
        Product productB = orderWithThreeProducts.Products[1].Key;
        Order newOrder = cartCalculator.Add(productA, productB);
        List<Product> productsList = newOrder.Products.ToDictionary().Keys.ToList();
        Assert.Contains(productA, productsList);
        Assert.Contains(productB, productsList);
    }

    [Test(Description = "Проверка успешного удаления единицы товара из заказа.")]
    public void Subtract_ValidProduct_NewOrderWithDeletedProduct()
    {
        Product product = orderWithThreeProducts.Products.First().Key;
        uint productNumber = orderWithThreeProducts.Products.First().Value;
        Order newOrder = cartCalculator.Subtract(orderWithThreeProducts, product);
        // Или нет объекта или количесто уменьшилось на единицу.
        if (productNumber == 1)
        {
            Assert.False(newOrder.Products.ToDictionary().ContainsKey(product));
        }
        else
        {
            Assert.That(newOrder.Products.ToDictionary()[product], Is.EqualTo(productNumber - 1));
        }
    }

    [Test(Description = "Проверка успешного удаления из первой корзины товаров, которые есть во второй корзине.")]
    public void Subtract_ValidOrder_NewOrderWithDeletedProducts()
    {
        Order orderB = new();
        orderB.Products.Add(orderWithThreeProducts.Products[0]);
        orderB.Products.Add(orderWithThreeProducts.Products[1]);
        Order newOrder = cartCalculator.Subtract(orderWithThreeProducts, orderB);
        foreach (KeyValuePair<Product, uint> orderItem in orderB.Products)
        {
            Assert.False(newOrder.Products.Contains(orderItem));
        }
    }
}