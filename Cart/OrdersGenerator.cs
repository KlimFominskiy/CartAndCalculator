using System.Text.Json;

namespace Cart;

public class OrdersGenerator
{
    public List<Product> Products { get; set; } = new();
    //static ulong productId = 0;

    /// <summary>
    /// Создаёт случайные наборы заказов из существующих товаров.
    /// </summary>
    public void GenerateRandomOrders()
    {
        Cart cart = new();
        Random random = new();

        string fileNameProducts = "Products.json";
        string fileNameOrders = "Orders.json";
        string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string fileContentProducts = File.ReadAllText(filePath + "\\" + fileNameProducts);
        Products = JsonSerializer.Deserialize<List<Product>>(fileContentProducts);
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
        for (int i = 0; i < 5; i++)
        {
            foreach (Product product in Products)
            {
                if (random.Next(0, 2) > 0)
                {
                    cart.Products.Add(product, 1);
                }
            }
            File.AppendAllText(filePath + "\\" + fileNameOrders, JsonSerializer.Serialize<List<Product>>(cart.Products.Keys.ToList(), options));
            cart.Products.Clear();
        }
    }

    /// <summary>
    /// Сформировать (выбрать) случайный заказ.
    /// </summary>
    /// <returns></returns>
    public Cart GenerateRandomOrder()
    {
        Random random = new();
        Cart cart = new();

        string fileNameProducts = "Products.json";
        string fileNameOrders = "Orders.json";
        string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string fileContentProducts = File.ReadAllText(filePath + "\\" + fileNameProducts);
        List<List<Product>> orders = JsonSerializer.Deserialize<List<List<Product>>>(fileContentProducts);
        int order = random.Next(0, orders.Count + 1);
        foreach (Product product in orders[order])
        {
            cart.Products.Add(product, Convert.ToUInt64(random.Next(0, 5)));
        }

        return cart;
    }

    public Cart GenerateOrderBySum(double maxSum)
    {
        Random random = new();
        Cart cart = new();

        foreach(Product product in Products)
        {
            if (random.Next(0, 2) > 0)
            {
                cart.Products.Add(product, 1);
            }
        }

        return cart;
    }
}
