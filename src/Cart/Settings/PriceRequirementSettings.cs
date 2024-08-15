namespace Cart.Settings;

/// <summary>
/// Настройки требования к цене.
/// </summary>
public enum PriceRequirementSettings
{
    /// <summary>
    /// Случайная цена.
    /// </summary>
    RandomValue,
    /// <summary>
    /// Самая низкая цена.
    /// </summary>
    TheLowestValue,
    /// <summary>
    /// Самая высокая цена.
    /// </summary>
    TheHighestValue,
}