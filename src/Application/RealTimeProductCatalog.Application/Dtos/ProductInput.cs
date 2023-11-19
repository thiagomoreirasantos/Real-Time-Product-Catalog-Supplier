namespace RealTimeProductCatalog.Application.Dtos
{
    public class ProductInput
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid BrandId { get; set; }
        public required string BrandName { get; set; }        
        public required List<string> BrandProducts { get; set; }
        public Guid ColorId { get; set; }
        public int ColorType { get; set; }
        public int Number { get; set; }
        public required string ColorName { get; set; }
        public bool Visible { get; set; }
        public Guid ProductionItemId { get; set; }        
        public long ProductionItemCreatedDate { get; set; }
        public required string ProductMetadataProductNumber { get; set; }
        public required string ProductMetadataKey { get; set; }        
        public int ProductMetadaProductNumber { get; set; }
        public int ProductMetadaTenantId { get; set; }
        public required IEnumerable<string> ProductMetadaProductionItems { get; set; }
        public required IEnumerable<string> StoreIds { get; set; }
        public Guid StoreId { get; set; }        
        public int StockPointId { get; set; }        
        public Guid StockVariantId { get; set; }        
        public int StockQuantity { get; set; }        
        public int StockPreOrderQuantity { get; set; }        
        public int? StockConcessionQuantity { get; set; }        
        public int? StockUnaccessibleQuantity { get; set; }
        public required List<string> StockProductItems { get; set; }
    }
}