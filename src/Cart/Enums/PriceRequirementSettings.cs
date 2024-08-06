namespace Cart.Enums;

/// <summary>
/// Настройки требования к цене.
/// </summary>
public enum PriceRequirementSettings
{
    /// <summary>
    /// Самая низкая цена.
    /// </summary>
    TheLowestValue = 1,
    /// <summary>
    /// Самая высокая цена.
    /// </summary>
    TheHighestValuem,
    /// <summary>
    /// Случайная цена.
    /// </summary>
    RandomValue,
}