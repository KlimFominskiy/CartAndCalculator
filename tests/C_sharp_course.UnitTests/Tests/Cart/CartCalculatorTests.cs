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

    [Test(Description = "Проверка объединения двух продуктов в заказ.")]
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
    public void Subtract_ValidProduct_NewOrderWithNewProduct()
    {
        Product product = orderWithThreeProducts.Products.First().Key;
        Order newOrder = cartCalculator.Subtract(orderWithThreeProducts, product);
        List<Product> productsList = newOrder.Products.ToDictionary().Keys.ToList();
        // Или нет объекта или количесто уменьшилось на единицу.
        Assert.False(productsList.Contains(product));
    }
}