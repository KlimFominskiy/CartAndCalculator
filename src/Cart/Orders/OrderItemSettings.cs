using Cart.Settings;

namespace Cart.Orders;

/// <summary>
/// Класс настройки требований к продукту при его поиске.
/// </summary>
internal class OrderItemSettings
{
    /// <summary>
    /// Номер типа товара в списке типов товаров.
    /// </summary>
    public uint ProductTypeNumber { get; set; }

    /// <summary>
    /// Количество товара.
    /// </summary>
    public uint QuantityOfProduct { get; set; }

    /// <summary>
    /// Требование к цене.
    /// </summary>
    public PriceRequirementSettings PriceRequirement { get; set; }
}
