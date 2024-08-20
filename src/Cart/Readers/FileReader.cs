namespace Cart.Readers;

internal static class FileReader
{
    public static string ReadDataFromFile(string fullPathToFile)
    {
        string jsonString;
        while (true)
        {
            if (File.Exists(fullPathToFile))
            {
                jsonString = File.ReadAllText(fullPathToFile);
                break;
            }
            else
            {
                Console.WriteLine($"Считан путь: {fullPathToFile}.\n" +
                    $"Файл не найден. Повторите ввод.");
                continue;
            }
        }

        return jsonString;
    }
}
