// Классы таблиц: Customer, Product, Order (шаги 1–2).
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
    public List<Order> Orders { get; set; } = new();
}

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public decimal Price { get; set; }
    public int QuantityBox { get; set; }
    public int QuantityStock { get; set; }
    public string? Description { get; set; }
    /*
    Оба столбца имеют тип text но они отличаются тем,
    что Title указан как обязательное поле, а Description может иметь
    значение NULL 
    */
    public List<Order> Orders { get; set; } = new();
}

public class Order
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public DateTime OrderDate { get; set; } // Я выбрал тип DateTime и в столбце получился тип timestamp with time zone
}