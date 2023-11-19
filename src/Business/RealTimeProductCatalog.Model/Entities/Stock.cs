namespace RealTimeProductCatalog.Model.Entities
{
    public class Stock
    {
        public Stock(int stockPointId, Guid variantId, int quantity, int preOrderQuantity, int? concessionQuantity, int? unaccessibleQuantity)
        {
            StoreId = Guid.NewGuid();
            StockPointId = stockPointId;
            VariantId = variantId;
            Quantity = quantity;
            PreOrderQuantity = preOrderQuantity;
            ConcessionQuantity = concessionQuantity;
            UnaccessibleQuantity = unaccessibleQuantity;
            Items = new List<ProductionItem>();
        }

        public Guid StoreId { get; private set; }        
        public int StockPointId { get; private set; }        
        public Guid VariantId { get; private set; }        
        public int Quantity { get; private set; }        
        public int PreOrderQuantity { get; private set; }        
        public int? ConcessionQuantity { get; private set; }        
        public int? UnaccessibleQuantity { get; private set; }
        public List<ProductionItem> Items { get; private set; }

        internal void AddProductionItem(ProductionItem productionItem)
        {
            Items.Add(productionItem);
        }
    }
}