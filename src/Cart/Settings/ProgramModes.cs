namespace Cart.Settings;

/// <summary>
/// Режимы работы программы.
/// </summary>
public enum ProgramModes
{
    /// <summary>
    /// Завершить работу.
    /// </summary>
    Exit = 0,
    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    ReadOrderFromConsole,
    /// <summary>
    /// Считать заказ из файла.
    /// </summary>
    ReadOrderFromFile,
    /// <summary>
    /// Сгенерировать случайный заказ.
    /// </summary>
    GenerateRandomOrder,
    /// <summary>
    /// Сгенерировать заказ по максимальной сумме.
    /// </summary>
    GenerateOrderByMaxSum,
    /// <summary>
    /// Сгенерировать заказ по диапазону суммы.
    /// </summary>
    GenerateOrderByMinMaxSumRange,
    /// <summary>
    /// Сгенерировать заказ по максимальному общему количеству товаров в заказе.
    /// </summary>
    GenerateOrderByMaxTotalQuantity,
    /// <summary>
    /// Изменить товар в заказе.
    /// </summary>
    ChangeProductInOrder,
    /// <summary>
    /// Вывести в консоль предсгенерированных товаров магазина.
    /// </summary>
    PrintProducts,
    /// <summary>
    /// Вывести в консоль предсгенерированных заказов.
    /// </summary>
    PrintOrders,
    /// <summary>
    /// Создать корзину с двумя указанными товарами. 
    /// </summary>
    CreateOrderFromTwoProducts,
    /// <summary>
    /// Добавить товар в корзину.
    /// </summary>
    AddProductToOrder,
    /// <summary>
    /// Объединить корзины.
    /// </summary>
    CombineTwoOrders,
    /// <summary>
    /// Удалить единицу товара из корзины.
    /// </summary>
    ReduceTheQuantityOfTheProductInOrderByOne,
    /// <summary>
    /// Удалить из первой корзины товары, которые есть во второй корзине.
    /// </summary>
    RemoveMatchingProducts,
    /// <summary>
    /// Удалить из корзины товары указанного типа.
    /// </summary>
    RemoveProductsFromOrderByType,
    /// <summary>
    /// Уменьшить в корзине каждое количество товара в указанное число раз.
    /// </summary>
    DivideTheQuantityOfEachProductInOrder,
    /// <summary>
    /// Увеличить в корзине каждое количество товара в указанное число раз.
    /// </summary>
    MultiplyTheQuantityOfEachProductInOrder,
    /// <summary>
    /// Получить заказы дешевле заданной суммы.
    /// </summary>
    GetOrdersByMaxSum,
    /// <summary>
    /// Получить заказы дороже заданной суммы.
    /// </summary>
    GetOrdersByMinSum,
    /// <summary>
    /// Получить заказы, имеющие в составе товары определённого типа.
    /// </summary>
    GetOrdersByProductType,
    /// <summary>
    /// Получить заказы, отсортированные по весу в порядке возрастания.
    /// </summary>
    GetOrdersSortedByWeight,
    /// <summary>
    /// Получить заказы с уникальными названиями (заказы, в которых количество каждого товара не превышает единицы).
    /// </summary>
    GetOrdersWithUniqueProductsInList,
    /// <summary>
    /// Получить заказы, отправленные до указанной даты.
    /// </summary>
    GetOrdersByMaxDepartureDate,
    /// <summary>
    /// Обновить данные о товаре в заказе.
    /// </summary>
    UpdateProductInOrder,
    /// <summary>
    /// Отсортировать заказ по алфавиту.
    /// </summary>
    SortOrderByNames,
    /// <summary>
    /// Записать заказ в файл.
    /// </summary>
    WriteOrderToFile
}