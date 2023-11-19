using RealTimeProductCatalog.Application.Dtos;
using RealTimeProductCatalog.Model.Entities;
using RealTimeProductCatalog.Model.Enums;

namespace RealTimeProductCatalog.Application.Map
{
    public static class ProductMap
    {
        public static Product Map(ProductInput productInput)
        {
            // var brand = new Brand(productInput.BrandName);
            // var product = new Product(productInput.Name, brand);

            // foreach (var color in productInput.Colors)
            // {
            //     var colorType = ColorTypes.GetColorType(color.Name);
            //     var colorEntity = new Color(colorType, color.Number, color.Name, color.Visible);
            //     product.AddColor(colorEntity);
            // }

            // foreach (var metadata in productInput.Metadata)
            // {
            //     var metadataEntity = new ProductMetadata(metadata.Key, metadata.Value);
            //     product.AddMetadata(metadataEntity);
            // }

            return new Product("name", new Brand("brand", new List<Product>()));
        }
    }
}