using System.Data;

namespace Calculator;

class Program
{
    private static void WorkWithTwoNumbers() {
        double? result = null;

        Console.WriteLine("Выбран режим работы с двумя числами.");
        while (true) {
            Console.WriteLine("Введите первое число.");
            if (!float.TryParse(Console.ReadLine(), out float firstNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
            Console.WriteLine("Введите второе число число.");
            if (!float.TryParse(Console.ReadLine(), out float secondNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
            Console.WriteLine("Введите знак математической операции - +, -, /, *.");
            while (true) {
                Console.ReadLine();
                switch (Console.ReadLine()) {
                case "+":
                    result = (double)(firstNumber + secondNumber);
                    break;
                case "-":
                    result = (double)(firstNumber - secondNumber);
                    break;
                case "/":
                    result = (double)(firstNumber + secondNumber);
                    break;
                case "*":
                    result = (double)(firstNumber * secondNumber);
                    break;
                default:
                    Console.WriteLine("Такой операции нет. Повторите ввод.");
                    break;
                }

                if (result is not null) {
                    break;
                }
            }

            Console.WriteLine($"Результат = {result}.");
            
            Console.WriteLine("Продолжить вычисления в этом режиме?");
            Console.WriteLine("y - продолжить.");
            Console.WriteLine("Любой другой символ - выйти.");
            if (Console.ReadLine() != "y") {
                return;
            }
        }
    }

    private static void WorkWithEquation() {
        Console.WriteLine("Выбран режим работы c выражением.");
        while (true) {
            Console.WriteLine("Введите выражение");
            double? result = Convert.ToDouble(new DataTable().Compute(Console.ReadLine(), null));
            Console.WriteLine($"Результат = {result}.");
            
            Console.WriteLine("Продолжить вычисления в этом режиме?");
            Console.WriteLine("y - продолжить.");
            Console.WriteLine("Любой другой символ - выйти.");
            if (Console.ReadLine() != "y") {
                return;
            }
        }

    }

    static void Main(string[] args)
    {
        Console.WriteLine("Введите цифру, чтобы выбрать режим работы калькулятора.");
        Console.WriteLine("1 - режим работы с двумя числами.");
        Console.WriteLine("2 - режим работы c выражением.");
        switch(Console.ReadLine()) {
            case "1":
                WorkWithTwoNumbers();
                break;
            case "2":
                WorkWithEquation();
                break;
            default:
                Console.WriteLine("Такого режима работы нет. Повторите выбор.");
                break;
        }
    }
}
