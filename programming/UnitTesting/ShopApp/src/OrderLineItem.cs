namespace ShopApp;

public class OrderLineItem
{
   public int ProductId { get; set; }
   public decimal UnitPrice { get; set; }
   public int Quantity { get; set; }

   public decimal Total => UnitPrice * Quantity;

   public OrderLineItem(Product product, int quantity)
   {
      ProductId = product.Id;
      UnitPrice = product.UnitPrice;
      if (quantity <= 0) throw new ArgumentOutOfRangeException(null, null, "Invalid quantity; value must be greater than 0");
      Quantity = quantity;
   }

}
