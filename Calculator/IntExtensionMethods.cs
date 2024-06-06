namespace Calculator;

public static class IntExtensionMethods
{
    private static Random random = new Random();

    public static int GenerateRandomEvenInt(this int randomInt)
    {
        do {
            randomInt = new Random().Next(int.MinValue, int.MaxValue);
        } while (randomInt % 2 != 0);

        return randomInt;
    }

    public static int GenerateRandomOddInt(this int randomInt)
    {
        do {
            randomInt = random.Next(int.MinValue, int.MaxValue);
        } while (randomInt % 2 == 0);

        return randomInt;
    }

    public static int GenerateRandomPositiveInt(this int randomInt)
    {
        do {
            randomInt = random.Next(int.MinValue, int.MaxValue);
        } while (randomInt % 2 <= 0);

        return randomInt;
    }

    public static int GenerateRandomNegativeInt(this int randomInt)
    {
        do {
            randomInt = random.Next(int.MinValue, int.MaxValue);
        } while (randomInt % 2 >= 0);

        return randomInt;
    }
}
