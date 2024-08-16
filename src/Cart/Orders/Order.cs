using System.Text.Json.Serialization;

namespace Cart.Orders;

/// <summary>
/// Корзина (заказ) Интернет-магазина.
/// </summary>
[Serializable]
public class Order
{
    [JsonPropertyName("Состав заказа")]
    /// <summary>
    /// Товары в заказе. TKey - товар. TValue - количество товара.
    /// </summary>
    public List<KeyValuePair<Product, uint>> Products = new();

    [JsonPropertyName("Дата отправки заказа")]
    /// <summary>
    /// Дата отправки заказа заказа.
    /// </summary>
    public DateTime? TimeOfDeparture = null;
}