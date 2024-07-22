namespace Cart.Products;

/// <summary>
/// Корвалол.
/// </summary>
public record Corvalol : Product
{
    public Corvalol(uint id, string? name, double? weight, decimal? price) : base(id, name, weight, price)
    {
    }
}
