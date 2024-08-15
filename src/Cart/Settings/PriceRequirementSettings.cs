namespace Cart.Settings;

/// <summary>
/// Варианты требования к цене.
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