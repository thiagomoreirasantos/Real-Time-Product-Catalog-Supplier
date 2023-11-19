using FluentValidation.Results;
using RealTimeProductCatalog.Model.Entities;

namespace RealTimeProductCatalog.Application.Service
{
    public interface IProductHandler
    {
        Task<ValidationResult> Handle(Product product);
        Task<bool> PublishMessage(Product product);
    }
}