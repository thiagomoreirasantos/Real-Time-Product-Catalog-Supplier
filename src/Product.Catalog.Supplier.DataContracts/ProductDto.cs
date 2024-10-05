namespace Product.Catalog.Supplier.DataContracts
{
    public record ProductDto
    {
        public required string StreamName { get; init; }

        public required string MessageId { get; init; }

        public required string CorrelationId { get; init; }

        public required string PartitionKey { get; init; }

        public required string MessageType { get; init; }

        public required string Timestamp { get; init; }

        public required object Payload { get; init; }
    }
}
