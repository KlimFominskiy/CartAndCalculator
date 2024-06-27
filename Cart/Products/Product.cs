using Cart.Products;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Cart;

/// <summary>
/// Базовый класса товара.
/// </summary>
[JsonDerivedType(typeof(WashingMachine), typeDiscriminator: "washingMachine")]
[JsonDerivedType(typeof(Corvalol), typeDiscriminator: "corvalol")]
[JsonDerivedType(typeof(Chips), typeDiscriminator: "chips")]
public abstract class Product : IComparable<Product>
{
    /// <summary>
    /// Id.
    /// </summary>
    public ulong Id { get; set; } // Is guid better?
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Вес.
    /// </summary>
    public double Weight { get; set; }
    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Дата доставки.
    /// </summary>
    public DateTime TimeOfArrival { get; set; }

    [JsonConstructor]
    protected Product(ulong id, string name, double weight, decimal price, DateTime timeOfArrival)
    {
        Id = id;
        Name = name;
        Weight = weight;
        Price = price;
        TimeOfArrival = timeOfArrival;
    }

    public int CompareTo(Product? other)
    {
        return Name.CompareTo(other?.Name);
    }
}