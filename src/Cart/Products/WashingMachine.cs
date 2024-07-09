using System.Text.Json.Serialization;

namespace Cart;

/// <summary>
/// Стиральная машина.
/// </summary>
public record WashingMachine : Product
{
    public bool IsDryerIncluded { get; set; }

    public WashingMachine(uint id, string name, double weight, decimal price, bool isDryerIncluded) : base(id, name, weight, price)
    {
        IsDryerIncluded = isDryerIncluded;
    }
}
