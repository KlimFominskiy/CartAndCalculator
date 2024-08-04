using Cart.Enums;
using Cart.Stores;

namespace Cart;

/// <summary>
/// Класс содержит методы считывания типов из консоли.
/// </summary>
public static class ReadTypesFromConsole
{
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

    private static void ConsoleInputError(string value)
    {
        Console.WriteLine($"Введено {value}. Неправильный формат.");
    }

    private static bool IsModeDefined(ProgramModes value)
    {
        if (Enum.IsDefined(typeof(ProgramModes), value))
        {
            return true;
        }
        else
        {
            Console.WriteLine($"Введено {(int)value}. Такого режима нет.");
            return false;
        }
    }

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
}
