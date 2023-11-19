using System.Text.Json;
using FluentValidation.Results;
using RealTimeProductCatalog.Application.Dtos;
using RealTimeProductCatalog.Application.Map;
using RealTimeProductCatalog.Application.Validation;
using RealTimeProductCatalog.Model.Entities;
using RealTimeProductCatalog.Producer;

namespace RealTimeProductCatalog.Application.Service
{
    public class ProductHandler: IProductHandler
    {
        private readonly IPublisher _publisher;

        public ProductHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task<ValidationResult> Handle(Product product)
        {            
            var validator = new ProductValidator();
            var validationResult = await validator.ValidateAsync(product);

            return validationResult;
        }

        public async Task<bool> PublishMessage(Product product)
        {
            return  await _publisher.StartSendingMessages(JsonSerializer.Serialize(product));
        }
    }
}