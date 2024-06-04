using System.Data;

namespace Calculator;

public class Calculator {
    public double? result = null;

    public void WorkWithTwoNumbers() {
        Console.WriteLine("Выбран режим работы с двумя числами.");
        while (true) {
            Console.WriteLine("Введите первое число.");
            if (!float.TryParse(Console.ReadLine(), out float firstNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
            Console.WriteLine("Введите второе число.");
            if (!float.TryParse(Console.ReadLine(), out float secondNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
            Console.WriteLine("Введите знак математической операции - +, -, /, *.");
            while (true) {
                switch (Console.ReadLine()) {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "/":
                    result = firstNumber + secondNumber;
                    break;
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                default:
                    Console.WriteLine("Такой операции нет. Повторите ввод операции.");
                    break;
                }

                if (result is not null) {
                    break;
                }
            }

            Console.WriteLine($"Результат = {double.Round((double)result, 4)}.");
            
            Console.WriteLine("Продолжить вычисления в этом режиме?");
            Console.WriteLine("y - продолжить.");
            Console.WriteLine("Любой другой символ - выйти.");
            if (Console.ReadLine() != "y") {
                return;
            }
        }
    } 

    public void WorkWithEquation() {
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
}