using System.Reflection;

namespace Cart;

/// <summary>
/// Корзина (заказ) Интернет-магазина.
/// </summary>
public class Order
{
    /// <summary>
    /// Товары, добавленные в заказ. TKey - товар. TValue - количество товара.
    /// </summary>
    public List<KeyValuePair<Product, uint>> Products = new();

    /// <summary>
    /// Дата отправления заказа.
    /// </summary>
    public DateTime? TimeOfDeparture = null;

    /// <summary>
    /// Получить информацию о товарах в корзине.
    /// </summary>
    /// <returns>Информацию о товарах в корзине.</returns>
    public Dictionary<uint, Dictionary<object, string?>> GetInfo()
    {
        //TKey - номер товара в корзине, TValue - словарь свойств товара (полей класса товара).
        //TKey вложенного словаря - наименование свойства. TValue вложенное словаря - значение свойства.
        Dictionary<uint, Dictionary<object, string?>> cartInfo = new(); 
        uint productNumber = 0;
        foreach (KeyValuePair<Product, uint> product in Products)
        {
            productNumber += 1;
            //TKey - наименование свойства, значение свойства.
            Dictionary<object, string?> propertiesInfo = new();
            foreach (PropertyInfo propertyInfo in product.GetType().GetProperties())
            {
                propertiesInfo.Add(propertyInfo.Name, propertyInfo.GetValue(product)?.ToString());
            }

            cartInfo.Add(productNumber, propertiesInfo);
        }
         
        return cartInfo;
    }
}