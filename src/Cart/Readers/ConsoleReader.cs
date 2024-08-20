using Cart.Settings;
using Cart.Stores;

namespace Cart.Readers;

/// <summary>
/// Класс содержит методы считывания различных типов данных из консоли.
/// </summary>
public static class ConsoleReader
{
    /// <summary>
    /// Считать decimal из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static decimal ReadDecimalFromConsole()
    {
        while (true)
        {
            string userInput = Console.ReadLine();
            if (decimal.TryParse(userInput, out decimal value))
            {
                return value;
            }
            else
            {
                ConsoleInputError(userInput);
                Console.WriteLine("Повторите ввод.");
                continue;
            }
        }
    }

    /// <summary>
    /// Считать uint из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static uint ReadUintFromConsole()
    {
        while (true)
        {
            string userInput = Console.ReadLine();
            if (uint.TryParse(userInput, out uint value))
            {
                return value;
            }
            else
            {
                ConsoleInputError(userInput);
                Console.WriteLine("Повторите ввод.");
                continue;
            }
        }
    }

    /// <summary>
    /// Считать int из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static int ReadIntFromConsole()
    {
        while (true)
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int value))
            {
                return value;
            }
            else
            {
                ConsoleInputError(userInput);
                Console.WriteLine("Повторите ввод.");
                continue;
            }
        }
    }

    /// <summary>
    /// Считать ProgramModes из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static ProgramModes ReadProgramModeFromConsole()
    {
        while (true)
        {
            string userInput = Console.ReadLine();
            if (Enum.TryParse(userInput, out ProgramModes value))
            {
                if (!IsModeDefined(value))
                {
                    Console.WriteLine("Повторите ввод.");
                    continue;
                }
                return value;
            }
            else
            {
                ConsoleInputError(userInput);
                Console.WriteLine("Повторите ввод.");
                continue;
            }
        }
    }

    /// <summary>
    /// Считать DateTime из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static DateTime ReadDateFromConsole()
    {
        while (true)
        {
            string userInput = Console.ReadLine();
            if (DateTime.TryParse(userInput, out DateTime value))
            {
                return value;
            }
            else
            {
                ConsoleInputError(userInput);
                Console.WriteLine("Повторите ввод.");
                continue;
            }
        }
    }

    /// <summary>
    /// Считать PriceRequirementSettings из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static PriceRequirementSettings ReadPriceRequirementFromConsole()
    {
        while (true)
        {
            string orderItemSetting = Console.ReadLine();
            if (Enum.TryParse(orderItemSetting, out PriceRequirementSettings setting))
            {
                if (Enum.IsDefined(typeof(PriceRequirementSettings), setting))
                {
                    return setting;
                }
                else
                {
                    Console.WriteLine($"Введено {setting}. Такой настройки требования нет. Повторите ввод.");
                    continue;
                }
            }
            else
            {
                Console.WriteLine($"Введено {orderItemSetting}. Неправильный формат. Повторите ввод.");
            }
        }
    }

    /// <summary>
    /// Вывод в консоль сообщения о неправильном формате введённого значения.
    /// </summary>
    /// <param name="value">Считанное значение.</param>
    private static void ConsoleInputError(string value)
    {
        Console.WriteLine($"Введено {value}. Неправильный формат.");
    }

    /// <summary>
    /// Проверка существования выбранного режима работы программы.
    /// </summary>
    /// <param name="mode">Введённое значение режима.</param>
    /// <returns>true - режим существует. Иначе - false.</returns>
    private static bool IsModeDefined(ProgramModes mode)
    {
        if (Enum.IsDefined(typeof(ProgramModes), mode))
        {
            return true;
        }
        else
        {
            Console.WriteLine($"Введено {(int)mode}. Такого режима нет.");
            return false;
        }
    }

    /// <summary>
    /// Считать номер типа продукта из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static uint ReadProductTypeNumberFromConsole()
    {
        while (true)
        {
            string orderItemSetting = Console.ReadLine();
            if (uint.TryParse(orderItemSetting, out uint productTypeNumber))
            {
                if (productTypeNumber > Store.ProductsTypes.Count)
                {
                    Console.WriteLine($"Введён тип {productTypeNumber}. Такого типа товара нет в списке. Повторите ввод.");
                    continue;
                }
                return productTypeNumber;
            }
            else
            {
                Console.WriteLine($"Введено {orderItemSetting}. Неправильный формат. Повторите ввод.");
                continue;
            }
        }
    }

    /// <summary>
    /// Считать номер продукта из консоли.
    /// </summary>
    /// <returns>Считанное значение.</returns>
    public static uint ReadProductNumberFromConsole()
    {
        while (true)
        {
            string orderItemSetting = Console.ReadLine();
            if (uint.TryParse(orderItemSetting, out uint productNumber))
            {
                if (productNumber > Store.Products.Count)
                {
                    Console.WriteLine($"Введено {productNumber}. Такого типа товара нет в списке. Повторите ввод.");
                    continue;
                }
                return productNumber;
            }
            else
            {
                Console.WriteLine($"Введено {orderItemSetting}. Неправильный формат. Повторите ввод.");
                continue;
            }
        }
    }

    public static string ReadFullFileNameFromConsole(string fileNameDefault)
    {
        Console.WriteLine("Введите полное имя файла (путь к файлу + имя файла). Нажмите enter для использования файла по умолчанию");
        if (string.IsNullOrEmpty(fileNameDefault))
        {
            throw new ArgumentNullException("Файл по умолчанию не задан");
        }

        while (true)
        {
            string fullPathToFile;
            string fileName = Console.ReadLine();
            if (string.IsNullOrEmpty(fileName))
            {
                fullPathToFile = ProgramSettings.ProjectPath + Path.DirectorySeparatorChar + fileNameDefault;
            }
            else
            {
                fullPathToFile = ProgramSettings.ProjectPath + Path.DirectorySeparatorChar + fileName;
            }

            return fullPathToFile;
        }
    }
}
