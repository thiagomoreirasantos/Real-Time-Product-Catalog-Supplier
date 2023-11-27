namespace RealTimeProductCatalog.Application.Map
{
    public static class ProductMap
    {
        public static Product Map(ProductInput input)
        {
            var brand = new Brand(input.BrandName, new List<Product>());
            var color = new Color(input.ColorId, (ColorTypes)input.ColorType, input.Number, input.ColorName, input.Visible);
            var product = new Product(input.Name, brand);
            product.AddColor(color);

            foreach (var brandProduct in input.BrandProducts)
            {
                var relatedProduct = new Product(brandProduct, brand);
                brand.AddProduct(relatedProduct);
            }

            var stock = new Stock(input.StockPointId, input.StockVariantId, input.StockQuantity, input.StockPreOrderQuantity, input.StockConcessionQuantity, input.StockUnaccessibleQuantity);

            var productionItem = new ProductionItem(input.ProductionItemCreatedDate, product, stock);
            product.AddProductionItem(productionItem);

            var metadata = new ProductMetadata(input.ProductMetadataKey, input.Id, input.ProductMetadaProductNumber, input.ProductMetadaTenantId, color, brand, stock, input.StoreIds, product.ProductionItems);
            product.AddMetadata(metadata);

            return product;
        }
    }
}