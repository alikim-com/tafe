namespace test;

public class OrderLineItemTest
    {
        // dotnet test --filter DisplayName~OrderLineItemTest

        [Theory]
        [InlineData(1, "Product name 1", 10.35, -10.0)]
        [InlineData(2, "Product name 2", 10.35,     0)]
        public void OrderLineItemTest_QuantityLessEqualToZero(
            int id, string name, decimal price, int _quantity
        )
        {
            // Arrange
            Product product = new Product(id, name, price);
 
            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new OrderLineItem(product, _quantity));

            // Assert
            Assert.Equal("Invalid quantity; value must be greater than 0", ex.Message);
        }

        [Theory]
        [InlineData(1, "Product name 1", 10.35, 3)]
        [InlineData(2, "Product name 2", 3.67,  5)]
        [InlineData(3, "Product name 3", 3.67,  15)]
        public void OrderLineItemTest_TotalCheck(
            int id, string name, decimal price, int quantity
        )
        {
            // Arrange
            Product product = new Product(id, name, price);
 
            // Act
            var item = new OrderLineItem(product, quantity);

            // Assert
            Assert.Equal(price * quantity, item.Total);
        }
    }