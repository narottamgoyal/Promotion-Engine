using PromotionEngineApp.Model;

namespace PromotionEngineApp
{
    public class ProductState
    {
        public Product Product { get; private set; }
        public bool IsProcessed { get; set; }
        public ProductState(Product product, bool isProcessed)
        {
            Product = product;
            IsProcessed = isProcessed;
        }
    }
}
