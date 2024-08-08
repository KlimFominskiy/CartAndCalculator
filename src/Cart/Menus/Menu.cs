using Cart.Orders;
using Cart.Products;
using Cart.Settings;
using Cart.Stores;

namespace Cart.Menus
{
    /// <summary>
    /// Меню пользователя для взаимодействия с программой.
    /// </summary>
    internal static class Menu
    {
        /// <summary>
        /// Стоимость самого дешёвого заказа из предсгенированных заказов.
        /// </summary>
        private static decimal orderMinSum = decimal.MaxValue;
        /// <summary>
        /// Стоимость самого дорого заказа из предсгенированных заказов.
        /// </summary>
        private static decimal orderMaxSum = 0;
        /// <summary>
        /// Количество товаров в заказе с наименьшим количеством товаров из предсгенированных заказов.
        /// </summary>
        private static uint minTotalProductsQuantityInOrder = uint.MaxValue;
        /// <summary>
        /// Количество товаров в заказе с наибольшим количеством товаров из предсгенированных заказов.
        /// </summary>
        private static uint maxProductsQuantityInOrder = 0;

        private static Order userOrder;

        private static OrderHandlers orderHandlers;

        /// <summary>
        /// Настройка программы перед выбором режима работы.
        /// </summary>
        public static void Start()
        {
            // Задание 2. Считывание товаров из файла.
            Console.WriteLine("Сгенерировать новый набор товаров в магазине?\nВведите y, чтобы сгенерировать новый набор.\nВведите любой другой символ, чтобы считать старый набор.");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Генерация товаров в магазине.");
                Store.GenerateProducts();
                Console.WriteLine("Товары в магазине сгенерированы.");
            }
            else
            {
                Console.WriteLine("Считывание продуктов из файла.");
                Store.ReadProductsFromFile();
                Console.WriteLine("Продукты считаны.");
            }

            // Задание 4. Генератор тестовых заказов.
            Console.WriteLine("Сгенерировать новый набор заказов?\nВведите y, чтобы сгенерировать новый набор.\nВведите любой другой символ, чтобы считать старый набор.");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Генерация заказов");
                OrdersGenerator.GenerateRandomOrders();
                Console.WriteLine("Заказы сгенерированы.");
            }
            else
            {
                Console.WriteLine("Считывание заказов из файла.");
                OrdersGenerator.ReadOrdersFromFile();
                Console.WriteLine("Заказы считаны.");
            }
        }

        /// <summary>
        /// Вывести в консоль режимы работы программы.
        /// </summary>
        public static void PrintProgramModes()
        {
            CalculateParams();

            // Так должно выглядеть????
            Console.WriteLine("Выберите режим работы программы:\n" +
                "Считать заказ:\n" +
                $"{(int)ProgramModes.ReadOrderFromConsole} - считать заказ из консоли.\n" +
                $"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.\n" +
                $"{(int)ProgramModes.GenerateRandomOrder} - сгенерировать случайный заказ.");

            Console.WriteLine("Сгенерировать заказ по сумме:");
            Console.WriteLine($"{(int)ProgramModes.GenerateOrderByMaxSum} - сгенерировать заказ по максимальной сумме.");
            Console.WriteLine($"{(int)ProgramModes.GenerateOrderByMinMaxSumRange} - cгенерировать заказ по диапазону суммы.");
            Console.WriteLine($"Минимальная общая сумма заказа - {orderMinSum}.");
            Console.WriteLine($"Максимальная общая сумма заказа - {orderMaxSum}.");
            Console.WriteLine($"{(int)ProgramModes.GenerateOrderByMaxTotalQuantity} - cгенерировать заказ по максимальному общему количеству товаров в заказе.");
            Console.WriteLine($"Минимальное общее количество товаров в заказе - {minTotalProductsQuantityInOrder}.");
            Console.WriteLine($"Максимальное общее количество товаров в заказе - {maxProductsQuantityInOrder}.");

            Console.WriteLine($"{(int)ProgramModes.ChangeProductInOrder} - изменить заказ.");
            Console.WriteLine($"{(int)ProgramModes.PrintProducts} - вывести в консоль существующие продукты магазина.");
            Console.WriteLine($"{(int)ProgramModes.PrintOrders} - вывести в консоль существующие заказы.");

            Console.WriteLine("Калькультор заказов:");
            Console.WriteLine($"{(int)ProgramModes.CreateOrderFromTwoProducts} - создать корзину с двумя указанными товарами.");
            Console.WriteLine($"{(int)ProgramModes.AddProductToOrder} - добавить товар в корзину.");
            Console.WriteLine($"{(int)ProgramModes.CombineTwoOrders} - объединить корзины.");
            Console.WriteLine($"{(int)ProgramModes.ReduceTheQuantityOfTheProductInOrderByOne} - удалить единицу каждого товара из корзины.");
            Console.WriteLine($"{(int)ProgramModes.RemoveMatchingProducts} - удалить из первой корзины товары, которые есть во второй корзине.");
            Console.WriteLine($"{(int)ProgramModes.RemoveProductsFromOrderByType} - удалить из корзины товары указанного типа.");
            Console.WriteLine($"{(int)ProgramModes.ReduceTheQuantityOfEachProductInOrderByNumberTimes} - уменьшить в корзине каждое количество товара в указанное число раз.");
            Console.WriteLine($"{(int)ProgramModes.IncreaseTheQuantityOfEachProductInOrderByNumberTimes} - увеличить в корзине каждое количество товара в указанное число раз.");

            // Задание 5. Работа с LINQ.
            Console.WriteLine($"Отсортировать заказы:");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByMaxSum} - заказы дешевле заданной суммы.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByMinSum} - заказы дороже заданной суммы.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByProductType} - заказы, имеющие в составе товары определённого типа.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersSortedByWeight} - заказы, отсортированные по весу в порядке возрастания.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersWithUniqueProductsInList} - заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByMaxDepartureDate} - заказы, отправленные до указанной даты.");
            Console.WriteLine("0 - закончить работу программы.");
        }

        /// <summary>
        /// Расчитать параметры предсгенерированны заказов.
        /// </summary>
        /// <exception cref="ArgumentNullException">Сумма предсгенерированного заказа равна null.</exception>
        private static void CalculateParams()
        {
            foreach (Order order in OrdersGenerator.Orders)
            {
                decimal tempSum = order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) ?? throw new ArgumentNullException();
                if (orderMinSum > tempSum)
                {
                    orderMinSum = tempSum;
                }
                if (orderMaxSum < tempSum)
                {
                    orderMaxSum = tempSum;
                }
                uint quantity = (uint)order.Products.Sum(orderItem => orderItem.Value);
                if (minTotalProductsQuantityInOrder > quantity)
                {
                    minTotalProductsQuantityInOrder = quantity;
                }
                if (maxProductsQuantityInOrder < quantity)
                {
                    maxProductsQuantityInOrder = quantity;
                }
            }
        }

        /// <summary>
        /// Выбрать режим работы программы.
        /// </summary>
        public static void ChooseMode()
        {
            while (true)
            {
                ProgramModes programMode = ReadTypesFromConsole.ReadProgramModeFromConsole();

                OrderCalculator orderCalculator = new(new Calculator.Logger());
                switch (programMode)
                {
                    case ProgramModes.Exit:

                        return;
                    //Задание 1. Ввод заказа с помощью консоли.
                    case ProgramModes.ReadOrderFromConsole:
                        {
                            Console.WriteLine("Типы товаров в магазине.");
                            Store.PrintProductsTypes();

                            Console.WriteLine("Считывание заказа из консоли.");
                            Console.WriteLine("Введите номер товара в списке продуктов.");
                            orderHandlers.ReadOrderFromConsole();
                            Console.WriteLine("Считывание заказа из консоли завершено.");
                            Console.WriteLine("Информация о заказе.");
                            orderHandlers.PrintOrderInfo();
                            Console.WriteLine("Отсортироваться заказ по алфавиту? y - да, любой другой символ - нет.");
                            if (Console.ReadLine() == "y")
                            {
                                //Задание 1. Отсортировать по алфавиту без LINQ.
                                orderHandlers.SortProductsByAlphabet();
                            }
                            Console.WriteLine("Записать заказ в файл? y - да, любой другой символ - нет.");
                            if (Console.ReadLine() == "y")
                            {
                                Console.WriteLine("Запись заказ в файл");
                                orderHandlers.WriteOrderToFile();
                                Console.WriteLine("Запись заказа в файл окончена.");
                            }

                            break;
                        }
                    case ProgramModes.ReadOrderFromFile:
                        {
                            Console.WriteLine("Считывание заказа из файла.");
                            userOrder = new();
                            orderHandlers = new(userOrder);
                            userOrder = orderHandlers.ReadOrderFromFile();
                            Console.WriteLine("Считывание заказа из файла завершено.");

                            Console.WriteLine("Информация о заказе.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.GenerateRandomOrder:
                        {
                            Console.WriteLine("Генерация случайного заказа.");
                            userOrder = OrdersGenerator.GenerateRandomOrder();
                            Console.WriteLine("Сгенерированный заказ.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.GenerateOrderByMaxSum:
                        {
                            Console.WriteLine("Введите максимальную сумму заказа.");
                            decimal userMaxOrderSum = ReadTypesFromConsole.ReadDecimalFromConsole();

                            userOrder = OrdersGenerator.GenerateOrderBySum(userMaxOrderSum);
                            orderHandlers = new(userOrder);
                            Console.WriteLine("Информация о заказе.");
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.GenerateOrderByMinMaxSumRange:
                        {
                            Console.WriteLine("Введите минимальную сумму заказа.");
                            decimal userMinOrderSum = ReadTypesFromConsole.ReadDecimalFromConsole();
                            Console.WriteLine("Введите максимальную сумму заказа.");
                            decimal userMaxOrderSum = ReadTypesFromConsole.ReadDecimalFromConsole();

                            userOrder = OrdersGenerator.GenerateOrderBySum(userMinOrderSum, userMaxOrderSum);
                            orderHandlers = new(userOrder);
                            Console.WriteLine("Информация о заказе.");
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.GenerateOrderByMaxTotalQuantity:
                        {
                            Console.WriteLine("Введите максимальное общее количество товаров в заказе.");
                            uint userMaxOrderQuantity = ReadTypesFromConsole.ReadUintFromConsole();

                            userOrder = OrdersGenerator.GenerateOrderByMaxQuantity(userMaxOrderQuantity);

                            Console.WriteLine("Информация о заказе.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.ChangeProductInOrder:
                        {
                            Console.WriteLine("Изменение товара в заказе.");
                            if (userOrder.Products.Count == 0)
                            {
                                Console.WriteLine("Для начала введите заказ.");
                                break;
                            }
                            orderHandlers = new(userOrder);
                            orderHandlers.UpdateProduct();
                            Console.WriteLine("Заказ обновлён");
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.PrintProducts:
                        {
                            Store.PrintProductsInfo();

                            break;
                        }
                    case ProgramModes.PrintOrders:
                        {
                            OrdersGenerator.PrintOrdersInfo();

                            break;
                        }
                    case ProgramModes.CreateOrderFromTwoProducts:
                        {
                            Console.WriteLine("Создать корзину с двумя указанными товарами.");
                            Console.WriteLine("Список товаров магазина.");
                            Store.PrintProductsInfo();

                            Console.WriteLine("Ввод первого товара.");
                            Console.WriteLine("Введите номер товара из списка товаров магазина.");
                            uint firstAddedProductNumber = ReadTypesFromConsole.ReadProductNumberFromConsole();

                            Console.WriteLine("Ввод второго товара.");
                            Console.WriteLine("Введите номер товара из списка товаров магазина.");
                            uint secondAddedProductNumber = ReadTypesFromConsole.ReadProductNumberFromConsole(); ;

                            userOrder = orderCalculator.Add(Store.Products[(int)firstAddedProductNumber - 1], Store.Products[(int)secondAddedProductNumber - 1]);
                            Console.WriteLine("Созданая корзина.");
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.AddProductToOrder:
                        {
                            Console.WriteLine("Добавить товар в корзину.");

                            Console.WriteLine("Список товаров магазина.");
                            Store.PrintProductsInfo();

                            Console.WriteLine("Ввод товара.");
                            Console.WriteLine("Введите номер товара из списка товаров магазина.");
                            uint addedProductNumber = ReadTypesFromConsole.ReadProductNumberFromConsole();
                            userOrder = orderCalculator.Add(userOrder, Store.Products[(int)addedProductNumber - 1]);
                            Console.WriteLine("Заказ с новым товаром.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.CombineTwoOrders:
                        {
                            Console.WriteLine("Объединить корзины.");
                            Console.WriteLine("Текущая корзина.");
                            orderHandlers.PrintOrderInfo();

                            Console.WriteLine("Ввод второй корзины.");
                            Console.WriteLine($"{(int)ProgramModes.ReadOrderFromConsole} - считать заказ из консоли.");
                            Console.WriteLine($"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.");

                            programMode = ReadTypesFromConsole.ReadProgramModeFromConsole();

                            Order secondOrder = new();

                            while (true)
                            {
                                if (programMode == ProgramModes.ReadOrderFromConsole)
                                {
                                    Console.WriteLine("Считывание заказа из консоли.");
                                    orderHandlers = new(userOrder);
                                    orderHandlers.ReadOrderFromConsole();
                                    Console.WriteLine("Считывание заказа из консоли завершено.");
                                }
                                else if (programMode == ProgramModes.ReadOrderFromFile)
                                {
                                    Console.WriteLine("Считывание заказа из файла.");
                                    orderHandlers = new(userOrder);
                                    orderHandlers.ReadOrderFromFile();
                                    Console.WriteLine("Считывание заказа из файла завершено.");
                                }
                                else
                                {
                                    Console.WriteLine($"Считано {programMode}. Такого режима нет. Повторите ввод.");
                                    continue;
                                }
                                break;
                            }
                            userOrder = orderCalculator.Add(userOrder, secondOrder);
                            Console.WriteLine("Объединённый заказ.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.ReduceTheQuantityOfTheProductInOrderByOne:
                        {
                            Console.WriteLine("Удалить единицу товара из корзины.");
                            Console.WriteLine("Текущая корзина.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();
                            Console.WriteLine("Ввод товара.");
                            Console.WriteLine("Введите номер товара из списка товаров магазина.");
                            Store.PrintProductsInfo();
                            uint deletedProductNumber = ReadTypesFromConsole.ReadProductNumberFromConsole();
                            userOrder = orderCalculator.Subtract(userOrder, Store.Products[(int)deletedProductNumber]);
                            Console.WriteLine("Заказ с удалённой единицей товара.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.RemoveMatchingProducts:
                        {
                            Console.WriteLine("Удалить из первой корзины товары, которые есть во второй корзине.");
                            Console.WriteLine("Ввод второй корзины.");
                            Console.WriteLine($"{(int)ProgramModes.ReadOrderFromConsole} - считать заказ из консоли.");
                            Console.WriteLine($"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.");

                            programMode = ReadTypesFromConsole.ReadProgramModeFromConsole();

                            Order secondOrder = new();

                            while (true)
                            {
                                if (programMode == ProgramModes.ReadOrderFromConsole)
                                {
                                    Console.WriteLine("Считывание заказа из консоли.");
                                    orderHandlers = new(userOrder);
                                    orderHandlers.ReadOrderFromConsole();
                                    Console.WriteLine("Считывание заказа из консоли завершено.");
                                }
                                else if (programMode == ProgramModes.ReadOrderFromFile)
                                {
                                    Console.WriteLine("Считывание заказа из файла.");
                                    orderHandlers = new(userOrder);
                                    orderHandlers.ReadOrderFromFile();
                                    Console.WriteLine("Считывание заказа из файла завершено.");
                                }
                                else
                                {
                                    Console.WriteLine($"Считано {programMode}. Такого режима нет. Повторите ввод.");
                                    continue;
                                }
                                break;
                            }
                            userOrder = orderCalculator.Subtract(userOrder, secondOrder);
                            Console.WriteLine("Первая корзина без товаров из второй корзины.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.RemoveProductsFromOrderByType:
                        {
                            Console.WriteLine("Удалить из корзины товары указанного типа.");
                            Console.WriteLine("Список типов товаров магазина.");
                            Store.PrintProductsTypes();
                            Console.WriteLine("Введите номер типа товара из списка товаров магазина.");
                            uint productType = ReadTypesFromConsole.ReadProductTypeNumberFromConsole();
                            userOrder = orderCalculator.Divide(userOrder, Store.Products[(int)productType - 1].GetType());
                            Console.WriteLine($"Корзина без товаров типа {Store.Products[(int)productType - 1].GetType()}.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.ReduceTheQuantityOfEachProductInOrderByNumberTimes:
                        {
                            Console.WriteLine("Уменьшить в корзине каждое количество товара в указанное число раз.");
                            Console.WriteLine("Список товаров в заказе.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();
                            Console.WriteLine("Введите число, указывающее во сколько раз уменьшить количество товара.");
                            uint number = ReadTypesFromConsole.ReadUintFromConsole();
                            userOrder = orderCalculator.Divide(userOrder, number);
                            Console.WriteLine($"Корзина с уменьшенным количеством товаров в {number} раз.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.IncreaseTheQuantityOfEachProductInOrderByNumberTimes:
                        {
                            Console.WriteLine("Увеличить в корзине каждое количество товара в указанное число раз.");
                            Console.WriteLine("Список товаров в заказе.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();
                            Console.WriteLine("Введите число, указывающее во сколько раз увеличить количество товара.");
                            uint number = ReadTypesFromConsole.ReadUintFromConsole();
                            userOrder = orderCalculator.Multiply(userOrder, number);
                            Console.WriteLine($"Корзина с уменьшенным количеством товаров в {number} раз.");
                            orderHandlers = new(userOrder);
                            orderHandlers.PrintOrderInfo();

                            break;
                        }
                    case ProgramModes.GetOrdersByMaxSum:
                        {
                            Console.WriteLine("Заказы дешевле заданной суммы.");
                            Console.WriteLine("Введите максимальную сумму заказа.");
                            decimal orderMaxSum = ReadTypesFromConsole.ReadDecimalFromConsole();
                            List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < orderMaxSum).ToList();
                            PrintOrdersList(validOrders);

                            break;
                        }
                    case ProgramModes.GetOrdersByMinSum:
                        {
                            Console.WriteLine("Заказы дороже заданной суммы.");
                            Console.WriteLine("Введите минимальную сумму заказа.");
                            decimal orderMinSum = ReadTypesFromConsole.ReadDecimalFromConsole();
                            List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > orderMinSum).ToList();
                            PrintOrdersList(validOrders);

                            break;
                        }
                    case ProgramModes.GetOrdersByProductType:
                        {
                            Console.WriteLine("Заказы, имеющие в составе товары определённого типа.");
                            Console.WriteLine("Выберите номер типа продукта из списка");
                            Store.PrintProductsTypes();
                            while (true)
                            {
                                uint productTypeNumber = ReadTypesFromConsole.ReadUintFromConsole();
                                if (productTypeNumber <= Store.ProductsTypes.Count)
                                {
                                    List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.Any(orderItem => orderItem.Key.GetType() == typeof(Corvalol))).ToList();
                                    PrintOrdersList(validOrders);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Нет такого типа товара. Повторите ввод.");
                                    continue;
                                }
                            }

                            break;
                        }
                    case ProgramModes.GetOrdersSortedByWeight:
                        {
                            Console.WriteLine("Заказы, отсортированные по весу в порядке возрастания.");
                            List<Order> validOrders = OrdersGenerator.Orders.OrderBy(order => order.Products.Sum(orderItem => orderItem.Key.Weight)).ToList();
                            PrintOrdersList(validOrders);

                            break;
                        }
                    case ProgramModes.GetOrdersWithUniqueProductsInList:
                        {
                            Console.WriteLine("Заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).");
                            //Заказы с уникальными названиями.
                            //validOrders = orders.Select(order => order.DistinctBy(orderItem => orderItem.Key.Name).ToDictionary()).ToList();
                            List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.Products.All(orderItem => orderItem.Value == 1)).ToList();
                            PrintOrdersList(validOrders);

                            break;
                        }
                    case ProgramModes.GetOrdersByMaxDepartureDate:
                        {
                            Console.WriteLine("Заказы, отправленные до указанной даты.");
                            Console.WriteLine("Введите дату в формате ДД/ММ/ГГГГ.");
                            DateTime? userDateTime = ReadTypesFromConsole.ReadDateFromConsole();
                            List<Order> validOrders = OrdersGenerator.Orders.Where(order => order.TimeOfDeparture <= userDateTime).ToList();
                            PrintOrdersList(validOrders);

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
        public static void PrintOrdersList(List<Order> orders)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                Console.WriteLine($"Заказ {i + 1}");
                OrderHandlers orderHandlers = new(orders[i]);
                orderHandlers.PrintOrderInfo();
            }
        }
    }
}