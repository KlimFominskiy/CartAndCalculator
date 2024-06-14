using System.Reflection;

namespace Cart;

/// <summary>
/// Базовый класса товара.
/// </summary>
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
    public double Price { get; set; }
    /// <summary>
    /// Дата доставки.
    /// </summary>
    public DateTime TimeOfArrival { get; set; }
    protected Product(ulong id, string name, double weight, double price, DateTime timeOfArrival)
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