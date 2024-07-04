using System.Reflection;

namespace Cart;

/// <summary>
/// Корзина (заказ) Интернет-магазина.
/// </summary>
public class Cart
{
    /// <summary>
    /// Товары, добавленные в карточку. TKey - товар. TValue - количество товара.
    /// </summary>
    public Dictionary<Product, uint> Products = new();

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
            Dictionary<object, string?> propertiesInfo = new(); //TKey - наименование свойства, значение свойства.
            foreach (PropertyInfo propertyInfo in product.GetType().GetProperties())
            {
                propertiesInfo.Add(propertyInfo.Name, propertyInfo.GetValue(product)?.ToString());
            }

            cartInfo.Add(productNumber, propertiesInfo);
        }
         
        return cartInfo;
    }
}