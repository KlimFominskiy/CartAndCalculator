using System.Text.Json;

namespace Cart.Settings;

/// <summary>
/// Настройки работы приложения.
/// </summary>
internal static class ProgramSettings
{
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
