using System.Reflection;

namespace Cart;
public class Cart
{
    public List<Product> Products;

    public Cart()
    {
        Products = new();
    }

    public Cart(List<Product> products)
    {
        Products = products;
    }

    public double GetTotalWeight()
    {
        double totalWeight = 0;
        
        foreach (Product product in Products)
        {
            totalWeight += product.Weight;
        }

        return totalWeight;
    }

    public decimal GetTotalPrice()
    {
        decimal totalPrice = 0;

        foreach (Product product in Products)
        {
            totalPrice += product.Price;
        }

        return totalPrice;
    }

    public Dictionary<ulong, Dictionary<object, string?>> GetInfo()
    {
        //TKey - номер товара в корзине, TValue - словарь качеств товара.
        //TKey вложенного словаря - наименоване свойства. TValue вложенное словаря - значение свойства.
        Dictionary<ulong, Dictionary<object, string?>> cartInfo = new(); 
        uint productNumber = 0;
        foreach (Product product in Products)
        {
            productNumber += 1;
            Dictionary<object, string?> propertiesInfo = new(); //TKey - наименоване свойства, значение свойства.
            foreach (PropertyInfo propertyInfo in product.GetType().GetProperties())
            {
                propertiesInfo.Add(propertyInfo.Name, propertyInfo.GetValue(product)?.ToString());
            }

            cartInfo.Add(productNumber, propertiesInfo);
        }
         
        return cartInfo;
    }
}