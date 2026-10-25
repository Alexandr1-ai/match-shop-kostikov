using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

Console.WriteLine("Магазин спичек «Чирк»: проект запущен!");

// Дальше — код по шагам задания.
using var db = new ShopContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

var ivan = new Customer { Name = "Иван Петров", Phone = "+7 900 111-22-33", City = "Омск" };
var olga = new Customer { Name = "Ольга Смирнова", Phone = "+7 900 444-55-66", City = "Томск" };
var mikhail = new Customer { Name = "Михаил Иванов", Phone = "+7 934 444-22-11", City = "Москва" };
var dmitry = new Customer { Name = "Дмитрий Соколов", Phone = "+7 912 345-67-89", City = "Екатеринбург" };
var anna = new Customer { Name = "Анна Кузнецова", Phone = "+7 926 123-45-67", City = "Москва" };
var customers = new List<Customer> { ivan, olga, mikhail, dmitry, anna };

var ogonek = new Product { Title = "Спички хозяйственные «Огонёк»", Price = 15m, QuantityBox = 50, QuantityStock = 1200, Description = "Классические спички для повседневного использования - плита, свечи, печь" };
var plamy = new Product { Title = "Спички каминные Extra Long «Пламя»", Price = 120m, QuantityBox = 40, QuantityStock = 340 };
var storm = new Product { Title = "Спички туристические водонепроницаемые «Storm»", Price = 95m, QuantityBox = 30, QuantityStock = 560 };
var vanil = new Product { Title = "Спички декоративные ароматические «Ваниль»", Price = 180m, QuantityBox = 25, QuantityStock = 210 };
var tayga = new Product { Title = "Спички охотничьи «Тайга» (горящее пламя)", Price = 70m, QuantityBox = 60, QuantityStock = 780, Description = "Спички с усиленной головкой и повышенной температурой горения — быстро разжигают сырые дрова и хворост" };
var products = new List<Product> { ogonek, plamy, storm, vanil, tayga };

var orders = new List<Order>
{
    new () {Quantity = 2, Customer = ivan, Product = storm, OrderDate = new DateTime(2025, 12, 8, 10, 0, 0, DateTimeKind.Utc)},
    new () {Quantity = 4, Customer = ivan, Product = plamy, OrderDate = new DateTime(2025, 12, 9, 14, 0, 0, DateTimeKind.Utc)},
    new () {Quantity = 3, Customer = olga, Product = ogonek, OrderDate = new DateTime(2025, 12, 10, 12, 0, 0, DateTimeKind.Utc)},
    new () {Quantity = 1, Customer = mikhail, Product = tayga, OrderDate = new DateTime(2025, 12, 11, 9, 0, 0, DateTimeKind.Utc)},
    new () {Quantity = 6, Customer = dmitry, Product = vanil, OrderDate = new DateTime(2025, 12, 12, 18, 0, 0, DateTimeKind.Utc)}
    /*
    Для того что сказать заказу кто покупатель и какой товар, вместо прямого указания id, 
    можно указать объект
    */

    /*
    DateTime.Now возвращает текущее локальное время компьютера и имеет свойство Kind = Local
    DateTime.UtcNow возвращает текущее время по UTC и меет свойство Kind = Utc
    */
};

db.Customers.AddRange(customers);
db.Products.AddRange(products);
db.Orders.AddRange(orders);
db.SaveChanges();

Console.WriteLine("=== Шаг 4 ===");

var omsk = db.Customers
    .Where(c => c.City == "Омск")
    .OrderBy(c => c.Name)
    .ToList();

var cheapMatches = db.Products
    .Where(p => p.Price < 100)
    .OrderBy(p => p.Price)
    .ToList();

var typesMatches = db.Products.Count(qs => qs.QuantityStock > 0);

var expensiveGoods = db.Products
    .OrderByDescending(p => p.Price)
    .Take(3)
    .ToList();

var total_price = db.Orders
    .Select(o => new
    {
        CustomerName = o.Customer.Name,
        ProductTitle = o.Product.Title,
        QuantityProduct = o.Quantity,
        TotalPrice = o.Product.Price * o.Quantity
    })
    .ToList();

var totalGrand = total_price.Sum(tp => tp.TotalPrice);

foreach (var c in omsk)
    Console.WriteLine($"{c.Name}, {c.Phone}");

foreach (var p in cheapMatches)
    Console.WriteLine($"{p.Title}: {p.Price}");

Console.WriteLine($"Количество видов спичек на складе: {typesMatches}");

foreach (var p in expensiveGoods)
    Console.WriteLine($"{p.Title}: {p.Price}");

foreach (var o in total_price)
    Console.WriteLine($"{o.CustomerName} - {o.ProductTitle} - {o.QuantityProduct} шт. - {o.TotalPrice} ₽");

Console.WriteLine($"Общая выручка: {totalGrand}");

Console.WriteLine("=== Шаг 5 ===");

var customer = db.Customers.First(c => c.Name == "Иван Петров");
customer.Phone = "+7 900 999-00-00";

var product = db.Products.ToList();
foreach (var p in product)
    p.Price *= 1.1m;

var productZero = db.Products.First(p => p.Title.Contains("Тайга"));
productZero.QuantityStock = 0;

void PlaceAnOrder(Customer cust, Product prod, int quantity)
{
    if (prod.QuantityStock >= quantity)
    {
        var order = new Order
        {
            Quantity = quantity,
            Customer = cust,
            Product = prod,
            OrderDate = DateTime.UtcNow
        };

        db.Orders.Add(order);
        prod.QuantityStock -= quantity;
        Console.WriteLine("Заказ оформлен:\n" +
        $"Заказчик: {cust.Name}\n" +
        $"В заказе: {prod.Title}\n"+
        $"Количество: {quantity}");
    }
    else
    {
        Console.WriteLine("Недостаточно на складе");
    }
}

PlaceAnOrder(mikhail, plamy, 3);
PlaceAnOrder(ivan, tayga, 10);

db.SaveChanges();
/*
Если сохранять заказ и новый остаток то При сбое между ними может произойти ситуация, 
что заказ был оформлен, но количество на складе не изменилось и наоборот
*/

Console.WriteLine("=== Шаг 6 ===");

var leaving = db.Customers.First(c => c.Name == "Ольга Смирнова");
db.Customers.Remove(leaving);

var notInStock = db.Products.Where(p => p.QuantityStock == 0).ToList();
db.Products.RemoveRange(notInStock);

db.Customers.Remove(ivan);

db.SaveChanges();
Console.WriteLine($"Количество оставшихся товаров: {db.Products.Count()}");
/*
Все его заказы удалились вместе с покупалетел
Так получилось потому, что EF Core создал вмнешний ключ c ON DELETE CASCADE,
и при удалении покупателя удаляются и все его заказы
Для магазина это очень плохо потому что могут начаться проблемы с финансовыми документами
и данные о продаже товара пропадают
*/