using System.Text.Json.Serialization;

namespace Cart;

/// <summary>
/// Стиральная машина.
/// </summary>
public record WashingMachine : Product
{
    public bool IsDryerIncluded { get; set; }

    public WashingMachine(ulong id, string name, double weight, decimal price, DateTime timeOfArrival, bool isDryerIncluded) : base(id, name, weight, price, timeOfArrival)
    {
        IsDryerIncluded = isDryerIncluded;
    }
}
