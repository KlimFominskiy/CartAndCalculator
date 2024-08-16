using System.Text.Json.Serialization;

namespace Cart;

/// <summary>
/// Стиральная машина.
/// </summary>
public record WashingMachine : Product
{
    [JsonPropertyName("Наличие сушилки")]
    public bool? IsDryerIncluded { get; set; }

    public WashingMachine(uint id, string? name, double? weight, decimal? price, bool? isDryerIncluded) : base(id, name, weight, price)
    {
        IsDryerIncluded = isDryerIncluded;
    }

    public override string ToString()
    {
        return base.ToString() + $"\nНаличие сушилки = Есть.";
    }
}
