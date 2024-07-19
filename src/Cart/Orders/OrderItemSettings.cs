using Cart.Enums;

namespace Cart.Orders;

/// <summary>
/// Класс настройки требований к продукту при его поиске.
/// </summary>
internal class OrderItemSettings
{
    /// <summary>
    /// Номер товара в списке продуктов.
    /// </summary>
    public uint ProductTypeNumber { get; set; }

    /// <summary>
    /// Количество товара.
    /// </summary>
    public uint ProductQuantity { get; set; }

    /// <summary>
    /// Требование к цене.
    /// </summary>
    public PriceRequirementSettings PriceRequirement { get; set; }
}
