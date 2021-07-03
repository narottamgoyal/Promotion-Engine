using PromotionEngineApp.Model;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PromotionEngineApp.Tests
{
    public class CartUnitTest
    {
        [Fact]
        public void AddProduct_GivenOutOfStockProduct_ShouldNotAddOutOfStockProductToTheCart()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var cart = new Cart(skuList);
            cart.AddProduct(new Product { Quantity = 1, SKU_Id = 'A' });
            cart.AddProduct(new Product { Quantity = 1, SKU_Id = 'X' });

            // Act
            var result = cart.GetAddedItems();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void AddProduct_GivenInStockProduct_ShouldAddProductToTheCart()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var cart = new Cart(skuList);
            cart.AddProduct(new Product { Quantity = 1, SKU_Id = 'A' });
            cart.AddProduct(new Product { Quantity = 2, SKU_Id = 'B' });

            // Act
            var result = cart.GetAddedItems();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAddedItems_GivenNoProductInCard_ShouldReturnEmptyCart()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var cart = new Cart(skuList);

            // Act
            var result = cart.GetAddedItems();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAddedItems_GivenMultipleProductWithSameSKU_IDInCard_ShouldReturnCartWithGroupedQuantity()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var cart = new Cart(skuList);
            cart.AddProduct(new Product { Quantity = 1, SKU_Id = 'A' });
            cart.AddProduct(new Product { Quantity = 2, SKU_Id = 'B' });
            cart.AddProduct(new Product { Quantity = 1, SKU_Id = 'C' });
            cart.AddProduct(new Product { Quantity = 2, SKU_Id = 'B' });

            // Act
            var result = cart.GetAddedItems();

            // Assert
            Assert.Equal(4, result.First(x => x.SKU_Id == 'B').Quantity);
        }
    }
}
