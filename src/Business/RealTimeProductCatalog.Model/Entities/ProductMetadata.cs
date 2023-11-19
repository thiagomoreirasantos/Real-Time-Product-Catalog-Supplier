namespace RealTimeProductCatalog.Model.Entities
{
    public class ProductMetadata
    {
        public ProductMetadata(string key, Guid productId, int productNumber, int tenantId, Color? color, Brand? brand, Stock? stock, IEnumerable<string> storeIds, IEnumerable<ProductionItem> productionItems)
        {
            Key = key;
            ProductId = productId;
            ProductNumber = productNumber;
            TenantId = tenantId;
            Color = color;
            Brand = brand;
            Stock = stock;
            this.StoreIds = new List<string>();
            this.ProductionItems = new List<ProductionItem>();
        }        

        public string Id => GetId(this.ProductNumber, this.TenantId);
        public string Key { get; private set; }
        public Guid ProductId { get; private set; }
        public int ProductNumber { get; private set; }
        public int TenantId { get; private set; }        
        public Color? Color { get; private set; }
        public Brand? Brand { get; private set; }
        public Stock? Stock { get; private set; }
        public IEnumerable<string> StoreIds { get; private set; }
        public IEnumerable<ProductionItem> ProductionItems { get; private set; }

        public static string GetId(int productNumber, int tenantId)
        {
            return $"{productNumber}-{tenantId}";
        }  

        public bool IsValidKey()
        {
            var validKeys = new HashSet<string>
            {
                "Weight", "Size", "Material", "Color"
            };

            return validKeys.Contains(this.Key);
        }
    }
}