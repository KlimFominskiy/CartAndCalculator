using Cart.Products;
using System.Text.Json.Serialization;

namespace Cart;

/// <summary>
/// Базовый класс продукта.
/// </summary>
[Serializable]
[JsonDerivedType(typeof(Product), typeDiscriminator: "Product")]
[JsonDerivedType(typeof(Chips), typeDiscriminator: "Чипсы")]
[JsonDerivedType(typeof(Corvalol), typeDiscriminator: "Корвалол")]
[JsonDerivedType(typeof(WashingMachine), typeDiscriminator: "Стиральная машина")]
public class Product
{
    /// <summary>
    /// Идентификационный номер.
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Вес.
    /// </summary>
    public double? Weight { get; set; }

    /// <summary>
    /// Стоимость.
    /// </summary>
    public decimal? Price { get; set; }

    [JsonConstructor]
    protected Product(uint id, string? name, double? weight, decimal? price)
    {
        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), message:"Вес должен быть положительным.");
        }
        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), message: "Цена должна быть положительной.");
        }
        Id = id;
        Name = name;
        Weight = weight;
        Price = price;
    }
}