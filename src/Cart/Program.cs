using Cart.Enums;
using Cart.Orders;
using Cart.Products;
using Cart.Stores;
using System.Text;

namespace Cart;

internal static class Program
{
    public static void PrintOrdersList(List<Order> orders)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Console.WriteLine($"Заказ {i + 1}");
            orders[i].PrintOrderInfo();
        }
    }

    internal static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Задание 2. Считывание товаров из файла.
        //Console.WriteLine("Генерация товаров в магазине.");
        //Store.GenerateProducts();
        //Console.WriteLine("Товары в магазине сгенерированы.");
        Console.WriteLine("Считывание продуктов из файла.");
        Store.ReadProductsFromFile();
        Console.WriteLine("Продукты считаны.");
        Console.WriteLine();

        // Задание 4. Генератор тестовых заказов.
        //Console.WriteLine("Генерация заказов");
        //OrdersGenerator.GenerateRandomOrders();
        //Console.WriteLine("Заказы сгенерированы.");
        Console.WriteLine("Считывание заказов из файла.");
        OrdersGenerator.ReadOrdersFromFile();
        Console.WriteLine("Заказы считаны.");
        Console.WriteLine();

        Order userOrder = new();

        while (true)
        {
            Console.WriteLine("Выберите режим работы программы:");
            Console.WriteLine("Считать заказ:");
            Console.WriteLine($"{(int)ProgramModes.ReadOrderFromConsole} - считать заказ из консоли.");
            Console.WriteLine($"{(int)ProgramModes.ReadOrderFromFile} - считать заказ из файла.");

            decimal minSum = decimal.MaxValue;
            decimal maxSum = 0;
            uint minTotalQuantity = uint.MaxValue;
            uint maxTotalQuantity = 0;
            foreach (Order order in OrdersGenerator.Orders)
            {
                decimal sum = (decimal)order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value);
                if (minSum > sum)
                {
                    minSum = sum;
                }
                if (maxSum < sum)
                {
                    maxSum = sum;
                }
                uint quantity = (uint)order.Products.Sum(orderItem => orderItem.Value);
                if (minTotalQuantity > quantity)
                {
                    minTotalQuantity = quantity;
                }
                if (maxTotalQuantity < quantity)
                {
                    maxTotalQuantity = quantity;
                }
            }
            Console.WriteLine($"{(int)ProgramModes.GenerateRandomOrder} - сгенерировать случайный заказ");
            Console.WriteLine("Сгенерировать заказ по сумме:");
            Console.WriteLine($"{(int)ProgramModes.GenerateOrderByMaxSum} - сгенерировать заказ по максимальной сумме.");
            Console.WriteLine($"{(int)ProgramModes.GenerateOrderByMinMaxSumRange} - cгенерировать заказ по диапазону суммы.");
            Console.WriteLine($"Минимальная общая сумма заказа - {minSum}.");
            Console.WriteLine($"Максимальная общая сумма заказа - {maxSum}.");
            Console.WriteLine($"{(int)ProgramModes.GenerateOrderByMaxTotalQuantity} - cгенерировать заказ по максимальному общему количеству товаров в заказе.");
            Console.WriteLine($"Минимальное общее количество товаров в заказе - {minTotalQuantity}.");
            Console.WriteLine($"Максимальное общее количество товаров в заказе - {maxTotalQuantity}.");

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

            Console.WriteLine($"Отсортировать заказы:");
            // Задание 5. Работа с LINQ.
            List<Order> validOrders = [];
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByMaxSum} - заказы дешевле заданной суммы.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByMinSum} - заказы дороже заданной суммы.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByProductType} - заказы, имеющие в составе товары определённого типа.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersSortedByWeight} - заказы, отсортированные по весу в порядке возрастания.");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersWithUniqueProductsInList} - заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).");
            Console.WriteLine($"{(int)ProgramModes.GetOrdersByMaxDepartureDate} - заказы, отправленные до указанной даты.");
            Console.WriteLine("0 - закончить работу программы.");

            decimal userMaxOrderSum;
            decimal userMinOrderSum;
            uint userMaxOrderQuantity;
            OrderCalculator orderCalculator = new(new Calculator.Logger());

            ProgramModes programMode = ReadTypesFromConsole.ReadProgramModeFromConsole();

            switch (programMode)
            {
                case ProgramModes.Exit:
                    break;
                //Задание 1. Ввод заказа с помощью консоли.
                case ProgramModes.ReadOrderFromConsole:
                    {
                        Console.WriteLine("Типы товаров в магазине.");
                        Store.PrintProductsTypes();

                        Console.WriteLine("Считывание заказа из консоли.");
                        Console.WriteLine("Введите номер товара в списке продуктов.");
                        userOrder.ReadOrderFromConsole();
                        Console.WriteLine("Считывание заказа из консоли завершено.");
                        Console.WriteLine("Информация о заказе.");
                        userOrder.PrintOrderInfo();
                        Console.WriteLine("Отсортироваться заказ по алфавиту? y - да, любой другой символ - нет.");
                        if (Console.ReadLine() == "y")
                        {
                            //Задание 1. Отсортировать по алфавиту без LINQ.
                            userOrder.Products.Sort((a, b) => a.Key.Name.CompareTo(b.Key.Name));
                        }
                        Console.WriteLine("Записать заказ в файл? y - да, любой другой символ - нет.");
                        if (Console.ReadLine() == "y")
                        {
                            Console.WriteLine("Запись заказ в файл");
                            userOrder.WriteOrderToFile();
                            Console.WriteLine("Запись заказа в файл окончена.");
                        }

                        continue;
                    }
                case ProgramModes.ReadOrderFromFile:
                    {
                        Console.WriteLine("Считывание заказа из файла.");
                        userOrder = userOrder.ReadOrderFromFile();
                        Console.WriteLine("Считывание заказа из файла завершено.");

                        Console.WriteLine("Информация о заказе.");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.GenerateRandomOrder:
                    {
                        Console.WriteLine("Генерация случайного заказа.");
                        userOrder = OrdersGenerator.GenerateRandomOrder();
                        Console.WriteLine("Сгенерированный заказ.");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.GenerateOrderByMaxSum:
                    {
                        Console.WriteLine("Введите максимальную сумму заказа.");
                        userMaxOrderSum = ReadTypesFromConsole.ReadDecimalFromConsole();

                        userOrder = OrdersGenerator.GenerateOrderBySum(userMaxOrderSum);

                        Console.WriteLine("Информация о заказе.");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.GenerateOrderByMinMaxSumRange:
                    {
                        Console.WriteLine("Введите минимальную сумму заказа.");
                        userMinOrderSum = ReadTypesFromConsole.ReadDecimalFromConsole();
                        Console.WriteLine("Введите максимальную сумму заказа.");
                        userMaxOrderSum = ReadTypesFromConsole.ReadDecimalFromConsole();

                        userOrder = OrdersGenerator.GenerateOrderBySum(userMinOrderSum, userMaxOrderSum);

                        Console.WriteLine("Информация о заказе.");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.GenerateOrderByMaxTotalQuantity:
                    {
                        Console.WriteLine("Введите максимальное общее количество товаров в заказе.");
                        userMaxOrderQuantity = ReadTypesFromConsole.ReadUintFromConsole();

                        userOrder = OrdersGenerator.GenerateOrderByMaxQuantity(userMaxOrderQuantity);

                        Console.WriteLine("Информация о заказе.");
                        userOrder.PrintOrderInfo();
                        continue;
                    }
                case ProgramModes.ChangeProductInOrder:
                    {
                        Console.WriteLine("Изменение товара в заказе.");
                        if (userOrder.Products.Count == 0)
                        {
                            Console.WriteLine("Для начала введите заказ.");
                            continue;
                        }
                        userOrder.UpdateProduct();
                        Console.WriteLine("Заказ обновлён");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.PrintProducts:
                    {
                        Store.PrintProductsInfo();

                        continue;
                    }
                case ProgramModes.PrintOrders:
                    {
                        OrdersGenerator.PrintOrdersInfo();

                        continue;
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
                        userOrder.PrintOrderInfo();

                        continue;
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
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.CombineTwoOrders:
                    { 
                        Console.WriteLine("Объединить корзины.");
                        Console.WriteLine("Текущая корзина.");
                        userOrder.PrintOrderInfo();

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
                                secondOrder.ReadOrderFromConsole();
                                Console.WriteLine("Считывание заказа из консоли завершено.");
                            }
                            else if (programMode == ProgramModes.ReadOrderFromFile)
                            {
                                Console.WriteLine("Считывание заказа из файла.");
                                secondOrder.ReadOrderFromFile();
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
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.ReduceTheQuantityOfTheProductInOrderByOne:
                    {
                        Console.WriteLine("Удалить единицу товара из корзины.");
                        Console.WriteLine("Текущая корзина.");
                        userOrder.PrintOrderInfo();
                        Console.WriteLine("Ввод товара.");
                        Console.WriteLine("Введите номер товара из списка товаров магазина.");
                        Store.PrintProductsInfo();
                        uint deletedProductNumber = ReadTypesFromConsole.ReadProductNumberFromConsole();
                        userOrder = orderCalculator.Subtract(userOrder, Store.Products[(int)deletedProductNumber]);
                        Console.WriteLine("Заказ с удалённой единицей товара.");
                        userOrder.PrintOrderInfo();

                        continue;
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
                                secondOrder.ReadOrderFromConsole();
                                Console.WriteLine("Считывание заказа из консоли завершено.");
                            }
                            else if (programMode == ProgramModes.ReadOrderFromFile)
                            {
                                Console.WriteLine("Считывание заказа из файла.");
                                secondOrder.ReadOrderFromFile();
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
                        userOrder.PrintOrderInfo();

                        continue;
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
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.ReduceTheQuantityOfEachProductInOrderByNumberTimes:
                    {
                        Console.WriteLine("Уменьшить в корзине каждое количество товара в указанное число раз.");
                        Console.WriteLine("Список товаров в заказе.");
                        userOrder.PrintOrderInfo();
                        Console.WriteLine("Введите число, указывающее во сколько раз уменьшить количество товара.");
                        uint number = ReadTypesFromConsole.ReadUintFromConsole();
                        userOrder = orderCalculator.Divide(userOrder, number);
                        Console.WriteLine($"Корзина с уменьшенным количеством товаров в {number} раз.");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.IncreaseTheQuantityOfEachProductInOrderByNumberTimes:
                    {
                        Console.WriteLine("Увеличить в корзине каждое количество товара в указанное число раз.");
                        Console.WriteLine("Список товаров в заказе.");
                        userOrder.PrintOrderInfo();
                        Console.WriteLine("Введите число, указывающее во сколько раз увеличить количество товара.");
                        uint number = ReadTypesFromConsole.ReadUintFromConsole();
                        userOrder = orderCalculator.Multiply(userOrder, number);
                        Console.WriteLine($"Корзина с уменьшенным количеством товаров в {number} раз.");
                        userOrder.PrintOrderInfo();

                        continue;
                    }
                case ProgramModes.GetOrdersByMaxSum:
                    {
                        Console.WriteLine("Заказы дешевле заданной суммы.");
                        Console.WriteLine("Введите максимальную сумму заказа.");
                        decimal orderMaxSum = ReadTypesFromConsole.ReadDecimalFromConsole();
                        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) < orderMaxSum).ToList();
                        PrintOrdersList(validOrders);

                        continue;
                    }
                case ProgramModes.GetOrdersByMinSum:
                    {
                        Console.WriteLine("Заказы дороже заданной суммы.");
                        Console.WriteLine("Введите минимальную сумму заказа.");
                        decimal orderMinSum = ReadTypesFromConsole.ReadDecimalFromConsole();
                        validOrders = OrdersGenerator.Orders.Where(order => order.Products.Sum(orderItem => orderItem.Key.Price * orderItem.Value) > orderMinSum).ToList();
                        PrintOrdersList(validOrders);

                        continue;
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
                                validOrders = OrdersGenerator.Orders.Where(order => order.Products.Any(orderItem => orderItem.Key.GetType() == typeof(Corvalol))).ToList();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Нет такого типа товара. Повторите ввод.");
                                continue;
                            }
                        }
                        PrintOrdersList(validOrders);

                        continue;
                    }
                case ProgramModes.GetOrdersSortedByWeight:
                    {
                        Console.WriteLine("Заказы, отсортированные по весу в порядке возрастания.");
                        validOrders = OrdersGenerator.Orders.OrderBy(order => order.Products.Sum(orderItem => orderItem.Key.Weight)).ToList();
                        PrintOrdersList(validOrders);

                        continue;
                    }
                case ProgramModes.GetOrdersWithUniqueProductsInList:
                    {
                        Console.WriteLine("Заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).");
                        //Заказы с уникальными названиями.
                        //validOrders = orders.Select(order => order.DistinctBy(orderItem => orderItem.Key.Name).ToDictionary()).ToList();
                        validOrders = OrdersGenerator.Orders.Where(order => order.Products.All(orderItem => orderItem.Value == 1)).ToList();
                        PrintOrdersList(validOrders);

                        continue;
                    }
                case ProgramModes.GetOrdersByMaxDepartureDate:
                    {
                        Console.WriteLine("Заказы, отправленные до указанной даты.");
                        Console.WriteLine("Введите дату в формате ДД/ММ/ГГГГ.");
                        DateTime? userDateTime = ReadTypesFromConsole.ReadDateFromConsole();
                        validOrders = OrdersGenerator.Orders.Where(order => order.TimeOfDeparture <= userDateTime).ToList();
                        PrintOrdersList(validOrders);

                        continue;
                    }
            }

            return;
        }
    }
}