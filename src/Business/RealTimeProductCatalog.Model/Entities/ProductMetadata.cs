namespace RealTimeProductCatalog.Model.Entities
{
    public class ProductMetadata
    {
        public string Id => GetId(this.ProductNumber, this.TenantId);
        public Guid ProductId { get; set; }
        public int ProductNumber { get; set; }
        public int TenantId { get; set; }        
        public Color? Color { get; set; }
        public Brand? Brand { get; set; }
        public Stock? Stock { get; set; }
        public IEnumerable<string> StoreIds { get; set; } = new List<string>();
        public IEnumerable<ProductionItem> ProductionItems { get; set; } = new List<ProductionItem>();

        public static string GetId(int productNumber, int tenantId)
        {
            return $"{productNumber}-{tenantId}";
        }  
    }
}