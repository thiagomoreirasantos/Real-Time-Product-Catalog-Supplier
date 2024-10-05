using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Product.Catalog.Supplier.DataContracts
{
    public class MessageDataHeaders
    {
        /// <summary>
        /// The message identifier.
        /// </summary>
        /// <example>352</example>
        [FromHeader(Name = "message_id")]
        public required string MessageId { get; set; }

        /// <summary>
        /// ID that can allow to easily track a message between services.
        /// </summary>
        /// <example>90e99df3-4413-4a3f-ae05-646a0790349c</example>
        [FromHeader(Name = "correlation_id")]
        public required string CorrelationId { get; set; }

        /// <summary>
        /// Key that is used to guarantee the order of messages with the same key within a partition.
        /// </summary>
        /// <example>1</example>
        [FromHeader(Name = "partition_key")]
        public  string? PartitionKey { get; set; }

        /// <summary>
        /// Identifies the type of action the event represents (e.g. CustomerCeated, CustomerDeleted, etc). It can be useful to help consumers filter.
        /// </summary>
        /// <example>CustomerCeated</example>
        [SwaggerParameter(Required = true)]
        [FromHeader(Name = "message_type")]
        public required string MessageType { get; set; }

        /// <summary>
        /// This is the timestamp of when the message was sent. It can be useful in some situations, such as when the data is not delivered to consumers immediately. This can occur for a number of reasons such as the retry policy.
        /// </summary>
        /// <example>1695656494</example>
        [FromHeader(Name = "timestamp")]
        public required string Timestamp { get; set; }
    }
}
