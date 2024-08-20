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
    public static string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

    /// <summary>
    /// Файл по умолчанию со списком сгенерированных товаров.
    /// </summary>
    public static string productsFileNameDefault = "Products.json";

    /// <summary>
    /// Файл по умолчанию со списком сгенерированных заказов.
    /// </summary>
    public static string ordersFileNameDefault = "Orders.json";

    /// <summary>
    /// Файл по умолчанию с заказом, введённым и сохранённым пользователем.
    /// </summary>
    public static string orderFileNameDefault = "Order.json";

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
