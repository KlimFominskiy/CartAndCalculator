using Calculator.Methods;
using System.Data;

namespace Calculator;

/// <summary>
/// Класс для выполнения базовых математических операций, расчёта выражения, генерации чисел и поиска значения в матрице.
/// </summary>
public class Calculator
{
    /// <summary>
    /// Результат вычислений.
    /// </summary>
    public double result;
    /// <summary>
    /// Логгер для записи вызываемых методов и их классов.
    /// </summary>
    public Logger? logger;
    /// <summary>
    /// Методы работы с матрицей.
    /// </summary>
    
    private MatrixMethods matrixMethods;

    public Calculator(Logger? logger)
    {
        this.logger = logger;
        matrixMethods = new MatrixMethods();
    }

    /// <summary>
    /// Метод вызывает меню ввода чисел и знака операции.
    /// После ввода знака операции вызывается соответствующий метод для выполнения расчёта.
    /// </summary>
    public void CalculateTwoNumbers()
    {
        Console.WriteLine("Выбран режим работы с двумя числами.");

        while (true)
        {
            Console.WriteLine("Введите первое число...");
            float firstNumber = ReadNumberFromConsole();
            Console.WriteLine("Введите второе число...");
            float secondNumber = ReadNumberFromConsole();

            while (true)
            {
                Console.WriteLine("Введите знак операции (+, -, /, *).");
                switch (Console.ReadLine())
                {
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
                        Console.WriteLine("Нет такой операции.");
                        continue;
                }

                break;
            }
            Console.WriteLine($"Результат = {double.Round(result, 4)}.");

            if (!IsExit())
            {
                break;
            }
        }
    }

    /// <summary>
    /// Вызывает меню ввода выражения. После ввода выражения происходит его расчёт.
    /// </summary>
    public void CalculateEquation()
    {
        Console.WriteLine("Выбран режим работы c выражением.");
        while (true)
        {
            Console.WriteLine("Введите выражение...");
            double result = Convert.ToDouble(new DataTable().Compute(Console.ReadLine(), null));
            Console.WriteLine($"Результат = {double.Round((double)result, 4)}.");

            if (!IsExit())
            {
                break;
            }
        }
    }

    /// <summary>
    /// Метод вызывает меню для выбора режима генерации числа и вызывает соответствующий метод. 
    /// </summary>
    public void GenerateRandomInt()
    {
        Console.WriteLine("Выбран режим генерации целого числа.");
        while (true)
        {
            Console.WriteLine("Какое число сгенерировать?");
            Console.WriteLine("1 - чётное.");
            Console.WriteLine("2 - нечётное.");
            Console.WriteLine("3 - положительное.");
            Console.WriteLine("4 - отрицательное.");
            int randomInt = 0;
            switch (Console.ReadLine())
            {
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

            if (!IsExit())
            {
                break;
            }
        }
    }

    /// <summary>
    /// Метод вызывает меню ввода матрицы через консоль и вызывает меню выбора режима работы с матрицей.
    /// </summary>
    public void FindNumberInMatrix()
    {
        Console.WriteLine("Выбран режим работы с матрицей.");
        while (true)
        {
            double[,] matrix = ReadMatrixFromConsole();

            while (true)
            {
                Console.WriteLine("Выберите режим работы с матрицей.");
                Console.WriteLine("1 - поиск минимального положительного числа.");
                Console.WriteLine("2 - поиск максимального отрицательного числа.");
                double? result;
                switch (Console.ReadLine())
                {
                    case "1":
                        result = matrixMethods.FindTheSmallestPositiveNumberInMatrix(matrix);
                        break;
                    case "2":
                        result = matrixMethods.FindTheLargestNegativeNumberInMatrix(matrix);
                        break;
                    default:
                        Console.WriteLine("Такой операции нет. Повторите ввод.");
                        continue;
                }

                if (result is null)
                {
                    Console.WriteLine("В матрице нет подходящих элементов.");
                }
                else
                {
                    Console.WriteLine($"Результат = {result}");
                }

                break;
            }

            if (!IsExit())
            {
                break;
            }
        }

    }

    /// <summary>
    /// Метод вызывает меню выбора продолжения/прерывания работы в текущем режиме.
    /// </summary>
    /// <returns>true - продолжить в этом режиме. Иначе - false</returns>
    private bool IsExit()
    {
        Console.WriteLine("Продолжить вычисления в этом режиме?");
        Console.WriteLine("y - продолжить.");
        Console.WriteLine("Любой другой символ - выйти в предыдущее меню.");
        switch (Console.ReadLine())
        {
            case "y":
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Метод отправляет логгеру выполняемые метод и класс метода,
    /// а также выполняет суммирование двух чисел.
    /// </summary>
    /// <param name="numberOne">Первое число.</param>
    /// <param name="numberTwo">Второе число.</param>
    /// <returns>Результат сложения.</returns>
    private double Add(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne + numberTwo;
    }

    /// <summary>
    /// Метод отправляет логгеру выполняемые метод и класс метода,
    /// а также выполняет вычитание двух чисел.
    /// </summary>
    /// <param name="numberOne">Первое число.</param>
    /// <param name="numberTwo">Второе число.</param>
    /// <returns>Результат вычитания.</returns>
    private double Subtract(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne - numberTwo;
    }

    /// <summary>
    /// Метод отправляет логгеру выполняемые метод и класс метода,
    /// а также выполняет умножение двух чисел.
    /// </summary>
    /// <param name="numberOne">Первое число.</param>
    /// <param name="numberTwo">Второе число.</param>
    /// <returns>Результат умножения.</returns>
    private double Multiply(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne * numberTwo;
    }

    /// <summary>
    /// Метод вызывает метод записи сообщения с помощью логгера,
    /// а также выполняет деление двух чисел.
    /// </summary>
    /// <param name="numberOne">Первое число.</param>
    /// <param name="numberTwo">Второе число.</param>
    /// <returns>Результат деления.</returns>
    private double Divide(float numberOne, float numberTwo)
    {
        Log(System.Reflection.MethodBase.GetCurrentMethod()?.Name, GetType().Name);

        return numberOne / numberTwo;
    }

    /// <summary>
    /// Метод записи сообщения с помощью логгера.
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="className"></param>
    private void Log(string? methodName, string className)
    {
        if (logger is not null)
        {
            logger.Write($"Выполняется метод {methodName} класса {className}.");
        }
    }

    /// <summary>
    /// Метод вызывает меню ввода количества строк и столбцов матрицы через консоль, а затем
    /// вызывает меню ввода матрицы.
    /// </summary>
    /// <returns>Введённая пользователем матрица.</returns>
    private double[,] ReadMatrixFromConsole()
    {
        int rowsNumber;
        int columnsNumber;

        Console.WriteLine("Введите количество строк в матрице");
        while (!int.TryParse(Console.ReadLine(), out rowsNumber))
        {
            Console.WriteLine("Неправильный формат числа. Повторите ввод.");
            continue;
        }
        Console.WriteLine("Введите количество столбцов в матрице");
        while (!int.TryParse(Console.ReadLine(), out columnsNumber))
        {
            Console.WriteLine("Неправильный формат числа. Повторите ввод.");
            continue;
        }

        double[,] matrix = new double[rowsNumber, columnsNumber];

        for (int i = 0; i < rowsNumber; i++)
        {
            Console.WriteLine($"Введите элементы строки {i + 1}. Например - 5.38  22  7.23.");
            string? elements = Console.ReadLine();
            if (!string.IsNullOrEmpty(elements))
            {
                double[] currentRowNumbers = Array.ConvertAll(elements.Trim().Split(" "), Convert.ToDouble);
                for (int j = 0; j < columnsNumber; j++)
                {
                    matrix[i, j] = currentRowNumbers[j];
                }
            }
            else
            {
                Console.WriteLine("Неправильный формат. Повторите ввод.");
                i -= 1;
            }
        }

        return matrix;
    }

    /// <summary>
    /// Метод считывает число из консоли.
    /// </summary>
    /// <returns>Считанное число из консоли.</returns>
    private float ReadNumberFromConsole()
    {
        float numberFromConsole;
        
        while(!float.TryParse(Console.ReadLine(), out numberFromConsole))
        {
            Console.WriteLine("Неправильный формат числа.");
        }

        return numberFromConsole;
    }
}