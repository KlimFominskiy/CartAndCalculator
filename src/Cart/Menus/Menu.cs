using Cart.Orders;
using Cart.Products;
using Cart.Readers;
using Cart.Settings;
using Cart.Stores;

namespace Cart.Menus;

/// <summary>
/// Меню пользователя для взаимодействия с программой.
/// </summary>
internal static class Menu
{
    /// <summary>
    /// Параметры предсгенерированного набор заказов.
    /// </summary>
    private struct ParamsOfRandomOrders
    {
        /// <summary>
        /// Стоимость самого дешёвого заказа из предсгенированных заказов.
        /// </summary>
        internal static decimal orderMinSum = decimal.MaxValue;
        /// <summary>
        /// Стоимость самого дорого заказа из предсгенированных заказов.
        /// </summary>
        internal static decimal orderMaxSum = 0;
        /// <summary>
        /// Количество товаров в заказе с наименьшим количеством товаров из предсгенированных заказов.
        /// </summary>
        internal static uint minQuantityOfProductsInOrder = uint.MaxValue;
        /// <summary>
        /// Количество товаров в заказе с наибольшим количеством товаров из предсгенированных заказов.
        /// </summary>
        internal static uint maxQuantityOfProductsInOrder = 0;
    }

    /// <summary>
    /// Заказ пользователя.
    /// </summary>
    private static Order userOrder = new();

    /// <summary>
    /// Обработчики заказа.
    /// </summary>
    private static readonly OrderHandlers orderHandlers = new();

    /// <summary>
    /// Обработчик печати заказа в консоль.
    /// </summary>
    private static readonly IPrintOrder printOrderToConsole = new PrintOrderToConsole();

    /// <summary>
    /// Обработчик печати заказа в API.
    /// </summary>
    private static readonly IPrintOrder printOrderToAPI = new PrintOrderToAPI();

    /// <summary>
    /// Настройка программы перед выбором режима работы.
    /// </summary>
    public static void Start()
    {
        // Задание 2. Считывание товаров из файла.
        Console.WriteLine("Сгенерировать новый набор товаров в магазине?\nВведите y, чтобы сгенерировать новый набор.\nВведите любой другой символ, чтобы считать старый набор.");
        if (Console.ReadLine() == "y")
        {
            Store.GenerateProducts("Генерация товаров в магазине.\n");
        }
        else
        {
            Store.ReadProductsFromFile("Считывание товаров из файла.\n");
        }

        // Задание 4. Генератор тестовых заказов.
        Console.WriteLine("Сгенерировать новый набор заказов?\nВведите y, чтобы сгенерировать новый набор.\nВведите любой другой символ, чтобы считать старый набор.");
        if (Console.ReadLine() == "y")
        {
            OrdersGenerator.GenerateRandomOrders("Генерация заказов\n");
        }
        else
        {
            OrdersGenerator.ReadOrdersFromFile("Считывание заказов из файла.\n");
        }

        CalculateParams("Расчёт параметров предсгенерированных заказов.\n");
    }

    /// <summary>
    /// Вывести в консоль режимы работы программы.
    /// </summary>
    public static void PrintProgramModes()
    {
        Console.WriteLine(
            "Выберите режим работы программы:\n" +
            "Считать заказ:\n" +
            $"{(int)ProgramModes.ReadOrderFromConsole} - ввести заказ через консоль.\n" +
            $"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.\n" +
            $"{(int)ProgramModes.GenerateRandomOrder} - сгенерировать случайный заказ.\n\n" +

            "Сгенерировать заказ по сумме:\n" +
            $"{(int)ProgramModes.GenerateOrderByMaxSum} - сгенерировать заказ по максимальной сумме.\n" +
            $"{(int)ProgramModes.GenerateOrderByMinMaxSumRange} - cгенерировать заказ по диапазону суммы.\n" +
            $"Минимальная общая сумма заказа - {ParamsOfRandomOrders.orderMinSum}.\n" +
            $"Максимальная общая сумма заказа - {ParamsOfRandomOrders.orderMaxSum}.\n" +
            $"{(int)ProgramModes.GenerateOrderByMaxTotalQuantity} - cгенерировать заказ по максимальному общему количеству товаров в заказе.\n" +
            $"Минимальное общее количество товаров в заказе - {ParamsOfRandomOrders.minQuantityOfProductsInOrder}.\n" +
            $"Максимальное общее количество товаров в заказе - {ParamsOfRandomOrders.maxQuantityOfProductsInOrder}.\n\n" +

            $"{(int)ProgramModes.ChangeProductInOrder} - изменить заказ.\n" +
            $"{(int)ProgramModes.PrintProducts} - вывести в консоль существующие продукты магазина.\n" +
            $"{(int)ProgramModes.PrintOrders} - вывести в консоль существующие заказы.\n\n" +

            "Калькультор заказов:\n" +
            $"{(int)ProgramModes.CreateOrderFromTwoProducts} - создать корзину с двумя указанными товарами.\n" +
            $"{(int)ProgramModes.AddProductToOrder} - добавить товар в корзину.\n" +
            $"{(int)ProgramModes.CombineTwoOrders} - объединить корзины.\n" +
            $"{(int)ProgramModes.ReduceTheQuantityOfTheProductInOrderByOne} - удалить единицу указанного товара из корзины.\n" +
            $"{(int)ProgramModes.RemoveMatchingProducts} - удалить из первой корзины товары, которые есть во второй корзине.\n" +
            $"{(int)ProgramModes.RemoveProductsFromOrderByType} - удалить из корзины товары указанного типа.\n" +
            $"{(int)ProgramModes.DivideTheQuantityOfEachProductInOrder} - уменьшить в корзине каждое количество товара в указанное число раз.\n" +
            $"{(int)ProgramModes.MultiplyTheQuantityOfEachProductInOrder} - увеличить в корзине каждое количество товара в указанное число раз.\n\n" +
            
            // Задание 5. Работа с LINQ.
            "Отсортировать заказы:\n" +
            $"{(int)ProgramModes.GetOrdersByMaxSum} - вывести заказы дешевле заданной суммы.\n" +
            $"{(int)ProgramModes.GetOrdersByMinSum} - вывести заказы дороже заданной суммы.\n" +
            $"{(int)ProgramModes.GetOrdersByProductType} - вывести заказы, имеющие в составе товары определённого типа.\n" +
            $"{(int)ProgramModes.GetOrdersSortedByWeight} - вывести заказы, отсортированные по весу в порядке возрастания.\n" +
            $"{(int)ProgramModes.GetOrdersWithUniqueProductsInList} - вывести заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).\n" +
            $"{(int)ProgramModes.GetOrdersByMaxDepartureDate} - вывести заказы, отправленные до указанной даты.\n\n" +

            $"{(int)ProgramModes.SortOrderByNames} - отсортировать заказ в алфавиту.\n" +
            $"{(int)ProgramModes.WriteOrderToFile} - записать заказ в файл.\n\n" +

            $"{(int)ProgramModes.Exit} - закончить работу программы.\n"
        );
    }

    /// <summary>
    /// Расчитать параметры предсгенерированных заказов.
    /// </summary>
    /// <exception cref="ArgumentNullException">Сумма предсгенерированного заказа равна null.</exception>
    private static void CalculateParams(string title)
    {
        Console.Write(title);

        foreach (Order order in OrdersGenerator.Orders)
        {
            decimal tempSum = order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) ??
                throw new ArgumentNullException(paramName: nameof(tempSum), message: "Сумма предсгенерированного заказа равна null.");
            if (ParamsOfRandomOrders.orderMinSum > tempSum)
            {
                ParamsOfRandomOrders.orderMinSum = tempSum;
            }
            if (ParamsOfRandomOrders.orderMaxSum < tempSum)
            {
                ParamsOfRandomOrders.orderMaxSum = tempSum;
            }
            uint quantity = (uint)order.Products.Sum(orderItem => orderItem.Value);
            if (ParamsOfRandomOrders.minQuantityOfProductsInOrder > quantity)
            {
                ParamsOfRandomOrders.minQuantityOfProductsInOrder = quantity;
            }
            if (ParamsOfRandomOrders.maxQuantityOfProductsInOrder < quantity)
            {
                ParamsOfRandomOrders.maxQuantityOfProductsInOrder = quantity;
            }
        }

        Console.WriteLine("Параметры рассчитаны.");
    }

    /// <summary>
    /// Выбрать режим работы программы.
    /// </summary>
    public static void ChooseMode()
    {
        while (true)
        {
            ProgramModes programMode = ConsoleReader.ReadProgramModeFromConsole("Введите режим работы программы.\n");

            OrderCalculator orderCalculator = new(new Calculator.Logger());
            switch (programMode)
            {
                case ProgramModes.Exit:
                    {
                        return;
                    }
                //Задание 1. Ввод заказа с помощью консоли.
                case ProgramModes.ReadOrderFromConsole:
                    {
                        userOrder = orderHandlers.ReadOrderFromConsole("Считывание заказа из консоли.\n");
                        printOrderToConsole.Print(userOrder, "Введённый из консоли заказ.\n");
                        break;
                    }
                case ProgramModes.ReadOrderFromFile:
                    {
                        userOrder = orderHandlers.ReadOrderFromFile("Считывание заказа из файла.\n");
                        printOrderToConsole.Print(userOrder, "Считанный из файла заказ.\n");
                        break;
                    }
                case ProgramModes.GenerateRandomOrder:
                    {
                        userOrder = OrdersGenerator.GenerateRandomOrder("Генерация случайного заказа.\n");
                        printOrderToConsole.Print(userOrder, "Сгенерированный заказ.\n");
                        break;
                    }
                case ProgramModes.GenerateOrderByMaxSum:
                    {
                        Console.WriteLine("Генерация заказа по макимальной сумме заказа.");
                        userOrder = OrdersGenerator.GenerateOrderBySum(ConsoleReader.ReadDecimalFromConsole("Введите максимальную сумму заказа.\n"));
                        printOrderToConsole.Print(userOrder, "Сгенерированный заказ.\n");
                        break;
                    }
                case ProgramModes.GenerateOrderByMinMaxSumRange:
                    {
                        Console.WriteLine("Генерация заказа по диапазону суммы заказа.");
                        userOrder = OrdersGenerator.GenerateOrderBySum(
                            ConsoleReader.ReadDecimalFromConsole("Введите минимальную сумму заказа.\n"),
                            ConsoleReader.ReadDecimalFromConsole("Введите максимальную сумму заказа.\n")
                            );
                        printOrderToConsole.Print(userOrder, "Сгенерированный заказ.\n");
                        break;
                    }
                case ProgramModes.GenerateOrderByMaxTotalQuantity:
                    {
                        Console.WriteLine("Генерация заказа по максимальному общему количеству товаров в заказе.");
                        userOrder = OrdersGenerator.GenerateOrderByMaxQuantity(
                            ConsoleReader.ReadUintFromConsole("Введите максимальное общее количество товаров в заказе.\n")
                            );
                        printOrderToConsole.Print(userOrder, "Сгенерированный заказ.\n");
                        break;
                    }
                case ProgramModes.ChangeProductInOrder:
                    {
                        userOrder = orderHandlers.UpdateProduct(userOrder, "Изменение товара в заказе.\n");
                        printOrderToConsole.Print(userOrder, "Обновлённый заказ.\n");
                        break;
                    }
                case ProgramModes.PrintProducts:
                    {
                        Store.PrintProductsInfo("Список товаров в магазине.\n");
                        break;
                    }
                case ProgramModes.PrintOrders:
                    {
                        Console.WriteLine("Список предсгенерированных заказов.\n");
                        foreach(Order order in OrdersGenerator.Orders)
                        {
                            printOrderToConsole.Print(order, "");
                        }
                        break;
                    }
                case ProgramModes.CreateOrderFromTwoProducts:
                    {
                        Console.WriteLine("Создание корзины с двумя указанными товарами.");
                        
                        Store.PrintProductsInfo("Список товаров магазина.\n");
                        userOrder = orderCalculator.Add(
                            Store.Products[(int)ConsoleReader.ReadProductNumberFromConsole("Ввод первого товара.\n" +
                            "Введите номер товара из списка товаров магазина.\n") - 1],
                            Store.Products[(int)ConsoleReader.ReadProductNumberFromConsole("Ввод второго товара.\n" +
                            "Введите номер товара из списка товаров магазина.\n") - 1]
                            );
                        printOrderToConsole.Print(userOrder, "Созданная из двух товаров корзина.\n");
                        break;
                    }
                case ProgramModes.AddProductToOrder:
                    {
                        Console.WriteLine("Добавить товар в корзину.");
                        Store.PrintProductsInfo("Список товаров магазина.\n");
                        userOrder = orderCalculator.Add(
                            userOrder,
                            Store.Products[(int)ConsoleReader.ReadProductNumberFromConsole("Ввод товара.\nВведите номер товара из списка товаров магазина.\n") - 1]
                            );
                        printOrderToConsole.Print(userOrder, "Заказ с новым товаром.\n");
                        break;
                    }
                case ProgramModes.CombineTwoOrders:
                    {
                        Console.WriteLine("Объединить корзины.");
                        printOrderToConsole.Print(userOrder, "Текущая корзина.\n");
                        
                        programMode = ConsoleReader.ReadProgramModeFromConsole("Ввод второй корзины.\n" +
                            $"{(int)ProgramModes.ReadOrderFromConsole} - считать заказ из консоли.\n" +
                            $"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.");

                        Order secondOrder = new();
                        while (true)
                        {
                            if (programMode == ProgramModes.ReadOrderFromConsole)
                            {
                                secondOrder = orderHandlers.ReadOrderFromConsole("Считывание заказа из консоли.\n");
                            }
                            else if (programMode == ProgramModes.ReadOrderFromFile)
                            {
                                secondOrder = orderHandlers.ReadOrderFromFile("Считывание заказа из файла.\n");
                            }
                            else
                            {
                                Console.WriteLine($"Считано {programMode}. Такого режима нет. Повторите ввод.");
                                continue;
                            }
                            break;
                        }
                        userOrder = orderCalculator.Add(userOrder, secondOrder);
                        printOrderToConsole.Print(userOrder, "Объединённый заказ.\n");
                        break;
                    }
                case ProgramModes.ReduceTheQuantityOfTheProductInOrderByOne:
                    {
                        Console.WriteLine("Удалить единицу товара из корзины.\n");
                        printOrderToConsole.Print(userOrder, "Текущая корзина.\n");
                        Store.PrintProductsInfo("Списов товаров магазина.\n");
                        userOrder = orderCalculator.Subtract(
                            userOrder,
                            Store.Products[(int)ConsoleReader.ReadProductNumberFromConsole("Введите номер товара из списка товаров магазина.\n") - 1]
                            );
                        printOrderToConsole.Print(userOrder, "Заказ с удалённой единицей товара.\n");
                        break;
                    }
                case ProgramModes.RemoveMatchingProducts:
                    {
                        Console.WriteLine("Удалить из первой корзины товары, которые есть во второй корзине.\n" +
                            "Ввод второй корзины.\n" +
                            $"{(int)ProgramModes.ReadOrderFromConsole} - считать заказ из консоли.\n" +
                            $"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.");
                        programMode = ConsoleReader.ReadProgramModeFromConsole();

                        Order secondOrder = new();
                        while (true)
                        {
                            if (programMode == ProgramModes.ReadOrderFromConsole)
                            {
                                userOrder = orderHandlers.ReadOrderFromConsole("Считывание заказа из консоли.\n");
                            }
                            else if (programMode == ProgramModes.ReadOrderFromFile)
                            {
                                userOrder = orderHandlers.ReadOrderFromFile("Считывание заказа из файла.\n");
                            }
                            else
                            {
                                Console.WriteLine($"Считано {programMode}. Такого режима нет. Повторите ввод.");
                                continue;
                            }
                            break;
                        }
                        userOrder = orderCalculator.Subtract(userOrder, secondOrder);
                        printOrderToConsole.Print(userOrder, "Первая корзина без товаров из второй корзины.\n");
                        break;
                    }
                case ProgramModes.RemoveProductsFromOrderByType:
                    {
                        Console.WriteLine("Удалить из корзины товары указанного типа.");

                        Store.PrintProductsTypes("Типы товаров в магазине.\n");
                        uint productType = ConsoleReader.ReadProductTypeNumberFromConsole("Введите номер типа товара из списка товаров магазина.\n");
                        userOrder = orderCalculator.Divide(userOrder, Store.Products[(int)productType - 1].GetType());
                        printOrderToConsole.Print(userOrder, $"Корзина без товаров типа {Store.Products[(int)productType - 1].GetType()}.\n");
                        break;
                    }
                case ProgramModes.DivideTheQuantityOfEachProductInOrder:
                    {
                        Console.WriteLine("Уменьшить в корзине каждое количество товара в указанное число раз.");

                        printOrderToConsole.Print(userOrder, "Список товаров в заказе.\n");
                        uint divisor = ConsoleReader.ReadUintFromConsole("Введите число, указывающее во сколько раз уменьшить количество товара.\n");
                        userOrder = orderCalculator.Divide(userOrder, divisor);
                        printOrderToConsole.Print(userOrder, $"Корзина с уменьшенным количеством товаров в {divisor} раз.\n");
                        break;
                    }
                case ProgramModes.MultiplyTheQuantityOfEachProductInOrder:
                    {
                        Console.WriteLine("Увеличить в корзине каждое количество товара в указанное число раз.");

                        printOrderToConsole.Print(userOrder, "Список товаров в заказе.\n");
                        uint multiplier = ConsoleReader.ReadUintFromConsole("Введите число, указывающее во сколько раз увеличить количество товара.\n");
                        userOrder = orderCalculator.Multiply(userOrder, multiplier);
                        printOrderToConsole.Print(userOrder, $"Корзина с уменьшенным количеством товаров в {multiplier} раз.\n");
                        break;
                    }
                case ProgramModes.GetOrdersByMaxSum:
                    {
                        Console.WriteLine("Заказы, дешевле заданной суммы.");

                        decimal orderMaxSum = ConsoleReader.ReadDecimalFromConsole("Введите максимальную сумму заказа.\n");
                        List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < orderMaxSum).ToList();
                        PrintOrdersList(validOrders, $"Заказы, дешевле {orderMaxSum}.\n");
                        break;
                    }
                case ProgramModes.GetOrdersByMinSum:
                    {
                        Console.WriteLine("Заказы, дороже заданной суммы.");

                        decimal orderMinSum = ConsoleReader.ReadDecimalFromConsole("Введите минимальную сумму заказа.");
                        List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > orderMinSum).ToList();
                        PrintOrdersList(validOrders, $"Заказы, дороже {orderMinSum}\n");
                        break;
                    }
                case ProgramModes.GetOrdersByProductType:
                    {
                        Console.WriteLine("Заказы, имеющие в составе товары определённого типа.");

                        uint productTypeNumber = ConsoleReader.ReadProductTypeNumberFromConsole("Выберите номер типа продукта из списка");
                        List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.Any(orderItem => orderItem.Key.GetType() == Store.ProductsTypes[(int)productTypeNumber - 1].GetType())).ToList();
                        PrintOrdersList(validOrders, $"Заказы, имеющие в составе хотя бы один товар типа {Store.ProductsTypes[(int)productTypeNumber - 1].GetType().Name}");
                        break;
                    }
                case ProgramModes.GetOrdersSortedByWeight:
                    {
                        List<Order> validOrders = OrdersGenerator.Orders.OrderBy(order => order.Products.Sum(orderItem => orderItem.Key.Weight)).ToList();
                        PrintOrdersList(validOrders, "Заказы, отсортированные по весу в порядке возрастания.");
                        break;
                    }
                case ProgramModes.GetOrdersWithUniqueProductsInList:
                    {
                        List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.All(orderItem => orderItem.Value == 1)).ToList();
                        PrintOrdersList(validOrders, "Заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).");
                        break;
                    }
                case ProgramModes.GetOrdersByMaxDepartureDate:
                    {
                        Console.WriteLine("Заказы, отправленные до указанной даты.");

                        DateTime? userDateTime = ConsoleReader.ReadDateFromConsole("Введите дату в формате ДД/ММ/ГГГГ.\n");
                        List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.TimeOfDeparture <= userDateTime).ToList();
                        PrintOrdersList(validOrders, $"Заказы, отправленные до даты {userDateTime}");
                        break;
                    }
                //Задание 1. Отсортировать по алфавиту без LINQ.
                case ProgramModes.SortOrderByNames:
                    {
                        userOrder = orderHandlers.SortProductsByAlphabet(userOrder, "Сортировка заказа по алфавиту.");
                        printOrderToConsole.Print(userOrder, "Заказ, отсортированный по алфавиту.");
                        break;
                    }
                case ProgramModes.WriteOrderToFile:
                    {
                        orderHandlers.WriteOrderToFile(userOrder, "Запись заказа в файл.");
                        break;
                    }

            }
            PrintProgramModes();
        }
    }

    /// <summary>
    /// Вывести список заказов в консоль.
    /// </summary>
    /// <param name="orders"></param>
    public static void PrintOrdersList(List<Order> orders, string title = "")
    {
        Console.Write(title);

        for (int i = 0; i < orders.Count; i++)
        {
            Console.WriteLine($"Заказ {i + 1}");
            printOrderToConsole.Print(orders[i]);
        }
    }

    /// <summary>
    /// Вывести в консоль варианты требования к цене.
    /// </summary>
    public static void PrintOrderItemPriceSettings(string title = "")
    {
        Console.Write(title);

        Console.WriteLine(
            "Возможные значения требования:\n" +
            $"{(int)PriceRequirementSettings.RandomValue} - любое значение.\n" +
            $"{(int)PriceRequirementSettings.TheLowestValue} - самое низкое значение,\n" +
            $"{(int)PriceRequirementSettings.TheHighestValue} - самое высокое значение.");
    }
}