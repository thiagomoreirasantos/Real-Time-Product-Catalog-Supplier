namespace RealTimeProductCatalog.Model.Entities
{
    public class Product
    {
        public Product(string name, Brand brand)
        {
            Id = Guid.NewGuid();
            Name = name;
            Brand = brand;
            Metadata = new List<ProductMetadata>();
            ProductionItems = new List<ProductionItem>();
            AvailableColors = new List<Color>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Brand Brand { get; private set; }
        public List<Color> AvailableColors { get; private set; }
        public List<ProductMetadata> Metadata { get; private set; }
        public List<ProductionItem> ProductionItems { get; private set; }

        public void AddMetadata(ProductMetadata metadata)
        {
            Metadata.Add(metadata);
        }

        public void AddProductionItem(ProductionItem productionItem)
        {
            ProductionItems.Add(productionItem);
        }

        public void AddColor(Color color)
        {
            AvailableColors.Add(color);
        }
    }
}