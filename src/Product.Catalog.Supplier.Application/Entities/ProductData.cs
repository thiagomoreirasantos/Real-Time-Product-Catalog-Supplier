namespace Product.Catalog.Supplier.Application.Entities
{
    public class ProductData
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public decimal Price { get; init; }
        public required string Category { get; init; }
    }
}
