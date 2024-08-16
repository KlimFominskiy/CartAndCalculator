using Cart.Products;
using System.Text.Json.Serialization;

namespace Cart;

/// <summary>
/// Базовый класс продукта.
/// </summary>
[Serializable]
[JsonDerivedType(typeof(Chips), typeDiscriminator: "Чипсы")]
[JsonDerivedType(typeof(Corvalol), typeDiscriminator: "Корвалол")]
[JsonDerivedType(typeof(WashingMachine), typeDiscriminator: "Стиральная машина")]
public abstract record Product
{
    [JsonPropertyName("Идентификационный номер")]
    /// <summary>
    /// Идентификационный номер.
    /// </summary>
    public uint Id { get; set; }

    [JsonPropertyName("Наименование")]
    /// <summary>
    /// Наименование.
    /// </summary>
    public string? Name { get; set; }

    [JsonPropertyName("Вес")]
    /// <summary>
    /// Вес.
    /// </summary>
    public double? Weight { get; set; }

    [JsonPropertyName("Цена")]
    /// <summary>
    /// Стоимость.
    /// </summary>
    public decimal? Price { get; set; }

    [JsonConstructor]
    protected Product(uint id, string? name, double? weight, decimal? price)
    {
        if (weight <= 0)
        {
            throw new Exception(message:$"Вес должен быть положительным. Текущий вес = {weight}.");
        }
        if (price <= 0)
        {
            throw new Exception(message: $"Цена должна быть положительной. Текущая цена = {price}.");
        }
        Id = id;
        Name = name;
        Weight = weight;
        Price = price;
    }

    public override string ToString()
    {
        return $"Идентификационный номер = {Id}\n" +
            $"Наименование = {Name}\n" +
            $"Вес = {Weight}\n" +
            $"Цена = {Price}";
    }
}