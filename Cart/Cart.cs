using System.Reflection;

namespace Cart;

/// <summary>
/// Корзина Интернет-магазина.
/// </summary>
public class Cart
{
    public Dictionary<Product, ulong> Products = new();

    /// <summary>
    /// Рассчитать и получить общий вес покупок в корзине.
    /// </summary>
    /// <returns>Общий вес покупок в корзине.</returns>
    public double GetTotalWeight()
    {
        double totalWeight = 0;

        foreach (KeyValuePair<Product, ulong> product in Products)
        {
            totalWeight += product.Key.Weight;
        }
        
        return totalWeight;
    }

    /// <summary>
    /// Рассчитать и получить сумму покупок в корзине.
    /// </summary>
    /// <returns>Сумма покупок в корзине.</returns>
    public decimal GetTotalPrice()
    {
        decimal totalPrice = 0;

        foreach (KeyValuePair<Product, ulong> product in Products)
        {
            totalPrice += product.Key.Price;
        }

        return totalPrice;
    }

    /// <summary>
    /// Получить информацию о товарах в корзине.
    /// </summary>
    /// <returns>Информацию о товарах в корзине.</returns>
    public Dictionary<ulong, Dictionary<object, string?>> GetInfo()
    {
        //TKey - номер товара в корзине, TValue - словарь качеств товара.
        //TKey вложенного словаря - наименоване свойства. TValue вложенное словаря - значение свойства.
        Dictionary<ulong, Dictionary<object, string?>> cartInfo = new(); 
        uint productNumber = 0;
        foreach (KeyValuePair<Product, ulong> product in Products)
        {
            productNumber += 1;
            Dictionary<object, string?> propertiesInfo = new(); //TKey - наименоване свойства, значение свойства.
            foreach (PropertyInfo propertyInfo in product.GetType().GetProperties())
            {
                propertiesInfo.Add(propertyInfo.Name, propertyInfo.GetValue(product)?.ToString());
            }

            cartInfo.Add(productNumber, propertiesInfo);
        }
         
        return cartInfo;
    }
}