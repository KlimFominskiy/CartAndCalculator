namespace Cart;

/// <summary>
/// Стиральная машина.
/// </summary>
public class WashingMachine : Product
{
    public bool? IsDryerIncluded { get; set; }

    public WashingMachine(uint id, string? name, double? weight, decimal? price, bool? isDryerIncluded) : base(id, name, weight, price)
    {
        IsDryerIncluded = isDryerIncluded;
    }

    public void CopyTo(WashingMachine other)
    {
        base.CopyTo(other);
        other.IsDryerIncluded = this.IsDryerIncluded;
    }
}
