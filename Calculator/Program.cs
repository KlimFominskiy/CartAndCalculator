namespace Calculator;

internal class Program {

    internal static void Main(string[] args) {
        while (true) {
            Console.WriteLine("Введите цифру, чтобы выбрать режим работы калькулятора.");
            Console.WriteLine("1 - режим работы с двумя числами.");
            Console.WriteLine("2 - режим работы c выражением.");
            Console.WriteLine("q - завершить работу программы.");
            Calculator calculator = new Calculator();
            switch(Console.ReadLine()) {
                case "1":
                    calculator.WorkWithTwoNumbers();
                    break;
                case "2":
                    calculator.WorkWithEquation();
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
