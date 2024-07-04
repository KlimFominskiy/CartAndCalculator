namespace Cart.Products;
public record Corvalol : Product
{
    public Corvalol(uint id, string name, double weight, decimal price, DateTime timeOfArrival) : base(id, name, weight, price, timeOfArrival)
    {
    }
}
