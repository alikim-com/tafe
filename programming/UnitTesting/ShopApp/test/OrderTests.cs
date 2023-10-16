namespace test;

public class OrderTests
    {
        // dotnet test --filter DisplayName~OrderLineItemTest

        [Fact]
        public void OrderTest_AddLineItemProductIsNull()
        {
            // Arrange
            Order ord = new Order(123);
 
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => ord.AddLineItem(null, 1));

            // Assert
            Assert.Equal("Can't add null product", ex.Message);
        }

        [Fact]
        public void OrderTest_AddLineItemCollectionCount()
        {
            // Arrange
            Order ord = new Order(123);
            Product product = new Product(1, "Name 1", 10.35m);
            int expectedCount = ord.LineItems.Count + 1;
 
            // Act
            ord.AddLineItem(product, 1);

            // Assert
            Assert.Equal(expectedCount, ord.LineItems.Count);
        }

        [Theory]
        [InlineData(10, 20)]
        public void OrderTest_AddLineItemExistingProductId(int quantity1, int quantity2)
        {
            // Arrange
            Order ord = new Order(123);
            Product product = new Product(1, "Name 1", 10.35m);
             
            // Act
            ord.AddLineItem(product, quantity1);
            int expectedCount = ord.LineItems.Count;
            var item = ord.LineItems.Find(item => item.ProductId == 1);
            int expectedQuantity = item.Quantity;
            ord.AddLineItem(product, quantity2);
            expectedQuantity += quantity2;

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedCount, ord.LineItems.Count),
                () => Assert.Equal(expectedQuantity, item.Quantity)
            );
        }
    }
    