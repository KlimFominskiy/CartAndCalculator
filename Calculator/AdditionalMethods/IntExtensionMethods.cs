namespace Calculator.Methods;

/// <summary>
/// Методы расширения типа int.
/// </summary>
public static class IntExtensionMethods
{
    /// <summary>
    /// Поле для генерации случайного числа.
    /// </summary>
    private static Random random = new Random();

    /// <summary>
    /// Генерация случайного целого чётного числа.
    /// </summary>
    /// <param name="randomInt">Тип, с которым работаем. В данном случае это int.</param>
    /// <returns>Сгенерированное число.</returns>
    public static int GenerateRandomEvenInt(this int randomInt)
    {
        do
        {
            randomInt = new Random().Next(int.MinValue, int.MaxValue);
        }
        while (randomInt % 2 != 0);

        return randomInt;
    }

    /// <summary>
    /// Генерация случайного целого нечётного числа.
    /// </summary>
    /// <param name="randomInt">Тип, с которым работаем. В данном случае это int.</param>
    /// <returns>Сгенерированное число.</returns>
    public static int GenerateRandomOddInt(this int randomInt)
    {
        do
        {
            randomInt = random.Next(int.MinValue, int.MaxValue);
        }
        while (randomInt % 2 == 0);

        return randomInt;
    }

    /// <summary>
    /// Генерация случайного целого положительного числа. 
    /// </summary>
    /// <param name="randomInt">Тип, с которым работаем. В данном случае это int.</param>
    /// <returns>Сгенерированное число.</returns>
    public static int GenerateRandomPositiveInt(this int randomInt)
    {
        do
        {
            randomInt = random.Next(int.MinValue, int.MaxValue);
        }
        while (randomInt % 2 <= 0);

        return randomInt;
    }

    /// <summary>
    /// Генерация случайного целого негативного числа.
    /// </summary>
    /// <param name="randomInt">Тип, с которым работаем. В данном случае это int.</param>
    /// <returns>Сгенерированное число.</returns>
    public static int GenerateRandomNegativeInt(this int randomInt)
    {
        do
        {
            randomInt = random.Next(int.MinValue, int.MaxValue);
        }
        while (randomInt % 2 >= 0);

        return randomInt;
    }
}
