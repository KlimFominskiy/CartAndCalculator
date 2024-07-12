﻿using C_sharp_course.UnitTests;
using C_sharp_course.UnitTests.Data.Cart;
using static NUnit.Framework.Internal.OSPlatform;

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
            Assert.That(productsList, Does.Contain(product));
        }
        foreach (Product product in orderWithOneProduct.Products.ToDictionary().Keys)
        {
            Assert.That(productsList, Does.Contain(product));
        }
    }

    [Test(Description = "Проверка успешного объединения двух продуктов в заказ.")]
    public void Add_ValidProducts_NewOrder()
    {
        Product productA = orderWithThreeProducts.Products[0].Key;
        Product productB = orderWithThreeProducts.Products[1].Key;
        Order newOrder = cartCalculator.Add(productA, productB);
        List<Product> productsList = newOrder.Products.ToDictionary().Keys.ToList();
        Assert.That(productsList, Does.Contain(productA));
        Assert.That(productsList, Does.Contain(productB));
    }

    [Test(Description = "Проверка успешного удаления единицы товара из заказа.")]
    public void Subtract_ValidProduct_NewOrderWithDeletedProduct()
    {
        Product product = orderWithThreeProducts.Products.First().Key;
        uint productNumber = orderWithThreeProducts.Products.First().Value;
        Order newOrder = cartCalculator.Subtract(orderWithThreeProducts, product);
        if (productNumber == 1)
        {
            Assert.That(newOrder.Products.ToDictionary().ContainsKey(product), Is.False);
        }
        else
        {
            Assert.That(newOrder.Products.ToDictionary()[product], Is.EqualTo(productNumber - 1));
        }
    }

    [Test(Description = "Проверка успешного удаления из первой корзины товаров, которые есть во второй корзине.")]
    public void Subtract_ValidOrder_NewOrderWithoutDeletedProducts()
    {
        Order orderB = new();
        orderB.Products.Add(orderWithThreeProducts.Products[0]);
        orderB.Products.Add(orderWithThreeProducts.Products[1]);
        Order newOrder = cartCalculator.Subtract(orderWithThreeProducts, orderB);
        foreach (KeyValuePair<Product, uint> orderItem in orderB.Products)
        {
            Assert.That(newOrder.Products.Contains(orderItem), Is.False);
        }
    }

    [Test(Description = "Проверка успешного удаления из заказа товаров определённого типа.")]
    public void Divide_ValidProduct_NewOrderWithoutDeletedProduct()
    {
        Type productType = orderWithThreeProducts.Products.First().Key.GetType();
        Order newOrder = cartCalculator.Divide(orderWithThreeProducts, productType);
        foreach(Product product in newOrder.Products.ToDictionary().Keys)
        {
            Assert.That(product.GetType() == productType, Is.False);
        }
    }

    [Test(Description = "Проверка уменьшения количества каждого товара в корзине в определённое количество раз.")]
    public void Divide_ValidOrder_NewOrderWithReducedProductsNumber()
    {
        uint number = 3;
        Order newOrder = cartCalculator.Divide(orderWithThreeProducts, number);
        for (int i = 0; i < newOrder.Products.Count(); i++)
        {
            Assert.That(orderWithThreeProducts.Products[i].Value / 3, Is.EqualTo(newOrder.Products[i].Value));
        }
    }

    [Test(Description = "Проверка увеличения количества каждого товара в корзине в определённое количество раз.")]
    public void Multiply_ValidOrder_NewOrderWithIncreasedProductsNumber()
    {
        uint number = 3;
        Order newOrder = cartCalculator.Multiply(orderWithThreeProducts, number);
        for (int i = 0; i < newOrder.Products.Count(); i++)
        {
            Assert.That(orderWithThreeProducts.Products[i].Value * 3, Is.EqualTo(newOrder.Products[i].Value));
        }
    }
}