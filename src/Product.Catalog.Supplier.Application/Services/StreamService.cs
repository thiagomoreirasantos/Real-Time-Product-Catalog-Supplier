using FluentResults;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.Catalog.Supplier.Application.Configuration;
using Product.Catalog.Supplier.Application.Errors;
using Product.Catalog.Supplier.Application.Extensions;
using Product.Catalog.Supplier.Application.Validators;
using Product.Catalog.Supplier.DataContracts;

namespace Product.Catalog.Supplier.Application.Services
{
    public class StreamService(ILogger<StreamService> logger, IProducerAccessor producerAccessor, IOptions<ApplicationSettings> options) : IStreamService
    {
        private readonly ILogger<StreamService> _logger = logger;
        private readonly IProducerAccessor _producerAccessor = producerAccessor;
        private readonly ApplicationSettings _settings = options.Value;

        public async Task<Result> HandleMessage(ProductDto productData)
        {
            MessageEnvelopeValidator _validator = new();
            var validationResult = await _validator.ValidateAsync(productData);
            var result = validationResult.ToFluentResult();

            if (result.IsFailed)
            {
                return result;
            }

            var headers = new MessageHeaders();
            headers.SetString("message_id", productData.MessageId);
            headers.SetString("message_type", productData.MessageType);
            headers.SetString("correlation_id", productData.CorrelationId);
            headers.SetString("timestamp_origin", productData.Timestamp.ToString());
            headers.SetString("partition_key", productData.PartitionKey);

            var producer = _producerAccessor.GetProducer(productData.StreamName);
            if (producer is null)
            {
                return Result.Fail(new NotFoundError($"Invalid Stream Name {productData.StreamName}"));
            }

            try
            {
                var deliveryReport = await producer.ProduceAsync(productData.StreamName, productData.PartitionKey, productData.Payload, headers);
                _logger.LogInformation("Message produced to {Topic}/{Partition} with offset {Offset}", deliveryReport.Topic, deliveryReport.Partition.Value, deliveryReport.Offset.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error producing message to {productData.StreamName}: {ex.Message}");
                return Result.Fail(new UnknownError($"Error producing message to {productData.StreamName}"));
            }
            return result;
        }

    }
}
