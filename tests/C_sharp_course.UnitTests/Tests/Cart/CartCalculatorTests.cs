using System.Reflection;

namespace Cart.UnitTests;

public class Tests
{
    private Product chips;
    private Product corvalol;
    private Product washingMachine;
    private Order emptyOrder;
    private Order orderWithOneProduct;
    private Order orderWithThreeProducts;

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}