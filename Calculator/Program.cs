namespace Calculator;

internal class Program
{

    internal static void Main(string[] args)
    {
        while (true) {
            Calculator calculator;
            Console.WriteLine("Хотите ли отслеживать выполняемые методы? (y/n)");
            switch(Console.ReadLine()) {
                case "y":
                    Console.WriteLine("Методы будут отслеживаться.");
                    calculator = new(new Logger());
                    break;
                case "n":
                    Console.WriteLine("Методы не будут отслеживаться.");
                    calculator = new(null);
                    break;
                default:
                    Console.WriteLine("Такого режима нет. Повторите ввод.");
                    continue;
            }
            Console.WriteLine("Введите цифру, чтобы выбрать режим работы калькулятора.");
            Console.WriteLine("1 - режим работы с двумя числами.");
            Console.WriteLine("2 - режим работы c выражением.");
            Console.WriteLine("3 - режим генерации целого числа.");
            Console.WriteLine("4 - режим поиска значения в массиве.");
            Console.WriteLine("q - завершить работу программы.");
            switch(Console.ReadLine()) {
                case "1":
                    calculator.WorkWithTwoNumbers();
                    break;
                case "2":
                    calculator.WorkWithEquation();
                    break;
                case "3":
                    IntExtender intExtender = new();;
                    intExtender.ChooseNumberToGenerate();
                    break;
                case "4":
                    break;
                case "q":
                    return;
                default:
                    Console.WriteLine("Такого режима работы нет. Повторите выбор.");
                    break;
            }
        }
    }
}
