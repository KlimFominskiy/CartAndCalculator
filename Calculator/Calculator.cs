using System.Data;

namespace PSB_Calculator;

public class Calculator {
    public double result;
    public Logger? logger;

    public Calculator(Logger? logger) {
        this.logger = logger;
    }

    public void WorkWithTwoNumbers() {
        Console.WriteLine("Выбран режим работы с двумя числами.");
        
        float firstNumber;
        float secondNumber;
        while (true) {
            Console.WriteLine("Введите число...");
            if (!float.TryParse(Console.ReadLine(), out firstNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
            Console.WriteLine("Введите ещё число...");
            if (!float.TryParse(Console.ReadLine(), out secondNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
        
            Console.WriteLine("Введите знак операции (+, -, /, *).");
            while (true) {
                switch (Console.ReadLine()) {
                case "+":
                    result = Add(firstNumber, secondNumber);
                    break;
                case "-":
                    result = Subtract(firstNumber, secondNumber);
                    break;
                case "*":
                    result = Multiply(firstNumber, secondNumber);
                    break;
                case "/":
                    result = Divide(firstNumber, secondNumber);;
                    break;
                default:
                    Console.WriteLine("Нет такой операции, введите допустимый знак операции...");
                    continue;
                }
                break;
            }
            
            if (!IsExit(result)) {
                break;
            }
        }
    } 

    public void WorkWithEquation() {
        Console.WriteLine("Выбран режим работы c выражением.");
        while (true) {
            Console.WriteLine("Введите выражение...");
            double result = Convert.ToDouble(new DataTable().Compute(Console.ReadLine(), null));

            if (!IsExit(result)) {
                break;
            }
        }
    }

    private bool IsExit(double result) {
        Console.WriteLine($"Результат = {double.Round((double)result, 4)}.");
        Console.WriteLine("Продолжить вычисления в этом режиме?");
        Console.WriteLine("y - продолжить.");
        Console.WriteLine("Любой другой символ - выйти.");
        switch(Console.ReadLine()) {
            case "y":
                return true;
            default:
                return false;
        }
    }

    private double Add(float numberOne, float numberTwo) {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne + numberTwo;
    }

    private double Subtract(float numberOne, float numberTwo) {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);
        
        return numberOne - numberTwo;
    }

    private double Multiply(float numberOne, float numberTwo) {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);
        
        return numberOne * numberTwo;
    }

    private double Divide(float numberOne, float numberTwo) {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);
        
        return numberOne / numberTwo;
    }

    private void Log(string? methodName, string className) {
        if (logger is not null) {
            logger.Write($"Выполняется метод {methodName} класса {className}.");
        }
    }
}