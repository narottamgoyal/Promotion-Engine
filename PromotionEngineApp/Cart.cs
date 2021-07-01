using PromotionEngineApp.Model;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngineApp
{
    public class Cart
    {
        private readonly List<Product> _products;
        private readonly List<SKU> _skuList;

        public Cart(List<SKU> skuList)
        {
            _products = new List<Product>();
            _skuList = skuList;
        }

        public void AddProduct(Product product)
        {
            if (!_skuList.Any(x => x.Id == product.SKU_Id)) return;
            var existingProduct = _products.Where(x => x.SKU_Id == product.SKU_Id).FirstOrDefault();
            if (existingProduct == null) { _products.Add(product); return; }
            existingProduct.Quantity += product.Quantity;
        }

        public List<Product> GetAddedItems()
        {
            return _products;
        }
    }
}
