namespace RealTimeProductCatalog.Model.Entities
{
    public class Stock
    {        
        public Guid StoreId { get; set; }        
        public int StockPointId { get; set; }        
        public Guid VariantId { get; set; }        
        public int Quantity { get; set; }        
        public int PreOrderQuantity { get; set; }        
        public int? ConcessionQuantity { get; set; }        
        public int? UnaccessibleQuantity { get; set; }
    }
}