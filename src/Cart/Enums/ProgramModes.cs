namespace Cart.Enums;

internal enum ProgramModes
{
    /// <summary>
    /// Считать заказ из консоли.
    /// </summary>
    ReadOrderFromConsole = 1,
    /// <summary>
    /// Считать заказ из файла.
    /// </summary>
    ReadOrderFromFile,
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
    /// Вывести в консоль существующие товары магазина.
    /// </summary>
    PrintProducts,
    /// <summary>
    /// Вывести в консоль существующие заказы.
    /// </summary>
    PrintOrders,
    /// <summary>
    /// Рассчитать заказы.
    /// </summary>
    CalculateOrders,
    //LINQ
    GetOrdersByMaxSum,
    GetOrdersByMinSum,
    GetOrdersByProductType,
    GetOrdersSortedByWeight,
    GetOrdersWithUniqueProductsInList,
    GetOrdersByMaxDepartureDate

}
