using Newtonsoft.Json;

namespace Cart;

/// <summary>
/// Базовый класс продукта.
/// </summary>

public record Product : IComparable<Product>
{
    /// <summary>
    /// Id.
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Вес.
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Стоимость.
    /// </summary>
    public decimal Price { get; set; }

    [JsonConstructor]
    protected Product(uint id, string name, double weight, decimal price)
    {
        Id = id;
        Name = name;
        Weight = weight;
        Price = price;
    }

    public int CompareTo(Product? other)
    {
        return Id.CompareTo(other?.Id);
    }
}