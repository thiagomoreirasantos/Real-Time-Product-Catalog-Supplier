namespace RealTimeProductCatalog.Model.Entities
{
    public class ProductionItem
    {
        public ProductionItem(long createdDate, Product product, Stock stock)
        {
            Id = Guid.NewGuid();
            CreatedDate = createdDate;
            Product = product;
            Stock = stock;
        }

        public Guid Id { get; private set; }        
        public long CreatedDate { get; private set; }
        public Product Product { get; private set; }
        public Stock Stock { get; private set; }

        public void AssignToStock(Stock stock)
        {
            Stock = stock;

            stock.AddProductionItem(this);
        }
    }
}