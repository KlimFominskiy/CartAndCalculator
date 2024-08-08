namespace Cart.Orders;

/// <summary>
/// Корзина (заказ) Интернет-магазина.
/// </summary>
[Serializable]
public class Order
{
    /// <summary>
    /// Товары в заказе. TKey - товар. TValue - количество товара.
    /// </summary>
    public List<KeyValuePair<Product, uint>> Products = new();

    /// <summary>
    /// Дата отправки заказа заказа.
    /// </summary>
    public DateTime? TimeOfDeparture = null;
}