using System.Text.Json;

namespace Cart.Settings;

/// <summary>
/// Основные параметры приложения приложения.
/// </summary>
internal static class ProgramSettings
{
    /// <summary>
    /// Путь к директории проекта.
    /// </summary>
    public static string ProjectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

    /// <summary>
    /// Файл по умолчанию со списком сгенерированных товаров.
    /// </summary>
    public static string ProductsFileNameDefault = "Products.json";

    /// <summary>
    /// Файл по умолчанию со списком сгенерированных заказов.
    /// </summary>
    public static string OrdersFileNameDefault = "Orders.json";

    /// <summary>
    /// Файл по умолчанию с заказом, введённым и сохранённым пользователем.
    /// </summary>
    public static string OrderFileNameDefault = "Order.json";

    /// <summary>
    /// Настройки сеариализации.
    /// </summary>
    public static JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        IncludeFields = true,
    };
}
