namespace Cart.Orders
{
    internal class PrintOrderToAPI : IPrintOrder
    {
        public virtual void Print(Order order, string title = "")
        {
            //APIOrder.post(string response);
        }
    }
}
