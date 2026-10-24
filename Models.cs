// Классы таблиц: Customer, Product, Order (шаги 1–2).
// Классы таблиц: Customer, Product, Order (шаги 1–2).
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
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
}