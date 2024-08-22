namespace Cart.Orders;

internal interface IPrintOrder
{
    public void Print(Order order, string title = "");
}
