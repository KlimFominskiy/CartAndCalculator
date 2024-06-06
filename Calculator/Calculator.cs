using System.Data;

namespace Calculator;

public class Calculator
{
    public double result;
    public Logger? logger;

    public Calculator(Logger? logger)
    {
        this.logger = logger;
    }

    public void WorkWithTwoNumbers()
    {
        Console.WriteLine("Выбран режим работы с двумя числами.");

        float firstNumber;
        float secondNumber;
        while (true) {
            Console.WriteLine("Введите первое число...");
            if (!float.TryParse(Console.ReadLine(), out firstNumber)) {
                Console.WriteLine("Неправильный формат числа.");
                continue;
            }
            Console.WriteLine("Введите второе число...");
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
                        result = Divide(firstNumber, secondNumber);
                        break;
                    default:
                        Console.WriteLine("Нет такой операции, введите допустимый знак операции...");
                        continue;
                }

                break;
            }
            Console.WriteLine($"Результат = {double.Round(result, 4)}.");

            if (!IsExit()) {
                break;
            }
        }
    }

    public void WorkWithEquation()
    {
        Console.WriteLine("Выбран режим работы c выражением.");
        while (true) {
            Console.WriteLine("Введите выражение...");
            double result = Convert.ToDouble(new DataTable().Compute(Console.ReadLine(), null));
            Console.WriteLine($"Результат = {double.Round((double)result, 4)}.");

            if (!IsExit()) {
                break;
            }
        }
    }

    public void WorhWithIntGeneratorExtension()
    {
        Console.WriteLine("Выбран режим генерации целого числа.");
        while (true) {
            Console.WriteLine("Какое число сгенерировать?");
            Console.WriteLine("1 - чётное.");
            Console.WriteLine("2 - нечётное.");
            Console.WriteLine("3 - положительное.");
            Console.WriteLine("4 - отрицательное.");
            int randomInt = 0;
            switch (Console.ReadLine()) {
                case "1":
                    Console.WriteLine(randomInt.GenerateRandomEvenInt());
                    break;
                case "2":
                    Console.WriteLine(randomInt.GenerateRandomOddInt());
                    break;
                case "3":
                    Console.WriteLine(randomInt.GenerateRandomPositiveInt());
                    break;
                case "4":
                    Console.WriteLine(randomInt.GenerateRandomNegativeInt());
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

    public void WorhWithMatrix()
    {
        Console.WriteLine("Выбран режим работы с матрицей.");
        while(true) {
            int rowsNumber;
            int columnsNumber;

            Console.WriteLine("Введите количество строк в матрице");
            while (!int.TryParse(Console.ReadLine(), out rowsNumber)) {
                Console.WriteLine("Неправильный формат числа. Повторите ввод.");
                continue;
            }
            Console.WriteLine("Введите количество столбцов в матрице");
            while (!int.TryParse(Console.ReadLine(), out columnsNumber)) {
                Console.WriteLine("Неправильный формат числа. Повторите ввод.");
                continue;
            }

            double[,] matrix = new double[rowsNumber, columnsNumber];

            for (int i = 0; i < rowsNumber; i++) {
                Console.WriteLine($"Введите элементы строки {i + 1}. Например - 5.38  22  7.23.");
                string? elements = Console.ReadLine();
                if (!string.IsNullOrEmpty(elements)) {
                    double[] currentRowNumbers = Array.ConvertAll(elements.Trim().Split(" "), Convert.ToDouble);
                    for (int j = 0; j < columnsNumber; j++) {
                        matrix[i, j] = currentRowNumbers[j];
                    }
                } else {
                    i -= 1;
                }
            }

            while(true) {
                Console.WriteLine("Выберите режим работы с матрицей.");
                Console.WriteLine("1 - поиск минимального положительного числа.");
                Console.WriteLine("2 - поиск максимального отрицательного числа.");
                double? result;
                switch(Console.ReadLine()) {
                    case "1":
                        result = FindTheSmallestPositiveNumberInMatrix(matrix);
                        break;
                    case "2":
                        result = FindTheLargestNegativeNumberInMatrix(matrix);
                        break;
                    default:
                        Console.WriteLine("Такой операции нет. Повторите ввод.");
                        continue;
                }

                if (result is null) {
                    Console.WriteLine("В матрице нет подъодящиъ элементов.");
                } else {
                    Console.WriteLine($"Результат = {result}");
                }

                break;    
            }

            if (!IsExit()) {
                break;
            }
        }
        
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

    private double Add(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne + numberTwo;
    }

    private double Subtract(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne - numberTwo;
    }

    private double Multiply(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne * numberTwo;
    }

    private double Divide(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne / numberTwo;
    }

    private void Log(string? methodName, string className)
    {
        if (logger is not null) {
            logger.Write($"Выполняется метод {methodName} класса {className}.");
        }
    }

    private double? FindTheSmallestPositiveNumberInMatrix(double [,] matrix)
    {
        double? theSmallestPositiveNumber = null;
        foreach(double matrixElement in matrix) {
            if (matrixElement > 0) {
                if (theSmallestPositiveNumber is null) {
                    theSmallestPositiveNumber = matrixElement;
                } else if (matrixElement < theSmallestPositiveNumber) {
                    theSmallestPositiveNumber = matrixElement;
                }
            }
        }

       return theSmallestPositiveNumber;
    }

    private double? FindTheLargestNegativeNumberInMatrix(double [,] matrix) 
    {
        double? theLargestNegativeNumber = null;
        foreach(double matrixElement in matrix) {
            if (matrixElement < 0) {
                if (theLargestNegativeNumber is null) {
                    theLargestNegativeNumber = matrixElement;
                } else if (matrixElement > theLargestNegativeNumber) {
                    theLargestNegativeNumber = matrixElement;
                }
            }
        }

       return theLargestNegativeNumber;
    }
}