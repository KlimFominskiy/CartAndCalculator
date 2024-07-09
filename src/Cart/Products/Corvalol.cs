using System.Text.Json.Serialization;

namespace Cart.Products;

public record Corvalol : Product
{
    public Corvalol(uint id, string name, double weight, decimal price) : base(id, name, weight, price)
    {
    }
}
