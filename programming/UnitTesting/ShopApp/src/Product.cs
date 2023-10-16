namespace ShopApp;

public class Product
{
   public int Id { get; set; }
   public string Name { get; set; } = "";
   public decimal UnitPrice { get; set; }

   public Product(int id, string name, decimal price)
   {
      Id = id;
      Name = name;
      if(price <= 0) throw new ArgumentOutOfRangeException(null, null, "Invalid price; value must be greater than 0");
      UnitPrice = price;
   }
}
