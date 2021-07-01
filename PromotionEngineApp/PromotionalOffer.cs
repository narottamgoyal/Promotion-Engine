using PromotionEngineApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngineApp
{
    public class PromotionalOffer
    {
        private readonly List<Offer> _offers;
        private readonly List<SKU> _skuList;
        public PromotionalOffer(List<SKU> skuList)
        {
            _skuList = skuList;
            _offers = new List<Offer>();
        }

        public void AddOffer(Offer offer)
        {
            var sku_ids = _skuList.Select(y => y.Id);
            if (!offer.Products.All(x => sku_ids.Any(y => y == x.SKU_Id))) return;
            _offers.Add(offer);
        }

        public List<Offer> GetPromotions()
        {
            var list = new List<(double, Offer)>();
            // Sort by best offer, So that best offer will be selected first
            foreach (var offer in _offers)
            {
                double actualPrice = 0;
                foreach (var product in offer.Products)
                {
                    var sku = _skuList.First(x => x.Id == product.SKU_Id);
                    actualPrice += sku.Price * product.Quantity;
                }
                var discountValue = actualPrice - offer.Price;
                var discountPercent = (discountValue * 100) / actualPrice;

                list.Add((discountPercent, offer));
            }
            return list.OrderByDescending(x => x.Item1).Select(y => y.Item2).ToList();
        }
    }
}
