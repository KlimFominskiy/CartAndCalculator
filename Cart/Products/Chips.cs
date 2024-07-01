using System.Text.Json.Serialization;

namespace Cart.Products;

public record Chips : Product
{
    public Chips(ulong id, string name, double weight, decimal price, DateTime timeOfArrival) : base(id, name, weight, price, timeOfArrival)
    {
    }
}
