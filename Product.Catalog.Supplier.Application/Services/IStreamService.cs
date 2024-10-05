using FluentResults;
using Product.Catalog.Supplier.DataContracts;

namespace Product.Catalog.Supplier.Application.Services
{
    public interface IStreamService
    {
        Task<Result> HandleMessage(ProductDto productData);
    }
}
