using System.Collections.Generic;

namespace PromotionEngineApp.Model
{
    public class Offer
    {
        public double Price { get; set; }
        public List<Product> Products { get; set; }
    }
}
