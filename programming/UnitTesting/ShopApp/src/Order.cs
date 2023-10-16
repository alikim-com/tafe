namespace ShopApp;

public class Order
{
   public DateTime Date { get; set; }
   public int Number { get; set; }
   public List<OrderLineItem> LineItems = new();

   public Order (int number) {
      
      Number = number;
   }

   public OrderLineItem AddLineItem(Product product, int quantity)
   {
      if(product == null) throw new ArgumentNullException(null, "Can't add null product");
      
      var item = LineItems.Find(item => item.ProductId == product.Id);
      if(item != null) {
         item.Quantity += quantity;
         return item;
      }

      item = new OrderLineItem(product, quantity);

      LineItems.Add(item);

      return item;
   }
}
