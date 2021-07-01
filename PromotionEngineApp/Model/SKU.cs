namespace PromotionEngineApp.Model
{
    public class SKU
    {
        public char Id { get; private set; }
        public double Price { get; private set; }
        public SKU(char id, double price)
        {
            Id = id;
            Price = price;
        }
    }
}
