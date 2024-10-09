namespace Product.Catalog.Supplier.DataContracts
{
    public record ProductDto(string StreamName, string MessageId, string CorrelationId, string PartitionKey, string MessageType, string Timestamp, object Payload);    
}
