using PromotionEngineApp.Model;
using System.Collections.Generic;
using Xunit;

namespace PromotionEngineApp.Tests
{
    public class CashierUnitTest
    {
        [Fact]
        public void CheckOut_GivenCart_ShouldReturnTotalCostOfTheCartItems()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var promotionalOffer = new PromotionalOffer(skuList);

            var cart = new Cart(skuList);
            cart.AddProduct(new Product { Quantity = 4, SKU_Id = 'A' });

            var cashier = new Cashier(cart, promotionalOffer, skuList);

            // Act
            var result = cashier.CheckOut();

            // Assert
            Assert.Equal(200, result);
        }


        [Fact]
        public void CheckOut_GivenCartWithItemEligibleUnderPromotion_ShouldReturnTotalCostOfTheCartItemsWithDiscount()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var promotionalOffer = new PromotionalOffer(skuList);
            promotionalOffer.AddOffer(new Offer { Price = 130, Products = new List<Product> { new Product { Quantity = 3, SKU_Id = 'A' } } });
            promotionalOffer.AddOffer(new Offer { Price = 30, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'C' }, new Product { Quantity = 1, SKU_Id = 'D' } } });

            var cart = new Cart(skuList);
            cart.AddProduct(new Product { Quantity = 4, SKU_Id = 'A' });
            cart.AddProduct(new Product { Quantity = 4, SKU_Id = 'B' });
            cart.AddProduct(new Product { Quantity = 4, SKU_Id = 'C' });

            var cashier = new Cashier(cart, promotionalOffer, skuList);

            // Act
            var result = cashier.CheckOut();

            // Assert
            Assert.Equal(380, result);
        }


        [Fact]
        public void CheckOut_GivenCartWithItemEligibleUnderPromotion_ShouldReturnTotalCostOfTheCartItemsWithBestDiscount()
        {
            // Arrange
            var skuList = new List<SKU>
            {
                new SKU('A', 50), new SKU('B', 30), new SKU('C', 20),new SKU('D', 15)
            };
            var promotionalOffer = new PromotionalOffer(skuList);
            promotionalOffer.AddOffer(new Offer { Price = 130, Products = new List<Product> { new Product { Quantity = 3, SKU_Id = 'A' } } });
            promotionalOffer.AddOffer(new Offer { Price = 30, Products = new List<Product> { new Product { Quantity = 1, SKU_Id = 'C' }, new Product { Quantity = 1, SKU_Id = 'D' } } });

            var cart = new Cart(skuList);
            cart.AddProduct(new Product { Quantity = 4, SKU_Id = 'A' });
            cart.AddProduct(new Product { Quantity = 2, SKU_Id = 'C' });
            cart.AddProduct(new Product { Quantity = 3, SKU_Id = 'D' });

            var cashier = new Cashier(cart, promotionalOffer, skuList);

            // Act
            var result = cashier.CheckOut();

            // Assert
            Assert.Equal(255, result);
        }
    }
}
