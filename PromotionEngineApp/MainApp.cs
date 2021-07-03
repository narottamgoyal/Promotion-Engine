using PromotionEngineApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PromotionEngineApp
{
    [ExcludeFromCodeCoverage]
    public class MainApp
    {
        private static void Main(string[] args)
        {
            // Add all possible SKU's
            var skuList = new List<SKU>
            {
                new SKU('A', 50),
                new SKU('B', 30),
                new SKU('C', 20),
                new SKU('D', 15)
            };

            // Create promotional offers
            var promotionEngine = GetPromotionOffer(skuList);

            // Scenario - 1
            var cart1 = new Cart(skuList);
            cart1.AddProduct(new Product { Quantity = 1, SKU_Id = 'A' });
            cart1.AddProduct(new Product { Quantity = 1, SKU_Id = 'B' });
            cart1.AddProduct(new Product { Quantity = 1, SKU_Id = 'C' });
            var cashier = new Cashier(cart1, promotionEngine, skuList);
            Console.WriteLine("Order 1: " + cashier.CheckOut());

            // Scenario - 2
            var cart2 = new Cart(skuList);
            cart2.AddProduct(new Product { Quantity = 5, SKU_Id = 'A' });
            cart2.AddProduct(new Product { Quantity = 5, SKU_Id = 'B' });
            cart2.AddProduct(new Product { Quantity = 1, SKU_Id = 'C' });
            cashier = new Cashier(cart2, promotionEngine, skuList);
            Console.WriteLine("Order 2: " + cashier.CheckOut());

            // Scenario - 3
            var cart3 = new Cart(skuList);
            cart3.AddProduct(new Product { Quantity = 3, SKU_Id = 'A' });
            cart3.AddProduct(new Product { Quantity = 5, SKU_Id = 'B' });
            cart3.AddProduct(new Product { Quantity = 1, SKU_Id = 'C' });
            cart3.AddProduct(new Product { Quantity = 1, SKU_Id = 'D' });
            cashier = new Cashier(cart3, promotionEngine, skuList);
            Console.WriteLine("Order 3: " + cashier.CheckOut());
        }

        private static PromotionalOffer GetPromotionOffer(List<SKU> skuList)
        {
            var promotionalOffer = new PromotionalOffer(skuList);
            promotionalOffer.AddOffer(new Offer
            {
                Price = 130,
                Products = new List<Product>
                {
                    new Product { Quantity = 3, SKU_Id = 'A' }
                }
            });

            promotionalOffer.AddOffer(new Offer
            {
                Price = 45,
                Products =
                new List<Product>
                {
                    new Product { Quantity = 2, SKU_Id = 'B' }
                }
            });

            promotionalOffer.AddOffer(new Offer
            {
                Price = 30,
                Products = new List<Product>
                {
                    new Product { Quantity = 1, SKU_Id = 'C' },
                    new Product { Quantity = 1, SKU_Id = 'D' }
                }
            });

            return promotionalOffer;
        }
    }
}
