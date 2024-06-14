namespace Cart;

/// <summary>
/// Базовый класса товара.
/// </summary>
public abstract class Product
{
    /// <summary>
    /// Id.
    /// </summary>
    public uint Id { get; set; }
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
    public abstract double Price { get; set; }
    /// <summary>
    /// Дата доставки.
    /// </summary>
    public abstract double TimeOfArrival { get; set; }
}