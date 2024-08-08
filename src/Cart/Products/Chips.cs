namespace Cart.Products;

/// <summary>
/// Чипсы.
/// </summary>
public record Chips : Product
{
    public Chips(uint id, string? name, double? weight, decimal? price) : base(id, name, weight, price)
    {
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
