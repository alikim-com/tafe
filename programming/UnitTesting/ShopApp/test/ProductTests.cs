namespace test;

public class ProductTests
    {
        [Fact]
        public void ProductConstructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            int expectedId = 1;
            string expectedName = "Test Product";
            decimal expectedPrice = 10.99m;

            // Act
            Product product = new Product(expectedId, expectedName, expectedPrice);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedId, product.Id),
                () => Assert.Equal(expectedName, product.Name),
                () => Assert.Equal(expectedPrice, product.UnitPrice)
            );
        }

        [Theory]
        [InlineData(-10.0)]
        [InlineData(0)]
        public void ProductConstructor_PriceLessEqualToZero(decimal _price)
        {
            // Arrange
            int expectedId = 1;
            string expectedName = "Test Product";
 
            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Product(expectedId, expectedName, _price));

            // Assert
            Assert.Equal("Invalid price; value must be greater than 0", ex.Message);
        }
    }