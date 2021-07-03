using PromotionEngineApp.Exceptions;
using PromotionEngineApp.Model;
using System.Collections.Generic;
using Xunit;

namespace PromotionEngineApp.Tests
{
    public class PromotionalOfferUnitTest
    {
        [Fact]
        public void AddOffer_GivenOffer_ShouldReturnAddedOffer()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var promotionalOffer = new PromotionalOffer(skuList);
            promotionalOffer.AddOffer(new Offer { Price = 1, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'A' } } });

            // Act
            var result = promotionalOffer.GetPromotions();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void AddOffer_GivenDuplicateOffer_ShouldNotAddOffer()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var promotionalOffer = new PromotionalOffer(skuList);
            promotionalOffer.AddOffer(new Offer { Price = 1, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'A' }, new Product { Quantity = 1, SKU_Id = 'B' } } });
            promotionalOffer.AddOffer(new Offer { Price = 1, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'C' } } });

            // Act
            void action() => promotionalOffer.AddOffer(new Offer { Price = 2, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'A' }, new Product { Quantity = 1, SKU_Id = 'B' } } });

            // Assert
            var caughtException = Assert.Throws<DuplicateOfferException>(action);
            Assert.Contains("DuplicateOfferException", caughtException.Message);
        }

        [Fact]
        public void AddOffer_GivenOfferWithOutOfStockProduct_ShouldNotAddOffer()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var promotionalOffer = new PromotionalOffer(skuList);
            promotionalOffer.AddOffer(new Offer { Price = 1, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'A' } } });
            promotionalOffer.AddOffer(new Offer { Price = 1, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'X' } } });
            promotionalOffer.AddOffer(new Offer { Price = 1, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'C' } } });

            // Act
            var result = promotionalOffer.GetPromotions();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
