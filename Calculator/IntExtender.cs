namespace Calculator;

public class IntExtender
{
    private Random random = new Random();

    public void ChooseNumberToGenerate()
    {
        Console.WriteLine("Выбран режим генерации целого числа.");
        while (true) {
            Console.WriteLine("Какое число сгенерировать?");
            Console.WriteLine("1 - чётное.");
            Console.WriteLine("2 - нечётное.");
            Console.WriteLine("3 - положительное.");
            Console.WriteLine("4 - отрицательное.");
            switch (Console.ReadLine()) {
                case "1":
                    Console.WriteLine(GenerateRandomEvenInt());
                    break;
                case "2":
                    Console.WriteLine(GenerateRandomOddInt());
                    break;
                case "3":
                    Console.WriteLine(GenerateRandomPositiveInt());
                    break;
                case "4":
                    Console.WriteLine(GenerateRandomNegativeInt());
                    break;
                default:
                    Console.WriteLine("Такой операции нет. Повторите ввод.");
                    continue;
            }

            if (!IsExit()) {
                break;
            }
        }
    }

    private int GenerateRandomEvenInt()
    {
        int randomNumber;
        do {
            randomNumber = new Random().Next(int.MinValue, int.MaxValue);
        } while (randomNumber % 2 != 0);

        return randomNumber;
    }

    private int GenerateRandomOddInt()
    {
        int randomNumber;
        do {
            randomNumber = random.Next(int.MinValue, int.MaxValue);
        } while (randomNumber % 2 == 0);

        return randomNumber;
    }

    private int GenerateRandomPositiveInt()
    {
        int randomNumber;
        do {
            randomNumber = random.Next(int.MinValue, int.MaxValue);
        } while (randomNumber % 2 <= 0);

        return randomNumber;
    }

    private int GenerateRandomNegativeInt()
    {
        int randomNumber;
        do {
            randomNumber = random.Next(int.MinValue, int.MaxValue);
        } while (randomNumber % 2 >= 0);

        return randomNumber;
    }

    private bool IsExit()
    {
        Console.WriteLine("Продолжить вычисления в этом режиме?");
        Console.WriteLine("y - продолжить.");
        Console.WriteLine("Любой другой символ - выйти в предыдущее меню.");
        switch (Console.ReadLine()) {
            case "y":
                return true;
            default:
                return false;
        }
    }
}
