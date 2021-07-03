using PromotionEngineApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngineApp
{
    public class Cashier
    {
        private readonly List<Offer> _promotionalItems;
        private readonly List<SKU> _skuList;
        private readonly Cart _cart;

        public Cashier(Cart cart, PromotionalOffer promotionEngine, List<SKU> skuList)
        {
            _skuList = skuList;
            _cart = cart;
            _promotionalItems = promotionEngine.GetPromotions();
        }

        public double CheckOut()
        {
            var productList = _cart.GetAddedItems().ConvertAll(product => new ProductState(product, false));
            var total = 0.0;
            for (int i = 0; i < productList.Count; i++)
            {
                var currentProduct = productList[i];
                if (currentProduct.IsProcessed == true) continue;
                var filteredPromotionalItems = _promotionalItems.Where(x => x.Products.Any(y => y.SKU_Id == currentProduct.Product.SKU_Id && y.Quantity <= currentProduct.Product.Quantity));

                // No offer
                if (!filteredPromotionalItems.Any())
                {
                    // full price
                    total += _skuList.First(x => x.Id == currentProduct.Product.SKU_Id).Price * currentProduct.Product.Quantity;
                    currentProduct.IsProcessed = true;
                }
                else
                {
                    // list of offers
                    foreach (var promotionalItem in filteredPromotionalItems)
                    {
                        // single product offer

                        if (promotionalItem.Products.Count == 1)
                        {
                            // process by quantity

                            var quantity = currentProduct.Product.Quantity / promotionalItem.Products[0].Quantity;
                            total += quantity * promotionalItem.Price;
                            var remainingProductQuantity = GetRemainingProductQuantity(currentProduct.Product.Quantity, promotionalItem.Products[0].Quantity);
                            if (remainingProductQuantity > 0)
                            {
                                productList.Add(new ProductState(new Product { Quantity = remainingProductQuantity, SKU_Id = currentProduct.Product.SKU_Id }, false));
                            }
                            currentProduct.IsProcessed = true;
                        }
                        else
                        {
                            // multi product offer

                            var currentPromotionalItem = promotionalItem.Products.First(x => x.SKU_Id == currentProduct.Product.SKU_Id);
                            var otherPromotionalMandatoryItems = promotionalItem.Products.Where(x => x.SKU_Id != currentProduct.Product.SKU_Id);
                            var otherUnprocessedProduct = productList.Where(x => x.IsProcessed == false & otherPromotionalMandatoryItems.All(y => x.Product.Quantity >= y.Quantity && x.Product.SKU_Id == y.SKU_Id));
                            if (otherUnprocessedProduct.Any())
                            {
                                // process by quantity
                                foreach (var promotionalItemProduct in otherPromotionalMandatoryItems)
                                {
                                    var unprocessedProduct = otherUnprocessedProduct.First(x => x.Product.SKU_Id == promotionalItemProduct.SKU_Id);
                                    var remainingOtherProductQuantity = GetRemainingProductQuantity(unprocessedProduct.Product.Quantity, promotionalItemProduct.Quantity);
                                    if (remainingOtherProductQuantity > 0)
                                    {
                                        productList.Add(new ProductState(new Product { Quantity = remainingOtherProductQuantity, SKU_Id = unprocessedProduct.Product.SKU_Id }, false));
                                    }
                                    unprocessedProduct.IsProcessed = true;
                                }

                                var remainingProductQuantity = GetRemainingProductQuantity(currentProduct.Product.Quantity, currentPromotionalItem.Quantity);
                                if (remainingProductQuantity > 0)
                                {
                                    productList.Add(new ProductState(new Product { Quantity = remainingProductQuantity, SKU_Id = currentProduct.Product.SKU_Id }, false));
                                }
                                total += promotionalItem.Price;
                                currentProduct.IsProcessed = true;
                            }
                        }
                    }
                }
            }

            // Process unprocess remaining items
            foreach (var product in productList.Where(x => x.IsProcessed == false))
            {
                // full price
                total += _skuList.First(x => x.Id == product.Product.SKU_Id).Price * product.Product.Quantity;
                product.IsProcessed = true;
            }

            return total;
        }

        private int GetRemainingProductQuantity(int productQuantity, int promotionalQuantity)
        {
            if (promotionalQuantity == 1) return productQuantity - promotionalQuantity;
            return productQuantity % promotionalQuantity;
        }
    }
}
