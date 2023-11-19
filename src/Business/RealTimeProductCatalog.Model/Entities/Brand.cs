namespace RealTimeProductCatalog.Model.Entities
{
    public class Brand
    {
        public Brand(string name, List<Product> product)
        {
            BrandId = Guid.NewGuid();
            Name = name;
            Product = new List<Product>();
        }

        public Guid BrandId { get; private set; }
        public string Name { get; private set; }
        public List<Product> Product { get; private set; }

        public void AddProduct(Product product)
        {
            Product.Add(product);
        }
    }
}